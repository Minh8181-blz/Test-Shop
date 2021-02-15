using System;
using System.Threading.Tasks;

namespace Infrastructure.Base.MessageQueue
{
    public interface IQueueProcessor
    {
        bool PublishMessageToExchange(string exchange, string routingKey, object message);
        bool SubscribeQueueToExchange<T>(string exchange, string routingKey, string queue, Func<T, bool> onMessageReceived);
        bool PublishPlainMessageToExchange(string exchange, string routingKey, string message);
    }
}
