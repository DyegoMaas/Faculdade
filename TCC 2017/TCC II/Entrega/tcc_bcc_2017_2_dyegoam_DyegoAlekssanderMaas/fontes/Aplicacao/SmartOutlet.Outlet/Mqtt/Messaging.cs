using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Polly;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace SmartOutlet.Outlet.Mqtt
{
    /*Public brokers:
        broker.hivemq.com (port 1883).
        broker.mqttdashboard.com (port 1883).
        iot.eclipse.org (port 1883).
        test.mosca.io (port 1883).
     */
    public class Messaging : IPublisher, ITopicGuest, IDisposable
    {
        private const int BrokerPort = 1883;
        private const byte QosLevel = MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE;
        private static readonly Guid ClientId = Guid.NewGuid();
        
        private static readonly Dictionary<string, List<Action<string>>> CallbacksPerTopic = new Dictionary<string, List<Action<string>>>();
        private readonly MqttClient _mqttClient;
        private readonly ConfiguracaoMqtt _configuracaoMqtt;

        public Messaging()
        {
            _configuracaoMqtt = new ConfiguracaoMqtt();
            _mqttClient = new MqttClient(
                _configuracaoMqtt.BrokerHostName ?? "iot.eclipse.org",
                _configuracaoMqtt.BrokerPort,
                secure: false,
                sslProtocol: MqttSslProtocols.None,
                userCertificateSelectionCallback: null,
                userCertificateValidationCallback: null
            );

            TryToConnect();
            
            _mqttClient.MqttMsgPublished += (sender, args) =>
            {
                Console.WriteLine($"Message published: {args.MessageId}");
            };

            _mqttClient.MqttMsgPublishReceived += (sender, args) =>
            {
                var message = Encoding.UTF8.GetString(args.Message);
                Console.WriteLine($"Message publish received: {message} in topic {args.Topic}");
                
                if (CallbacksPerTopic.ContainsKey(args.Topic))
                {
                    var callbacks = CallbacksPerTopic[args.Topic];
                    foreach (var callback in callbacks)
                    {
                        callback.Invoke(message);
                    }
                }
            };

            _mqttClient.ConnectionClosed += (sender, args) =>
            {
                Console.WriteLine("MQTT connection closed.");
                TryToConnect();
            };

            _mqttClient.MqttMsgUnsubscribed += (sender, args) =>
            {
                Console.WriteLine($"MQTT unsubscription: {args.MessageId}");
            };
            
            _mqttClient.MqttMsgSubscribed += (sender, args) =>
            {
                Console.WriteLine($"MQTT subscription: {args.MessageId}");
            };
        }

        private void TryToConnect()
        {
            Policy
                .Handle<Exception>()
                .WaitAndRetryForever(
                    count => TimeSpan.FromSeconds(1), 
                    (exception, retryCount) =>
                    {
                        Console.WriteLine($"Not able to connect to MQTT broker. Retrying for the {retryCount} time");
                    })
                .Execute(() =>
                {
                    _mqttClient.Connect(ClientId.ToString());
                    Thread.Sleep(500);
                    if (_mqttClient.IsConnected)
                    {
                        Console.WriteLine("MQTT connected");
                        return;
                    }

                    var message = $"Not connected to MQTT broker '{_configuracaoMqtt.BrokerHostName}' on port '{BrokerPort}'";
                    throw new InvalidOperationException(message);
                });
        }

        public void Publish(string topic, string message)
        {
            var bytes = Encoding.UTF8.GetBytes(message);
            _mqttClient.Publish(topic, bytes, QosLevel, true);
        }

        public void Subscribe(string topic, Action<string> onMessageReceived)
        {
            Subscribe(new [] {topic}, onMessageReceived);           
        }
        
        public void Subscribe(string[] topics, Action<string> onMessageReceived)
        {
            foreach (var topic in topics)
            {
                RegisterCallback(onMessageReceived, topic);
            }
            _mqttClient.Subscribe(topics, new[] { QosLevel });            
        }

        private static void RegisterCallback(Action<string> onMessageReceived, string topic)
        {
            if (!CallbacksPerTopic.ContainsKey(topic))
                CallbacksPerTopic.Add(topic, new List<Action<string>>());

            CallbacksPerTopic[topic].Add(onMessageReceived);
        }

        public void Dispose()
        {
            _mqttClient.Disconnect();
        }
    }
}