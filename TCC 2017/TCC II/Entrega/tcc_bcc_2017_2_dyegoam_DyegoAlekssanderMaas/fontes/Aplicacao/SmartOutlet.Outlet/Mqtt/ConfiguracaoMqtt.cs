using System;
using System.Configuration;

namespace SmartOutlet.Outlet.Mqtt
{
    public class ConfiguracaoMqtt
    {
        public string BrokerHostName { get; }
        public int BrokerPort { get; }

        public ConfiguracaoMqtt()
        {
            var mqttSection = ConfigurationManager.GetSection("mqtt") as System.Collections.Specialized.NameValueCollection;
            if (mqttSection == null)
            {
                throw new InvalidOperationException("Configuração válida do MQTT não encontrada.");
            }

            BrokerHostName = mqttSection.Get("BrokerHostName");
            BrokerPort = int.Parse(mqttSection.Get("BrokerPort"));
        }
    }
}