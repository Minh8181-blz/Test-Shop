using RabbitMQ.Client;

namespace Infrastructure.Base.MessageQueue
{
    public interface IQueueConnectionFactory
    {
        IConnection GetConnection();
    }
}
