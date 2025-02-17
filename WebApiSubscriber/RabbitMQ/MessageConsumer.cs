
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Serilog;
using System.Text;

namespace WebApiSubscriber.RabbitMQ
{
    public class MessageConsumer : IMessageConsumer
    {
        private readonly IRabbitMQConnection _connection;

        public MessageConsumer(IRabbitMQConnection connection)
        {
            _connection = connection;
        }
        public async Task ReceivedMessage()
        {
            try
            {
                Log.Information("Message Received Start");

                // Create a new channel for communication with RabbitMQ
                var channel = await _connection.Connection.CreateChannelAsync();

                // Declare an exchange of type "fanout"
                await channel.ExchangeDeclareAsync(exchange: "multi_log_msg", type: ExchangeType.Fanout, durable: true, autoDelete: false);

                // Declare a queue named "queue_console"
                await channel.QueueDeclareAsync(queue: "queue_webapi", durable: true, exclusive: false, autoDelete: false);

                //Bind the Queue with Exchange
                await channel.QueueBindAsync(queue: "queue_webapi", exchange: "multi_log_msg", routingKey: "");

                // Create a new consumer to receive messages from the queue
                var consumer = new AsyncEventingBasicConsumer(channel);

                // Define the event handler for processing received messages
                consumer.ReceivedAsync += async (sender, args) =>
                {
                    try
                    {
                        var body = args.Body.ToArray();
                        var message = Encoding.UTF8.GetString(body);
                        Log.Information($"Message Received: {message}");
                    }
                    catch (Exception ex)
                    {
                        Log.Error($"Error processing message: {ex.Message}");
                    }

                    await Task.CompletedTask;
                };

                // Start consuming messages from the "info" queue with automatic acknowledgment
                await channel.BasicConsumeAsync(queue: "queue_webapi", autoAck: true, consumer: consumer);

                Log.Information("Message Received End");

                // Keep the consumer running
                await Task.Delay(Timeout.Infinite);
            }
            catch(Exception ex)
            {
                Log.Information("Message Received Error: " + ex.Message.ToString());
            }
        }
    }
}
