using Domain.Base.SeedWork;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Base
{
    public class DomainEventPublisher : IDomainEventPublisher
    {
        private readonly IMediator _mediator;

        public DomainEventPublisher(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Publish(DomainEvent @event)
        {
            var eventType = @event.GetType();

            Type notificationType = typeof(DomainEventNotification<>).MakeGenericType(eventType);

            var notification = Activator.CreateInstance(notificationType, @event);

            await _mediator.Publish(notification);
        }

        public async Task Publish(IEnumerable<DomainEvent> events)
        {
            if (events != null)
            {
                foreach (var @event in events)
                {
                    await Publish(@event);
                }
            }
        }
    }
}
