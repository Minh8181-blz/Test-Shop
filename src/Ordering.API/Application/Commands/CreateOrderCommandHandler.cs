using Application.Base.SeedWork;
using Domain.Base.SeedWork;
using Infrastructure.Base.EventLog;
using MediatR;
using Ordering.API.Application.IntegrationEvents;
using Ordering.API.Domain.Entities;
using Ordering.API.Domain.Interfaces;
using Ordering.API.Domain.Services;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.API.Application.Commands
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, bool>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IIntegrationEventService _integrationEventService;
        private readonly IUnitOfWork _unitOfWork;

        public CreateOrderCommandHandler(
            IOrderRepository orderRepository,
            IIntegrationEventService integrationEventService,
            IUnitOfWork unitOfWork)
        {
            _orderRepository = orderRepository;
            _integrationEventService = integrationEventService;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(CreateOrderCommand notification, CancellationToken cancellationToken)
        {
            await _unitOfWork.BeginTransactionAsync();

            var orderItems = notification.OrderItems.Select(x => OrderItem.CreateOrderItem(x.ProductId, x.Quantity));

            var order = Order.CreateOrder(notification.CustomerId, notification.Description, orderItems);

            _orderRepository.Add(order);

            await _unitOfWork.SaveEntitiesAsync();

            var @event = new OrderCreatedIntegrationEvent(order, true);

            await _integrationEventService.SaveEventAsync(@event);

            await _unitOfWork.SaveChangesAsync();

            await _unitOfWork.CommitAsync();

            await _integrationEventService.PublishAsync(@event);

            return true;
        }
    }

    public class CreateOrderIdentifiedCommandHandler : IdentifiedCommandHandler<CreateOrderCommand, bool>
    {
        public CreateOrderIdentifiedCommandHandler(
            IMediator mediator,
            IRequestManager requestManager)
            : base(mediator, requestManager)
        {
        }

        protected override bool CreateResultForDuplicateRequest()
        {
            return true;
        }
    }
}
