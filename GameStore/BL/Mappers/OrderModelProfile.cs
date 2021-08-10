using AutoMapper;
using GameStore.DAL.Entities;
using GameStore.WEB.DTO.OrderModels;

namespace GameStore.BL.Mappers
{
    public class OrderModelProfile : Profile
    {
        public OrderModelProfile()
        {
            CreateMap<Order, OrderModel>().ReverseMap();
            CreateMap<Order, ExtendedOrderModel>().ReverseMap();
        }
    }
}