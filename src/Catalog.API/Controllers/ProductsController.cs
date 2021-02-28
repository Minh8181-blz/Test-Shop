using API.Catalog.Application.Dto;
using API.Catalog.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Catalog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(IMediator mediator, ILogger<ProductsController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IEnumerable<ProductDto>> GetLatestProducts()
        {
            int limit = 10;

            var query = new GetLatestProductsQuery(limit);

            var products = await _mediator.Send(query);

            return products;
        }

        [HttpGet("{id}")]
        public object GetById(int id)
        {
            return new { ProductId = id };
        }
    }
}
