using System;
using System.Globalization;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using FluentAssertions;
using NUnit.Framework;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace SmartOutlet.Outlet.Tests.Integration
{
    public class MqttTests
    {

//        [Test]
//        public void conversao()
//        {
//            Convert.ToDouble("20.5", CultureInfo.InvariantCulture).Should().Be(20.5d);
//        }
        
        [Test, Ignore("publishes real messages")]
        public void publishing_a_message()
        {
            var client = new MqttClient(
//                brokerHostName:"localhost",
                brokerHostName:"localhost",
                brokerPort: 1883,
                secure: false,
                sslProtocol: MqttSslProtocols.None,
                userCertificateSelectionCallback: null,
                userCertificateValidationCallback: null
            ); 
//            var client = new MqttClient(IPAddress.Parse("10.0.0.9")); 
            

 
            var clientId = Guid.NewGuid().ToString();
            var connected = client.Connect(clientId);
            client.IsConnected.Should().BeTrue();
            client.MqttMsgPublished += (sender, args) =>
            {
                Console.WriteLine($"Message published {args.MessageId}");
            };

            client.MqttMsgSubscribed += (sender, args) =>
            {
                Console.WriteLine($"Subscription: {args.MessageId}");
            };

            client.ConnectionClosed += (sender, args) =>
            {
                Console.WriteLine("Connection closed");
            };
            
            
            
            client.MqttMsgPublishReceived += (sender, args) =>
            {
                Console.WriteLine($"message received: {Encoding.UTF8.GetString(args.Message)}, duplicated? {args.DupFlag}");
            }; 
            var subscriptionId = client.Subscribe(new string[] { "/home/temperature" }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE });
            Console.WriteLine($"Subscription Id: {subscriptionId}");
 
// publish a message on "/home/temperature" topic with QoS 2 
            var messageId = client.Publish("/home/temperature", Encoding.UTF8.GetBytes("25"), MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE, true);
            messageId = client.Publish("/home/temperature", Encoding.UTF8.GetBytes("25"), MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE, true);
            messageId = client.Publish("/home/temperature", Encoding.UTF8.GetBytes("25"), MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE, true);
            messageId = client.Publish("/home/temperature", Encoding.UTF8.GetBytes("25"), MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE, true);
            messageId = client.Publish("/home/temperature", Encoding.UTF8.GetBytes("25"), MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE, true);
            messageId = client.Publish("/home/temperature", Encoding.UTF8.GetBytes("25"), MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE, true);
            messageId = client.Publish("/home/temperature", Encoding.UTF8.GetBytes("25"), MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE, true);
            messageId = client.Publish("/home/temperature", Encoding.UTF8.GetBytes("25"), MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE, true);
            messageId = client.Publish("/home/temperature", Encoding.UTF8.GetBytes("25"), MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE, true);
            messageId = client.Publish("/home/temperature", Encoding.UTF8.GetBytes("25"), MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE, true);
            messageId = client.Publish("/home/temperature", Encoding.UTF8.GetBytes("25"), MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE, true);
            Console.WriteLine($"Publish MessageId {messageId}");

            Thread.Sleep(3000);
            
            client.Disconnect();

//            Console.Read();

// register to message received 


// subscribe to the topic "/home/temperature" with QoS 2 
        }
        
        static void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e) 
        {
// handle message received
        } 
    }
}