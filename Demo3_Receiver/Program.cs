using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using Demo3_Work;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Demo3_Receiver
{
    class Program
    {
        private readonly static dynamic RabbitConfiguration = new
        {
            Server = "192.168.100.86",
            Username = "jfasenmyer",
            Password = "password",
            QueueName = "calculate"
        };

        public static IModel BuildChannel(string hostName, string username, string password, int port = 5672)
        {
            ConnectionFactory factory = new ConnectionFactory { HostName = hostName, Port = port, UserName = username, Password = password };

            IConnection connection = factory.CreateConnection();

            return connection.CreateModel();
        }

        static void Main(string[] args)
        {
            // Build a new channel to Rabbit.
            IModel channel = BuildChannel(RabbitConfiguration.Server, RabbitConfiguration.Username, RabbitConfiguration.Password);

            channel.QueueDeclare(RabbitConfiguration.QueueName, false, false, false, null);

            const bool noAck = true;

//            const bool noAck = false;
//            channel.BasicQos(0, 1, false);

            var consumer = new QueueingBasicConsumer(channel);
            channel.BasicConsume(RabbitConfiguration.QueueName, noAck, consumer);

            Console.WriteLine(" [*] Waiting for messages...");

            while (true)
            {
                var ea = (BasicDeliverEventArgs)consumer.Queue.Dequeue();

                var body = ea.Body;

                var message = Encoding.UTF8.GetString(body);

                XmlSerializer serializer = new XmlSerializer(typeof(CalculateBase));

                TextReader reader = new StringReader(message);

                object test = serializer.Deserialize(reader);

                CalculateBase calculate = (CalculateBase)test;

                float result = calculate.Calculate();

                Console.WriteLine(" [x] Received {0}", result);

//                Thread.Sleep(5000);

//                channel.BasicAck(ea.DeliveryTag, false);
            }
        }
    }
}