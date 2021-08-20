using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameStore.BL.Enums;
using GameStore.BL.Interfaces;
using GameStore.BL.Utilities;
using GameStore.WEB.DTO.Orders;
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
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        /// <summary>
        ///     Get information about order using provided id
        /// </summary>
        /// <remark>By default, id is application user id</remark>
        /// <param name="id">Order id</param>
        /// <response code="200">Information taken successfully</response>
        /// <response code="401">User is not authenticated</response>
        /// <response code="404">Order doesn't exist</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetOrderList([FromQuery] int[] id = null)
        {
            var userIdResult = ClaimsUtility.GetUserIdFromClaims(User);
            if (userIdResult.Result is not ServiceResultType.Success)
            {
                return StatusCode((int)userIdResult.Result, userIdResult.ErrorMessage);
            }

            var orders = await _orderService.GetOrdersAsync(userIdResult.Data, id);
            if (!orders.Any())
            {
                return NotFound();
            }

            return Ok(orders);
        }

        /// <summary>
        ///     Create new order with provided properties model
        /// </summary>
        /// <remark>By default, id is application user id</remark>
        /// <param name="orderDto">Order properties model</param>
        /// <returns>Extended order properties model</returns>
        /// <response code="201">Order created successfully</response>
        /// <response code="401">User is not authenticated</response>
        /// <response code="500">Order already exist</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<OutputOrderDto>> CreateOrder(BasicOrderDto orderDto)
        {
            var createResult = await _orderService.CreateOrderAsync(orderDto);

            return
                createResult.Result is not ServiceResultType.Success
                    ? StatusCode((int)createResult.Result, createResult.ErrorMessage)
                    : CreatedAtAction(nameof(CreateOrder), createResult.Data);
        }

        /// <summary>
        ///     Updates orders range
        /// </summary>
        /// <returns>Updated collection</returns>
        /// <response code="200">Orders updated successfully</response>
        /// <response code="401">User is not authenticated</response>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<OutputOrderDto>> UpdateOrder([FromBody] ExtendedOrderDto order)
        {
            var result = await _orderService.UpdateItemAsync(order);

            return Ok(result);
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
        public async Task<IActionResult> DeleteOrder([FromBody] ICollection<ExtendedOrderDto> orders)
        {
            await _orderService.DeleteOrdersAsync(orders);

            return NoContent();
        }

        /// <summary>
        ///     Complete all user's orders
        /// </summary>
        /// <returns>No content</returns>
        /// <response code="204">Orders completed successfully</response>
        /// <response code="401">User is not authenticated</response>
        /// <response code="500">Unable to perform payment</response>
        [HttpPost("buy")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> OrderPayment()
        {
            var userIdResult = ClaimsUtility.GetUserIdFromClaims(User);
            if (userIdResult.Result is not ServiceResultType.Success)
            {
                return StatusCode((int)userIdResult.Result, userIdResult.ErrorMessage);
            }

            var result = await _orderService.CompleteOrdersAsync(userIdResult.Data);
            if (result.Result is not ServiceResultType.Success)
            {
                return StatusCode((int)result.Result, result.ErrorMessage);
            }

            return NoContent();
        }
    }
}