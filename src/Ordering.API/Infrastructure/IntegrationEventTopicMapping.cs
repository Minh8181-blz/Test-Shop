using Application.Base.SeedWork;
using Ordering.API.Application.IntegrationEvents;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Base.EventBus
{
    public class IntegrationEventTopicMapping : IIntegrationEventTopicMapping
    {
        public const string OrderExchange = "test_micro.order";

        private readonly Dictionary<string, RabbitMqTopicModel> PubMaps = new Dictionary<string, RabbitMqTopicModel>
        {
            { nameof(OrderCreatedIntegrationEvent), new RabbitMqTopicModel(OrderExchange, "order.int_event.created") },
        };

        private readonly Dictionary<string, RabbitMqQueueModel> SubMaps = new Dictionary<string, RabbitMqQueueModel>
        {
            { nameof(PriceCalculatedIntegrationEvent),
                new RabbitMqQueueModel(OrderExchange, "order.int_event.price_calculated", "order_int_event_price_calculated_orderms") }
        };

        public RabbitMqTopicModel GetPublishedTopic(Type type)
        {
            var key = type.Name;
            if (type.IsAssignableFrom(typeof(IntegrationEvent))) {
                throw new Exception("Invalid type parameter");
            }

            if (PubMaps.ContainsKey(key))
            {
                return PubMaps[key];
            }

            return null;
        }

        public RabbitMqQueueModel GetSubscribedQueue(Type type)
        {
            var key = type.Name;
            if (type.IsAssignableFrom(typeof(IntegrationEvent)))
            {
                throw new Exception("Invalid type parameter");
            }

            if (SubMaps.ContainsKey(key))
            {
                return SubMaps[key];
            }

            return null;
        }
    }
}
