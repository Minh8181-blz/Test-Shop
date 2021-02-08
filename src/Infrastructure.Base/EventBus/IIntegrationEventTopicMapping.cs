using Application.Base.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Base.EventBus
{
    public interface IIntegrationEventTopicMapping
    {
        RabbitMqTopicModel GetPublishedTopic(Type type);
        RabbitMqQueueModel GetSubscribedQueue(Type type);
    }
}
