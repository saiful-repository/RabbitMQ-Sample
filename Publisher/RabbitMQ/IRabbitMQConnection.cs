using RabbitMQ.Client;

namespace Publisher.RabbitMQ
{
    public interface IRabbitMQConnection
    {
        IConnection Connection { get; }
    }
}
