using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace RabbitMQReceive
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

            var consumer = new EventingBasicConsumer(_chanel);

            consumer.Received += (model, ea) =>
            {
                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body);
                _chanel.BasicAck(ea.DeliveryTag, false);

                Console.WriteLine(" [x] {0}", message);
            };

            _chanel.BasicConsume(QueueName, false, consumer);
        }
    }
}