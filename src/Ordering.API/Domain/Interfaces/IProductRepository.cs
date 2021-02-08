using Domain.Base.SeedWork;
using Ordering.API.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ordering.API.Domain.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
        public Task<IEnumerable<Product>> GetByIdsAsync(IEnumerable<int> ids);
    }
}
