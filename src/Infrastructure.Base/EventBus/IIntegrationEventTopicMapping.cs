using Application.Base.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Base.EventBus
{
    public interface IIntegrationEventTopicMapping
    {
        RabbitMqTopicModel GetPublishedTopic(string eventTypeName);
        RabbitMqQueueModel GetSubscribedQueue(string eventTypeName);
    }
}
