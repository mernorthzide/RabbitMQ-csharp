using NLipsum.Core;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQSend
{
    internal class Program
    {
        private const string QueueName = "ChatMessage";
        private const string ExchangeName = "SendMessage";
        private const string RoutingKey = "";
        private static ConnectionFactory _factory;
        private static IConnection _connection;
        private static IModel _chanel;

        private static void Main(string[] args)
        {
            _factory = new ConnectionFactory
            {
                HostName = "localhost",
                UserName = "ChatAdmin",
                Password = "1234",
                VirtualHost = "Chat"
            };
            _connection = _factory.CreateConnection();
            _chanel = _connection.CreateModel();
            _chanel.QueueDeclare(QueueName, true, false, false, null);
            _chanel.QueueBind(QueueName, ExchangeName, RoutingKey, null);

            for (var i = 1; i <= 10000; i++)
            {
                var lipsum = LipsumGenerator.Generate(1);

                Console.WriteLine("No. " + i);

                // Add to queue
                var chatEncode = Encoding.ASCII.GetBytes(i + lipsum);
                _chanel.BasicPublish(ExchangeName, RoutingKey, null, chatEncode);
            }
        }
    }
}