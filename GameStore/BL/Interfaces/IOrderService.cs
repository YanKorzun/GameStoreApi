using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using GameStore.WEB.DTO.OrderModels;

namespace GameStore.BL.Interfaces
{
    public interface IOrderService
    {
        Task<List<ExtendedOrderModel>> GetOrdersAsync(ClaimsPrincipal user, int? id);

        Task<ExtendedOrderModel> CreateOrderAsync(OrderModel orderModel, ClaimsPrincipal user);

        Task DeleteOrders(ICollection<ExtendedOrderModel> orders);

        Task CompleteOrders(ClaimsPrincipal user);
    }
}