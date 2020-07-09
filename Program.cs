using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;

namespace TaskSubscriber
{
    class Program
    {
        static void Main(string[] args)
        {
            const string exchangeName = "EXCHANGE2";
            ConnectionFactory factory = new ConnectionFactory();
            factory.AutomaticRecoveryEnabled = true;
            factory.NetworkRecoveryInterval = TimeSpan.FromSeconds(5);
            factory.RequestedHeartbeat = TimeSpan.FromSeconds(5);
            factory.TopologyRecoveryEnabled = true;

            using (IConnection connection = factory.CreateConnection())
            {
                using (IModel channel = connection.CreateModel())
                {
                    factory.HostName = "localhost";
                    channel.ExchangeDeclare(
                        exchangeName,
                        ExchangeType.Topic,
                        true,
                        false,
                        null);

                    string queueName = channel.QueueDeclare();

                    EventingBasicConsumer consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (o, e) =>
                    {
                        string message = Encoding.ASCII.GetString(e.Body.ToArray());
                        try
                        {
                            Packet packet = JsonConvert.DeserializeObject<Packet>(message);
                            packet.receiveDate = DateTime.Now;
                            channel.BasicAck(e.DeliveryTag, false);
                            Console.WriteLine($"id:{packet.publishId} message:{packet.message} date:{packet.sendDate} hash:{packet.hash} receiveDate:{packet.receiveDate}");

                            using (TaskDbContext db = new TaskDbContext())
                            {
                                db.packets.Add(packet);
                                db.SaveChanges();
                            }
                        }
                        catch (DbUpdateException ex) { Console.WriteLine("Ошибка сохранения пакета данных!"); }
                        catch (Exception ex) { Console.WriteLine("Ошибка разбора пакета данных!"); }
                    };

                    string consumerTag = channel.BasicConsume(
                        queueName,
                        false,
                        consumer);

                    channel.QueueBind(
                        queueName,
                        exchangeName,
                        "testTopic");

                    Console.WriteLine("Listening...... press ENTER to exit");
                    Console.ReadLine();

                    channel.QueueUnbind(
                        queueName,
                        exchangeName,
                        "testTopic",
                        null);

                }
            }
        }
    }
}
