using System.Collections.Generic;
using System.Threading.Tasks;
using GameStore.BL.ResultWrappers;
using GameStore.WEB.DTO.Orders;

namespace GameStore.BL.Interfaces
{
    public interface IOrderService
    {
        Task<List<OutputOrderDto>> GetOrdersAsync(int userId, int[] ordersId = null);

        Task<ServiceResult<OutputOrderDto>> CreateOrderAsync(BasicOrderDto order);

        Task DeleteOrders(ICollection<ExtendedOrderDto> orders);

        Task<ServiceResult> CompleteOrders(int userId);

        Task<OutputOrderDto> UpdateItemsAsync(ExtendedOrderDto orderDto);
    }
}