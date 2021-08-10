using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using GameStore.BL.Interfaces;
using GameStore.DAL.Entities;
using GameStore.DAL.Enums;
using GameStore.DAL.Interfaces;
using GameStore.WEB.DTO.OrderModels;

namespace GameStore.BL.Services
{
    public class OrderService : IOrderService
    {
        private readonly IClaimsUtility _claimsUtility;
        private readonly IMapper _mapper;
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository, IClaimsUtility claimsUtility, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _claimsUtility = claimsUtility;
            _mapper = mapper;
        }

        public async Task<List<ExtendedOrderModel>> GetOrdersAsync(ClaimsPrincipal user, int? id = null)
        {
            int searchId;
            List<Order> orders;
            if (id is null)
            {
                searchId = _claimsUtility.GetUserIdFromClaims(user).Data;

                orders = await _orderRepository.GetOrdersAsync(o => o.UserId == searchId);
            }
            else
            {
                orders = await _orderRepository.GetOrdersAsync(o => o.Id == id);
            }

            var orderModels = _mapper.Map<List<ExtendedOrderModel>>(orders);

            return orderModels;
        }

        public async Task<ExtendedOrderModel> CreateOrderAsync(OrderModel orderModel, ClaimsPrincipal user)
        {
            var order = _mapper.Map<Order>(orderModel);

            var createdOrder = await _orderRepository.CreateItemAsync(order);

            var createdOrderModel = _mapper.Map<ExtendedOrderModel>(createdOrder);

            return createdOrderModel;
        }

        public async Task DeleteOrders(ICollection<ExtendedOrderModel> orderModels)
        {
            var orders = _mapper.Map<ICollection<Order>>(orderModels);
            await _orderRepository.RemoveOrderRange(orders);
        }

        public async Task CompleteOrders(ClaimsPrincipal user)
        {
            var searchId = _claimsUtility.GetUserIdFromClaims(user).Data;

            var orderModels = await _orderRepository.GetOrdersAsync(o => o.UserId == searchId);

            var orders = _mapper.Map<ICollection<Order>>(orderModels);

            foreach (var order in orders)
            {
                order.Status = OrderStatus.Completed;
            }

            var updateResult = await _orderRepository.UpdateItemsAsync(orders);
        }
    }
}