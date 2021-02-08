using Application.Base.SeedWork;
using Infrastructure.Base.MessageQueue;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Base.EventBus
{
    public class IntegrationEventBus : IIntegrationEventBus
    {
        private readonly IQueueProcessor _queueProcessor;
        private readonly IServiceProvider _serviceProvider;
        private readonly IIntegrationEventTopicMapping _integrationEventTopicMapping;

        public IntegrationEventBus(
            IQueueProcessor queueProcessor,
            IServiceProvider serviceProvider,
            IIntegrationEventTopicMapping integrationEventTopicMapping)
        {
            _serviceProvider = serviceProvider;
            _queueProcessor = queueProcessor;
            _integrationEventTopicMapping = integrationEventTopicMapping;
        }

        public bool PublishEvent(IntegrationEvent @event)
        {
            RabbitMqTopicModel topicModel = _integrationEventTopicMapping.GetPublishedTopic(@event.GetType());

            if (topicModel == null)
                return false;

            _queueProcessor.PublishMessageToExchange(topicModel.Exchange, topicModel.RoutingKey, @event);
            return true;
        }

        public bool SubscribeEvent<T>() where T : IntegrationEvent
        {
            RabbitMqQueueModel queueModel = _integrationEventTopicMapping.GetSubscribedQueue(typeof(T));

            if (queueModel == null)
                return false;

            _queueProcessor.SubscribeQueueToExchange<T>(queueModel.Exchange, queueModel.RoutingKey, queueModel.Queue, Consume);
            return true;
        }

        private void Consume<T>(T @event)
        {
            var eventType = @event.GetType();

            Type notificationType = typeof(IntegrationEventNotification<>).MakeGenericType(eventType);

            var notification = Activator.CreateInstance(notificationType, @event);

            using var serviceScope = _serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
            var mediator = serviceScope.ServiceProvider.GetRequiredService<IMediator>();

            mediator.Publish(notification).Wait();
        }
    }
}
