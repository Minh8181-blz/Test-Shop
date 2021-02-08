using Application.Base.SeedWork;
using Domain.Base.SeedWork;
using Infrastructure.Base;
using Infrastructure.Base.MessageQueue;
using MediatR;
using Microsoft.Extensions.Configuration;
using Ordering.API.Application.IntegrationEvents;
using Ordering.API.Domain.Events;
using Ordering.API.Domain.Interfaces;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.API.Application.DomainEventHandlers
{
    public class OrderCreatedDomainEventHandler : INotificationHandler<DomainEventNotification<OrderCreatedDomainEvent>>
    {
        private readonly IIntegrationEventBus _eventBus;

        public OrderCreatedDomainEventHandler(IIntegrationEventBus eventBus)
        {
            _eventBus = eventBus;
        }

        public Task Handle(DomainEventNotification<OrderCreatedDomainEvent> notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
