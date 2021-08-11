using System.Collections.Generic;
using AutoMapper;
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

            CreateMap<List<Product>, ProductModel>().ReverseMap();
        }
    }
}