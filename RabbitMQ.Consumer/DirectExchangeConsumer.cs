using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace RabbitMQ.Consumer
{
    public static class DirectExchangeConsumer
    {
        public static void Consume(IModel channel)
        {
            channel.ExchangeDeclare("demo-direct-exchange", ExchangeType.Direct);

            channel.QueueDeclare(
                "demo-direct-queue",
                durable: true,
                exclusive: false,
                autoDelete: true,
                arguments: null);

            channel.QueueBind(
                "demo-direct-queue",        // queue name
                "demo-direct-exchange",     // exchange name
                "account.init");            // routing key

            channel.BasicQos(0, 10, false); // fetches 10 messages at a time

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += (sender, args) =>
            {
                var body = args.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine(message);
            };

            channel.BasicConsume("demo-direct-queue", true, consumer);

            Console.WriteLine("Consumer started for direct-queue");
            Console.ReadLine();
        }
    }
}