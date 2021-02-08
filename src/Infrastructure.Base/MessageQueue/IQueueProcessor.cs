using System;

namespace Infrastructure.Base.MessageQueue
{
    public interface IQueueProcessor
    {
        bool PublishMessageToExchange(string exchange, string routingKey, object message);
        bool SubscribeQueueToExchange<T>(string exchange, string routingKey, string queue, Action<T> onMessageReceived);
    }
}
