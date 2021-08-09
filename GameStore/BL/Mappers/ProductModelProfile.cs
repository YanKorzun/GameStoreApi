using AutoMapper;
using GameStore.BL.Utilities;
using GameStore.DAL.Entities;
using GameStore.WEB.DTO.ProductModels;

namespace GameStore.BL.Mappers
{
    public class ProductModelProfile : Profile
    {
        public ProductModelProfile()
        {
            CreateMap<Product, ProductModel>().ReverseMap();
            CreateMap<Product, ExtendedProductModel>().ReverseMap();

            CreateMap<PagedList<Product>, ProductModel>().ReverseMap();
        }
    }
}