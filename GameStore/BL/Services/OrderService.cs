using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Transactions;
using AutoMapper;
using GameStore.BL.Enums;
using GameStore.BL.Interfaces;
using GameStore.BL.ResultWrappers;
using GameStore.DAL.Entities;
using GameStore.DAL.Enums;
using GameStore.DAL.Interfaces;
using GameStore.WEB.DTO.Orders;

namespace GameStore.BL.Services
{
    public class OrderService : IOrderService
    {
        private readonly IProductLibraryService _libraryService;
        private readonly IMapper _mapper;
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository, IMapper mapper,
            IProductLibraryService libraryService)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
            _libraryService = libraryService;
        }

        public async Task<List<OutputOrderDto>> GetOrdersAsync(int userId, int[] ordersId = null)
        {
            Expression<Func<Order, bool>> expression;
            if (ordersId is null)
            {
                expression = order => order.ApplicationUserId == userId;
            }
            else
            {
                expression = order => ordersId.Contains(order.Id);
            }

            var orders = await _orderRepository.GetOrdersAsync(expression);

            var orderModels = orders.Select(_mapper.Map<OutputOrderDto>).ToList();

            return orderModels;
        }

        public async Task DeleteOrders(ICollection<ExtendedOrderDto> orderModels)
        {
            var orders = orderModels.Select(_mapper.Map<Order>).ToList();
            await _orderRepository.SoftRangeRemoveAsync(orders);
        }

        public async Task<ServiceResult> CompleteOrders(int userId)
        {
            var orders = await _orderRepository.GetOrdersAsync(o => o.ApplicationUserId == userId);

            var addedGames = new List<ProductLibraries>();

            var uncompleted = orders.Where(o => o.Status != OrderStatus.Completed).ToList();
            if (!uncompleted.Any())
            {
                return new(ServiceResultType.InternalError,
                    "You have already bought all products from your order list");
            }

            try
            {
                using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                uncompleted.ForEach(order =>
                {
                    order.Status = OrderStatus.Completed;

                    addedGames.Add(new(order.ApplicationUserId, order.ProductId));
                });

                await _orderRepository.UpdateItemsAsync(uncompleted);

                await _libraryService.AddItemsToLibrary(addedGames);

                scope.Complete();
            }
            catch (Exception e)
            {
                return new(ServiceResultType.InternalError, e.Message);
            }

            return new(ServiceResultType.Success);
        }

        public async Task<OutputOrderDto> UpdateItemsAsync(ExtendedOrderDto orderDto)
        {
            var order = _mapper.Map<Order>(orderDto);
            order.UpdateOrderDate = DateTime.Now;

            await _orderRepository.UpdateItemAsync(order, o => o.CreateOrderDate);

            var updatedOrder = (await _orderRepository.GetOrdersAsync(o => o.Id == order.Id)).FirstOrDefault();

            return _mapper.Map<OutputOrderDto>(updatedOrder);
        }

        public async Task<ServiceResult<OutputOrderDto>> CreateOrderAsync(BasicOrderDto orderDto)
        {
            var order = _mapper.Map<Order>(orderDto);
            order.CreateOrderDate = DateTime.Now;

            var unexpectedOrder = await _orderRepository.SearchForSingleItemAsync(o =>
                o.ProductId == orderDto.ProductId && o.ApplicationUserId == orderDto.ApplicationUserId &&
                o.Status != OrderStatus.Completed);
            if (unexpectedOrder is not null)
            {
                return new(ServiceResultType.InternalError,
                    $"This order is already exists, its id is '{unexpectedOrder.Id}'\nYou can edit it and then complete your order");
            }

            var createdOrder = await _orderRepository.CreateItemAsync(order);

            var createdOrderModel = _mapper.Map<OutputOrderDto>(createdOrder);

            return new(ServiceResultType.Success, createdOrderModel);
        }
    }
}