using AutoMapper;
using GameStore.DAL.Entities;
using GameStore.WEB.DTO.Orders;

namespace GameStore.BL.Mappers
{
    public class OrderModelProfile : Profile
    {
        public OrderModelProfile()
        {
            CreateMap<Order, BasicOrderDto>().ReverseMap();
            CreateMap<Order, OutputOrderDto>().ReverseMap();
            CreateMap<Order, ExtendedOrderDto>().ReverseMap();
        }
    }
}