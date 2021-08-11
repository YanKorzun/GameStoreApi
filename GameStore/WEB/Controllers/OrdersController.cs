using System.Collections.Generic;
using System.Threading.Tasks;
using GameStore.BL.Interfaces;
using GameStore.DAL;
using GameStore.WEB.DTO.OrderModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.WEB.Controllers
{
    [ApiController]
    [Route("api/orders")]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IOrderService _orderService;

        public OrdersController(ApplicationDbContext context, IOrderService orderService)
        {
            _context = context;
            _orderService = orderService;
        }

        /// <summary>
        ///     Get information about order using provided id
        /// </summary>
        /// <remark>By default, id is application user id</remark>
        /// <param name="id">Order id</param>
        /// <returns>Orders list</returns>
        /// <response code="200">Information taken successfully</response>
        /// <response code="401">User is not authenticated</response>
        /// <response code="404">Order doesn't exist</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetOrder(int? id = null)
        {
            var orders = await _orderService.GetOrdersAsync(User, id);
            return Ok(orders);
        }

        /// <summary>
        ///     Create new order with provided properties model
        /// </summary>
        /// <remark>By default, id is application user id</remark>
        /// <param name="orderModel">Order properties model</param>
        /// <returns>Extended order properties model</returns>
        /// <response code="201">Order created successfully</response>
        /// <response code="401">User is not authenticated</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ExtendedOrderModel>> PostOrder(OrderModel orderModel)
        {
            var createdOrder = await _orderService.CreateOrderAsync(orderModel, User);
            return CreatedAtAction(nameof(PostOrder), createdOrder);
        }

        /// <summary>
        ///     Delete orders range
        /// </summary>
        /// <remark>By default, id is application user id</remark>
        /// <param name="orders">Orders collection</param>
        /// <returns>No content</returns>
        /// <response code="204">Information deleted successfully</response>
        /// <response code="401">User is not authenticated</response>
        /// <response code="404">Order doesn't exist</response>
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteOrder([FromBody] ICollection<ExtendedOrderModel> orders)
        {
            await _orderService.DeleteOrders(orders);
            return NoContent();
        }

        /// <summary>
        ///     Complete all user's orders
        /// </summary>
        /// <returns>No content</returns>
        /// <response code="204">Orders completed successfully</response>
        /// <response code="401">User is not authenticated</response>
        [HttpPost("buy")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ExtendedOrderModel>> BuyOrder()
        {
            await _orderService.CompleteOrders(User);
            return NoContent();
        }
    }
}