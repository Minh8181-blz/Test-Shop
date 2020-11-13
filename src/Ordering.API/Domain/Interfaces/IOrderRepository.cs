using Domain.Base.SeedWork;
using Ordering.API.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ordering.API.Domain.Interfaces
{
    public interface IOrderRepository : IRepository<Order>
    {
    }
}
