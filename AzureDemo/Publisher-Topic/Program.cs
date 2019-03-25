using Microsoft.Azure;
using Microsoft.Azure.ServiceBus;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ServiceBus.Messaging;

namespace Publisher.Topic
{
    class Program
    {
        private static string topicName = "test";
        private string serviceBusConnectionstring = "PrimaryConnectionstring"; //can but it on confing and read
        private static string message = "test message";
        static void Main(string[] args)
        {
            Console.WriteLine("write your message");
            var message = Console.ReadLine();

            var serviceBusConnectionString = ConfigurationManager.AppSettings["serviceBusConnectionstring"]; //or 
            //you can directly read from cloud 
            //Microsoft.WindowsAzure.ConfigurationManager
            //Microsoft Azure Configuration Manager provides a unified API to load configuration settings regardless of where the application is hosted - whether on-premises or in a Cloud Service.
            var serviceBusConnectionString1 = CloudConfigurationManager.GetSetting("serviceBusConnectionstring");

            //write message to topic
            var topicClient = new Microsoft.Azure.ServiceBus.TopicClient(serviceBusConnectionString, topicName);
            var body = Encoding.UTF8.GetBytes(message);
            var busMessage = new Message(body);
            topicClient.SendAsync(busMessage).GetAwaiter().GetResult();


            //for queues
            // Microsoft.ServiceBus.Messsaging
            var queueName = "testQueue";
            var queueClient = Microsoft.ServiceBus.Messaging.QueueClient.CreateFromConnectionString(serviceBusConnectionString, queueName);
            var input = Console.ReadLine();
            var message1 = new BrokeredMessage(input);
            queueClient.Send(message1);

        }
    }
}
