#include <ESP8266WiFi.h>
#include <PubSubClient.h>
#include "Relay5V.h"
#include "SensoresCorrente.h"
#include "EEPROM_Manager.h"
using namespace SensoresCorrente;
#include "Scheduler.h"

const char* SSID = "Connectify-me"; 
const char* PASSWORD = "wytaxyx8"; 

const char* BROKER_MQTT = "192.168.137.1"; //"10.0.0.7"; // "iot.eclipse.org"; 
int BROKER_PORT = 1883; 

void initPins();
void initSerial();
void initWiFi();
void initMQTT();

WiFiClient espClient;
PubSubClient MQTT(espClient);
Scheduler scheduler = Scheduler();
EEPROM_Manager eepromManager;

int WIFI_LED = D7;
int RELAY_PIN = D5;
Relay5V relay = Relay5V(true);

int AC_SENSOR_PIN = A0;
ACS712 currentSensor = ACS712(_30A);

bool hasCredentials = false;
String id = "";
long lastMessageTime = 0;

void reportConsumption() {
  if (!hasCredentials) {
    // Serial.println("Nothing to report, because I don't have credentials");
    delay(200);
    return;
  }
  
  auto timeSinceLastConsumptionReport = millis() - lastMessageTime;  
  if (timeSinceLastConsumptionReport >= 5000) {
    auto reading = currentSensor.readAC(AC_SENSOR_PIN);

    char power[10];
    dtostrf(reading.power, 7, 2, power);
    char amps[10];
    dtostrf(reading.amps, 7, 2, amps);
    char voltage[10];
    dtostrf(reading.voltage, 7, 2, voltage);

    String payload = id;
    payload += '|';
    payload += amps;
    payload += '|';
    payload += voltage;
    payload += '|';    
    payload += power;

    
    Serial.print("Payload medicao: ");
    Serial.println(payload);
    

    MQTT.publish("/smart-things/plug/consumption", (char *)payload.c_str());
    lastMessageTime = millis();
  }
}

void initCredentials() {
  Serial.println("INITILIASING CREDENTIALS: ");
  
  hasCredentials = eepromManager.hasData();
  id = hasCredentials 
    ? eepromManager.loadId() 
    : "";
  Serial.print("loaded credentials:");
  Serial.println(id);  
}

void setup() {
	initPins();
	initSerial();
 
  eepromManager.begin();
  initCredentials();
  
	initWiFi();
	initMQTT();
}

void loop() {
	recconectWiFi();
	if (!MQTT.connected()) {
		reconnectMQTT();
	}	

  MQTT.loop();
  scheduler.update();
  
  reportConsumption();
}

void initPins() {
  pinMode(WIFI_LED, OUTPUT);
  digitalWrite(WIFI_LED, LOW);
  
  pinMode(RELAY_PIN, OUTPUT);
  relay.turnOff(RELAY_PIN);

  pinMode(AC_SENSOR_PIN, INPUT);
}

void initSerial() {
	Serial.begin(115200);
}

void initWiFi() {
	delay(10);
	Serial.println("Conectando-se em: " + String(SSID));

	WiFi.begin(SSID, PASSWORD);
	while (WiFi.status() != WL_CONNECTED) {
		delay(500);
		Serial.print(".");
	}
	Serial.println();
	Serial.print("Conectado na Rede " + String(SSID) + " | IP => ");
	Serial.println(WiFi.localIP());
 
  digitalWrite(WIFI_LED, HIGH);
}

void initMQTT() {
	Serial.println("Initializing MQTT");
	MQTT.setServer(BROKER_MQTT, BROKER_PORT);
	MQTT.setCallback(mqtt_callback);

	lastMessageTime = millis();
}

void recalibrateCurrentSensor() {
  currentSensor.calibrate(AC_SENSOR_PIN);
}

void turnOn() {
  Serial.print("TURNING ON ");
  Serial.println(id);
  relay.turnOn(RELAY_PIN);
  sendConfirmationOfRelayStateChange();

  //TODO remover
  recalibrateCurrentSensor();
}

void turnOff() {
  Serial.print("TURNING OFF ");
  Serial.println(id);
  relay.turnOff(RELAY_PIN);
  sendConfirmationOfRelayStateChange();
  
  recalibrateCurrentSensor();
}

void sendConfirmationOfRelayStateChange() {
  auto isOn = relay.isOn(RELAY_PIN) ? "on": "off";
  String payload = id;
  payload += '|';
  payload += isOn;

	MQTT.publish("/smart-things/plug/new-state", (char *)payload.c_str());
}

