namespace WebApiSubscriber.RabbitMQ
{
    public interface IMessageConsumer
    {
        Task ReceivedMessage();
    }
}
