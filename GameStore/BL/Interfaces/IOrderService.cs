using System.Collections.Generic;
using System.Threading.Tasks;
using GameStore.BL.ResultWrappers;
using GameStore.WEB.DTO.Orders;

namespace GameStore.BL.Interfaces
{
    public interface IOrderService
    {
        Task<List<OutOrderDto>> GetOrdersAsync(int userId, int[] ordersId = null);

        Task<ServiceResult<OutOrderDto>> CreateOrderAsync(BasicOrderDto order);

        Task DeleteOrders(ICollection<ExtendedOrderDto> orders);

        Task<ServiceResult> CompleteOrders(int userId);

        Task<OutOrderDto> UpdateItemsAsync(ExtendedOrderDto orderDto);
    }
}