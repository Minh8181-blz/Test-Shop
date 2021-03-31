using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Base.SeedWork;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ordering.API.Application.Commands;
using Ordering.API.Application.Models;
using Ordering.API.Domain.Interfaces;
using Ordering.API.ViewModels;

namespace Ordering.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrdersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult> CreateOrder(OrderViewModel model, [FromHeader(Name = "x-requestid")] string requestId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.Values.SelectMany(x => x.Errors));

            if (!Guid.TryParse(requestId, out Guid guid) || guid == Guid.Empty)
            {
                return BadRequest();
            }

            var orderItems = model.OrderItems.Select(x => new OrderItemDto
            {
                ProductId = x.ProductId,
                Quantity = x.Quantity
            });

            var command = new CreateOrderCommand(model.CustomerId, orderItems, DateTime.Now, model.Description);

            var identifiedCommand = new IdentifiedCommand<CreateOrderCommand, bool>(command, guid);

            var result = await _mediator.Send(identifiedCommand);

            if (!result)
                return BadRequest(result);

            return Ok(result);
        }
    }
}
