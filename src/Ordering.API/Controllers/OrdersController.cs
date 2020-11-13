using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ordering.API.Domain.Interfaces;
using Ordering.API.ViewModels;

namespace Ordering.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;

        public OrdersController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult> GetOrders(int id)
        {
            var orders = await _orderRepository.GetAsync(id);
            return Ok(orders);
        }

        [HttpPost]
        public async Task<ActionResult> CreateOrder(OrderViewModel model)
        {
            await Task.FromResult(true);
            return Ok("HAHA");
        }
    }
}
