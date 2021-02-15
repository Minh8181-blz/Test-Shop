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

        public const string PriceServiceOrderingSubQueue = "pricems_orderms_ordering";
        public const string CatalogServiceOrderingSubQueue = "catalogms_orderms_ordering";

        public const string OrderCreatedPubRoutingKey = "int_event.order_created";

        public const string PriceCalculatedSubRoutingKey = "int_event.price_calculated";

        private readonly Dictionary<string, RabbitMqTopicModel> PubMaps = new Dictionary<string, RabbitMqTopicModel>
        {
            { nameof(OrderCreatedIntegrationEvent), new RabbitMqTopicModel(OrderExchange, OrderCreatedPubRoutingKey) },
        };

        private readonly Dictionary<string, RabbitMqQueueModel> SubMaps = new Dictionary<string, RabbitMqQueueModel>
        {
            { nameof(PriceCalculatedIntegrationEvent),
                new RabbitMqQueueModel(OrderExchange, PriceCalculatedSubRoutingKey, PriceServiceOrderingSubQueue) }
        };

        public RabbitMqTopicModel GetPublishedTopic(string eventTypeName)
        {
            if (PubMaps.ContainsKey(eventTypeName))
            {
                return PubMaps[eventTypeName];
            }

            return null;
        }

        public RabbitMqQueueModel GetSubscribedQueue(string eventTypeName)
        {
            if (SubMaps.ContainsKey(eventTypeName))
            {
                return SubMaps[eventTypeName];
            }

            return null;
        }
    }
}
