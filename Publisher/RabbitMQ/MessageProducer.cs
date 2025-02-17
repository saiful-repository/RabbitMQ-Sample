
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace Publisher.RabbitMQ
{
    public class MessageProducer : IMessageProducer
    {
        private readonly IRabbitMQConnection _connection;

        public MessageProducer(IRabbitMQConnection connection)
        {
            _connection = connection;
        }

        public async Task SendMessage<T>(T message)
        {
            // Create a new channel for communication with RabbitMQ.
            using var channel = await _connection.Connection.CreateChannelAsync();

            // Declare an exchange of type "fanout"
            await channel.ExchangeDeclareAsync(exchange: "multi_log_msg", type: ExchangeType.Fanout, durable: true, autoDelete: false);

            // Serialize the message object to a JSON string.
            var json = JsonSerializer.Serialize(message);

            // Convert the JSON string into a byte array.
            var body = Encoding.UTF8.GetBytes(json);

            // Publish the message to the "multi_log_msg" exchange.
            await channel.BasicPublishAsync(exchange: "multi_log_msg", routingKey: "", body: body);

        }
    }
}
