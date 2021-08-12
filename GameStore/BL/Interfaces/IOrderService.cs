using System.Collections.Generic;
using System.Threading.Tasks;
using GameStore.BL.ResultWrappers;
using GameStore.WEB.DTO.OrderModels;

namespace GameStore.BL.Interfaces
{
    public interface IOrderService
    {
        Task<List<OutOrderModel>> GetOrdersAsync(int userId, int[] ordersId = null);

        Task<ServiceResult<OutOrderModel>> CreateOrderAsync(BasicOrderModel order);

        Task DeleteOrders(ICollection<ExtendedOrderModel> orders);

        Task<ServiceResult> CompleteOrders(int userId);

        Task<OutOrderModel> UpdateItemsAsync(ExtendedOrderModel orderModel);
    }
}