void mqtt_callback(char* topic, byte* payload, unsigned int length) {
	String message;
	for (int i = 0; i < length; i++) {
		char c = (char)payload[i];
		message += c;
	}
 
	auto topicString = String(topic);
	Serial.println("Topic => " + topicString + " | Value => " + message);

  if (topicString == "/smart-things/plug/clean-identity") {
    Serial.println("identity cleared");
    eepromManager.clear(); 
    initCredentials();   
    return;
  } else if (topicString == "/smart-things/plug/activate") {
    if (!hasCredentials) {
      Serial.print("assuming new identity! ");
      auto newId = message;
      Serial.println(newId);      
      eepromManager.saveId(newId);   
      initCredentials();  
      return; 
    }
    else { 
      Serial.print("already have identity! ");
      Serial.println(id);
      return;
    }
  } 

  if (!hasCredentials) {
    Serial.print("No credentials for topic ");
    Serial.println(topic);
    return;
  }

  auto delimiterIndex = message.indexOf('|');
  if (delimiterIndex == -1) {
    Serial.print("No id on topic ");
    Serial.println(topic);
    return;
  }

  auto targetId = message.substring(0, delimiterIndex);
  auto data = message.substring(delimiterIndex + 1);

  Serial.print("Message is intented for ");
  Serial.print(targetId);
  if (id != targetId) {
    Serial.print(", not for me: ");
    Serial.println(id);
    return;
  }
  else {
    Serial.println("it's for me!");
  }
  Serial.print("Message content is ");
  Serial.println(data);

  if (topicString == "/smart-things/plug/calibrate") {
    recalibrateCurrentSensor();
  } else if (topicString == "/smart-things/plug/state") {
		if (data == "turn-on") {
			turnOn();			
		}
		else if (data == "turn-off") {
			turnOff();
		}  
    else {
      Serial.println("nothing to do related to state...");  
    }
	} else if (topicString == "/smart-things/plug/schedule-on") {
    
    auto intervaloMilisegundos = data.toInt();
    Serial.print("ligando em ");
    Serial.print(intervaloMilisegundos);
    Serial.println("ms");
    
    scheduler.schedule(turnOn, intervaloMilisegundos);   
  } else if (topicString == "/smart-things/plug/schedule-off") {

    auto intervaloMilisegundos = data.toInt();
    Serial.print("desligando em ");
    Serial.print(intervaloMilisegundos);
    Serial.println("ms");
    
    scheduler.schedule(turnOff, intervaloMilisegundos);
  }
  else {
    Serial.println("not implemented topic");
  }
	
	Serial.flush();
}

bool isConnectedToWiFi() {
  return WiFi.status() == WL_CONNECTED;
}

void reconnectMQTT() {
	while (isConnectedToWiFi() && !MQTT.connected()) {
		Serial.println("Tentando se conectar ao Broker MQTT: " + String(BROKER_MQTT));
    String clientId = id == "" ? "Unknown plug X" : id;
		if (MQTT.connect((char *)clientId.c_str())) {
			Serial.println("Conectado ao broker MQTT!");
      
      Serial.println("subscribed to /smart-things/plug/clean-identity");
      MQTT.subscribe("/smart-things/plug/clean-identity");
      MQTT.loop();

      Serial.println("subscribed to /smart-things/plug/activate");
      MQTT.subscribe("/smart-things/plug/activate");
      MQTT.loop();

      Serial.println("subscribed to /smart-things/plug/calibrate");
      MQTT.subscribe("/smart-things/plug/calibrate");
      MQTT.loop();
			
			Serial.println("subscribed to /smart-things/plug/state");
			MQTT.subscribe("/smart-things/plug/state");
      MQTT.loop();
      
      Serial.println("subscribed to /smart-things/plug/schedule-on");
      MQTT.subscribe("/smart-things/plug/schedule-on");
      MQTT.loop();
      
      Serial.println("subscribed to /smart-things/plug/schedule-off");
      MQTT.subscribe("/smart-things/plug/schedule-off");
      MQTT.loop();
		}
		else {
			Serial.println("Falha ao Reconectar");
			Serial.println("Tentando se reconectar em 2 segundos");
			delay(2000);
		}
	}
}

void recconectWiFi() {
  int reconnectCount = 0;
	while (!isConnectedToWiFi()) {
		delay(100);
		Serial.print(".");

    if (reconnectCount % 5 == 0)
      digitalWrite(WIFI_LED, LOW);
    else 
      digitalWrite(WIFI_LED, HIGH);
    reconnectCount++;
	}
}
