using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using RabbitMQ.Client;
using Demo3_Work;

namespace RabbitMQ_Demo3
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

        private static void Main(string[] args)
        {
            // Build a new channel to Rabbit.
            IModel channel = BuildChannel(RabbitConfiguration.Server, RabbitConfiguration.Username, RabbitConfiguration.Password);

            for (int i = 0; i < 100000; i++)
            {
                CalculateBase work = DoWork();

                StringWriter sw = new StringWriter();

                XmlSerializer serializer = new XmlSerializer(typeof(CalculateBase));
                serializer.Serialize(sw, work);

                // Convert input to byte array
                byte[] body = Encoding.UTF8.GetBytes(sw.ToString());

                // Write the byte array to the Rabbit queue.
                channel.BasicPublish("", RabbitConfiguration.QueueName, null, body);

                // Notify user that message was sent
                string notify = String.Format(" [x] Sent {0}", i);

                Console.WriteLine(notify);

            }
        }

        /*
         * Supporting Calculate methods
         */
        static readonly Random Rnd = new Random();

        private static CalculateBase DoWork()
        {
            float x = NextFloat();
            float y = NextFloat();

            CalculateBase calculate;

            int rand = Rnd.Next(1, 3);

            if (rand % 2 == 0)
            {
                calculate = new Add(x, y);
            }
            else
            {
                calculate = new Subtract(x, y);
            }

            return calculate;
        }

        static float NextFloat()
        {
            double mantissa = (Rnd.NextDouble() * 2.0) - 1.0;
            double exponent = Math.Pow(2.0, Rnd.Next(-126, 128));
            return (float)(mantissa * exponent);
        }
    }
}