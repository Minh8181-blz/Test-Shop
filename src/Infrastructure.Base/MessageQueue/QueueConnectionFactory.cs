using RabbitMQ.Client;

namespace Infrastructure.Base.MessageQueue
{
    public class QueueConnectionFactory : IQueueConnectionFactory
    {
        private readonly ConnectionFactory _factory;

        public QueueConnectionFactory(string connectionString)
        {
            _factory = new ConnectionFactory
            {
                HostName = connectionString
            };
        }

        public IConnection GetConnection()
        {
            return _factory.CreateConnection();
        }
    }
}
