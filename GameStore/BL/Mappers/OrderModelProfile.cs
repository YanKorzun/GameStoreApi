using AutoMapper;
using GameStore.DAL.Entities;
using GameStore.WEB.DTO.OrderModels;

namespace GameStore.BL.Mappers
{
    public class OrderModelProfile : Profile
    {
        public OrderModelProfile()
        {
            CreateMap<Order, BasicOrderModel>().ReverseMap();
            CreateMap<Order, OutOrderModel>().ReverseMap();
            CreateMap<Order, ExtendedOrderModel>().ReverseMap();
        }
    }
}