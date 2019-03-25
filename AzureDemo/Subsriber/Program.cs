using Microsoft.Azure;
using Microsoft.Azure.ServiceBus;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Subsriber
{
    class Program
    {
        private static string topicName = "test";
        private static string subscription = "listenerName";
        private static string serviceBusConnectionstring = "PrimaryConnectionstring"; //can but it on confing and read
        private static string message = "test message";
        static void Main(string[] args)
        {
            var serviceBusConnectionString = ConfigurationManager.AppSettings["serviceBusConnectionstring"]; //or 
            //you can directly read from cloud 
            //Microsoft.WindowsAzure.ConfigurationManager
            //Microsoft Azure Configuration Manager provides a unified API to load configuration settings regardless of where the application is hosted - whether on-premises or in a Cloud Service.
            var serviceBusConnectionString1 = CloudConfigurationManager.GetSetting("serviceBusConnectionstring");

            var subscriptionCleint = new SubscriptionClient(serviceBusConnectionString, topicName, subscription);

            //read message and write to console app
            subscriptionCleint.RegisterMessageHandler(async (msg, concelationtoken) =>
            {
                var body = Encoding.UTF8.GetString(msg.Body);
                Console.WriteLine(body);
            },
            async exception =>
            {
                //log exception
            });


            // for queuss
            // Microsoft.ServiceBus.Messsaging
            var queueName = "testQueue";
            var queueClient = Microsoft.ServiceBus.Messaging.QueueClient.CreateFromConnectionString(serviceBusConnectionString, queueName);
            queueClient.OnMessage(message =>
            {
                Console.WriteLine($"Message body:{message.GetBody<string>()}");
                Console.WriteLine($"Message Id: {message.MessageId}");
            });
            Console.ReadLine();
        }
    }
}
