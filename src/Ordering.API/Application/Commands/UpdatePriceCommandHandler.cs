using Application.Base.SeedWork;
using Domain.Base.SeedWork;
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
        private readonly IUnitOfWork _unitOfWork;

        public UpdatePriceCommandHandler(IOrderRepository orderRepository, IUnitOfWork unitOfWork)
        {
            _orderRepository = orderRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(UpdatePriceCommand request, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetAsync(request.OrderId);

            if (order == null)
                return false;

            order.UpdatePrice(request.Total);

            _orderRepository.Update(order);

            await _unitOfWork.SaveEntitiesAsync();

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
