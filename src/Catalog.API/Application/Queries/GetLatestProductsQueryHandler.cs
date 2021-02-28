using API.Catalog.Application.DataAccess;
using API.Catalog.Application.Dto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace API.Catalog.Application.Queries
{
    public class GetLatestProductsQueryHandler : IRequestHandler<GetLatestProductsQuery, IEnumerable<ProductDto>>
    {
        private readonly IProductQueries _productQueries;

        public GetLatestProductsQueryHandler(IProductQueries productQueries)
        {
            _productQueries = productQueries;
        }

        public async Task<IEnumerable<ProductDto>> Handle(GetLatestProductsQuery request, CancellationToken cancellationToken)
        {
            var products = await _productQueries.GetLatestProductAsync(request.Limit);

            if (products == null)
            {
                products = new List<ProductDto>();
            }

            return products;
        }
    }
}
