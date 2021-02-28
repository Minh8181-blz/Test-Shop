using API.Catalog.Application.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Catalog.Application.DataAccess
{
    public interface IProductQueries
    {
        Task<IEnumerable<ProductDto>> GetLatestProductAsync(int limit);
    }
}
