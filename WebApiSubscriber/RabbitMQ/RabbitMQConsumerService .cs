
using Serilog;

namespace WebApiSubscriber.RabbitMQ
{
    public class RabbitMQConsumerService : BackgroundService
    {
        private readonly IMessageConsumer _consumer;
        public RabbitMQConsumerService(IMessageConsumer consumer)
        {
            _consumer = consumer;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Log.Information("Background Service Run");

            await _consumer.ReceivedMessage();
        }
    }
}
