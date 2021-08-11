using System.Collections.Generic;
using System.Threading.Tasks;
using GameStore.BL.Interfaces;
using GameStore.DAL;
using GameStore.WEB.DTO.OrderModels;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.WEB.Controllers
{
    [Route("api/orders")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IOrderService _orderService;

        public OrdersController(ApplicationDbContext context, IOrderService orderService)
        {
            _context = context;
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<IActionResult> GetOrder(int? id = null)
        {
            var orders = await _orderService.GetOrdersAsync(User, id);
            return Ok(orders);
        }

        [HttpPost]
        public async Task<ActionResult<ExtendedOrderModel>> PostOrder(OrderModel orderModel)
        {
            var createdOrder = await _orderService.CreateOrderAsync(orderModel, User);
            return CreatedAtAction(nameof(PostOrder), createdOrder);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteOrder([FromBody] ICollection<ExtendedOrderModel> orders)
        {
            await _orderService.DeleteOrders(orders);
            return NoContent();
        }

        [HttpPost("buy")]
        public async Task<ActionResult<ExtendedOrderModel>> BuyOrder()
        {
            await _orderService.CompleteOrders(User);
            return NoContent();
        }
    }
}