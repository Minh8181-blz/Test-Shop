using Application.Base.SeedWork;
using Infrastructure.Base;
using Infrastructure.Base.EventBus;
using MediatR;
using Ordering.API.Application.Commands;
using Ordering.API.Application.IntegrationEvents;
using Ordering.API.Domain.Events;
using Ordering.API.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.API.Application.IntegrationEventHandlers
{
    public class PriceCalculatedIntegrationEventHandler : IRequestHandler<IntegrationEventNotification<PriceCalculatedIntegrationEvent>, bool>
    {
        private readonly IMediator _mediator;

        public PriceCalculatedIntegrationEventHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<bool> Handle(IntegrationEventNotification<PriceCalculatedIntegrationEvent> notification, CancellationToken cancellationToken)
        {
            var command = new UpdatePriceCommand(notification.Data.OrderId, notification.Data.Total);

            var identifiedCommand = new IdentifiedCommand<UpdatePriceCommand, bool>(command, notification.Data.Id);

            return await _mediator.Send(identifiedCommand, cancellationToken);
        }
    }
}
