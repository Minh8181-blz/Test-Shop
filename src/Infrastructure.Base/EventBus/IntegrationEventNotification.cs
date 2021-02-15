using Application.Base.SeedWork;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Base.EventBus
{
    public class IntegrationEventNotification<T> : IRequest<bool> where T : IntegrationEvent
    {
        public IntegrationEventNotification(T data)
        {
            Data = data;
        }

        public T Data { get; }
    }
}
