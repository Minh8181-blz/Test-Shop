using Application.Base.SeedWork;
using Domain.Base.SeedWork;
using Infrastructure.Base.EventLog;
using MediatR;
using Ordering.API.Application.IntegrationEvents;
using Ordering.API.Domain.Entities;
using Ordering.API.Domain.Interfaces;
using Ordering.API.Domain.Services;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.API.Application.Commands
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, bool>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IIntegrationEventLogService _integrationEventLogService;

        public CreateOrderCommandHandler(
            IOrderRepository orderRepository,
            IIntegrationEventLogService integrationEventLogService)
        {
            _orderRepository = orderRepository;
            _integrationEventLogService = integrationEventLogService;
        }

        public async Task<bool> Handle(CreateOrderCommand notification, CancellationToken cancellationToken)
        {
            var orderItems = notification.OrderItems.Select(x => OrderItem.CreateOrderItem(x.ProductId, x.Quantity));

            var order = Order.CreateOrder(notification.CustomerId, notification.Description, orderItems);

            _orderRepository.Add(order);

            var @event = new OrderCreatedIntegrationEvent(order);

            _integrationEventLogService.SaveEvent(@event);

            await _orderRepository.UnitOfWork.SaveEntitiesAsync();

            order = await _orderRepository.GetAsync(order.Id);

            await _integrationEventLogService.PublishAsync(@event);

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
