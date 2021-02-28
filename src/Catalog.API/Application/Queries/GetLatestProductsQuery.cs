using API.Catalog.Application.Dto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Catalog.Application.Queries
{
    public class GetLatestProductsQuery : IRequest<IEnumerable<ProductDto>>
    {
        public GetLatestProductsQuery(int limit)
        {
            Limit = limit;
        }

        public int Limit { get; set; }
    }
}
