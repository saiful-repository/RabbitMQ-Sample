using RabbitMQ.Client;

namespace WebApiSubscriber.RabbitMQ
{
    public interface IRabbitMQConnection
    {
        IConnection Connection { get; }
    }
}
