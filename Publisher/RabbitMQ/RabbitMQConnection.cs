using RabbitMQ.Client;

namespace Publisher.RabbitMQ
{
    public class RabbitMQConnection : IRabbitMQConnection, IDisposable
    {
        private  IConnection? _connection;
        public IConnection Connection
        {
            get
            {
                if (_connection == null)
                    throw new InvalidOperationException("Connection has not been initialized.");
                return _connection;
            }
        }

        private RabbitMQConnection() { }
        public static async Task<RabbitMQConnection> CreateAsync()
        {
            var instance = new RabbitMQConnection();
            await instance.InitializeConnectionAsync();
            return instance;
        }

        private async Task InitializeConnectionAsync()
        {
            var factory = new ConnectionFactory
            {
                HostName = "localhost"
            };

            _connection = await factory.CreateConnectionAsync();
        }

        public void Dispose()
        {
            _connection?.Dispose();
        }
    }
}
