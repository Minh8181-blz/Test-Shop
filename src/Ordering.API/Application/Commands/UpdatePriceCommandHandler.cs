using Application.Base.SeedWork;
using MediatR;
using Ordering.API.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.API.Application.Commands
{
    public class UpdatePriceCommandHandler : IRequestHandler<UpdatePriceCommand, bool>
    {
        private readonly IOrderRepository _orderRepository;

        public UpdatePriceCommandHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<bool> Handle(UpdatePriceCommand request, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetAsync(request.OrderId);

            if (order == null)
                return false;

            order.UpdatePrice(request.Total);

            _orderRepository.Update(order);

            await _orderRepository.UnitOfWork.SaveEntitiesAsync();

            return true;
        }
    }

    public class UpdatePriceIdentifiedCommandHandler : IdentifiedCommandHandler<UpdatePriceCommand, bool>
    {
        public UpdatePriceIdentifiedCommandHandler(
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
