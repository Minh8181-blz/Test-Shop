using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Base.SeedWork
{
    public class IdentifiedCommandHandler<T, R> : IRequestHandler<IdentifiedCommand<T, R>, R>
        where T : IRequest<R>
    {
        private readonly IMediator _mediator;
        private readonly IRequestManager _requestManager;

        public IdentifiedCommandHandler(
            IMediator mediator,
            IRequestManager requestManager)
        {
            _mediator = mediator;
            _requestManager = requestManager;
        }

        protected virtual R CreateResultForDuplicateRequest()
        {
            return default;
        }

        public async Task<R> Handle(IdentifiedCommand<T, R> message, CancellationToken cancellationToken)
        {
            var alreadyExists = await _requestManager.ExistAsync(message.Id);
            if (alreadyExists)
            {
                return CreateResultForDuplicateRequest();
            }
            else
            {
                await _requestManager.CreateRequestForCommandAsync<T>(message.Id);
                try
                {
                    var command = message.Command;

                    var result = await _mediator.Send(command, cancellationToken);

                    return result;
                }
                catch
                {
                    return default;
                }
            }
        }
    }
}
