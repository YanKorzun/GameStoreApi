using AutoMapper;
using GameStore.DAL.Entities;
using GameStore.WEB.DTO.UserModels;

namespace GameStore.BL.Mappers
{
    public class UserModelProfile : Profile
    {
        public UserModelProfile()
        {
            CreateMap<BasicUserModel, ApplicationUser>().ReverseMap();

            CreateMap<BasicUserRoleModel, ApplicationUser>().ReverseMap();

            CreateMap<UpdateUserModel, ApplicationUser>().ReverseMap();
        }
    }
}