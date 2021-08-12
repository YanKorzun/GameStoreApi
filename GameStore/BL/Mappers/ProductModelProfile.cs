using AutoMapper;
using GameStore.DAL.Entities;
using GameStore.WEB.DTO.Products;

namespace GameStore.BL.Mappers
{
    public class ProductModelProfile : Profile
    {
        public ProductModelProfile()
        {
            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<Product, ExtendedProductDto>().ReverseMap();
        }
    }
}