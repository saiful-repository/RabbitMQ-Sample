namespace Publisher.RabbitMQ
{
    public interface IMessageProducer
    {
        Task SendMessage<T> (T message);
    }
}
