#include "SensoresCorrente.h"
#include "Utils.h"

namespace SensoresCorrente{
  ACS712::ACS712(ModuleType moduleType)
  {
  	switch (moduleType)
  	{
  		case _30A: mVperAmp = 0.066; break;
  		case _20A: mVperAmp = 0.1; break;
  		case _5A:
  		default: mVperAmp = 0.185; break;
  	}
  }
  
  DCReading ACS712::readDC(int analogInPin) const
  {
  	auto rawValue = analogRead(analogInPin);
  	auto voltage = rawValue / 1024.0 * 5000; // mV
  	auto amps = (voltage - ACSoffset) / mVperAmp;
  	auto power = Utils::computePower(amps, voltage);
  	return DCReading(amps, voltage, power);
  }

  int ACReadCount = 0;    
  int middleValue = 510;

  //it assumes that nothing is connected yet
  void ACS712::calibrate(int analogInPin) {   
    Serial.println("Initializing calibration process"); 
    auto middle = 510;
    this->isCalibrated = false;    
    while (!isCalibrated) {
      int readings = 1500;  
      int sum = 0;
      for (int i = 0; i < readings; i++) {
        int rawValue = analogRead(analogInPin);  
        sum += rawValue;  
        delay(1);
      }
      int average = sum / readings;
      middle = average;
      Serial.print("New middle:");
      Serial.print(middle);

      // TODO add validation
      middleValue = middle;
      this->isCalibrated = true;
    }
  }
  
  ACReading ACS712::readAC(int analogInPin)
  {
    if(!isCalibrated)
      calibrate(analogInPin);
      
    double sensorValue = 0.0;
    int numeroLeituras = 1000;  
      
    for (int i = 0; i < numeroLeituras; i++) {
      int rawValue = analogRead(analogInPin);
         int auxValue = rawValue - middleValue;

      // somam os quadrados das leituras.
      sensorValue += pow(auxValue, 2);
      delay(1);
    }



    Serial.print("middle: ");
    Serial.println(middleValue);
    
    Serial.print("somou: ");
    Serial.println(sensorValue);
    // finaliza o calculo da média quadratica e ajusta o valor lido para volts
    //double voltsporUnidade = 0.002737;
    double voltsporUnidade = 0.0048875855;// 5/1024;
    sensorValue = (sqrt(sensorValue / numeroLeituras)) * voltsporUnidade;
    Serial.print("calculou: ");
    Serial.println(sensorValue);
    double sensibility = mVperAmp;
    double amps = (sensorValue/sensibility);
    Serial.print("amps: ");
    Serial.println(amps);
   
    //tratamento para possivel ruido
    //O ACS712 para 30 Amperes é projetado para fazer leitura
    // de valores alto acima de 0.25 Amperes até 30.
    // por isso é normal ocorrer ruidos de até 0.20A
    //por isso deve ser tratado
    if(amps <= 0.095){     
      amps = 0;
    }

    if (!isCalibrated) {
      Serial.println("not calibrated yet");
      return ACReading(0, 220, 0);  
    }

    return ACReading(amps, 220, amps*220);
  }
}
