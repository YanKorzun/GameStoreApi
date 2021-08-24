using AutoMapper;
using GameStore.DAL.Entities;
using GameStore.WEB.DTO.Users;

namespace GameStore.BL.Mappers
{
    public class UserModelProfile : Profile
    {
        public UserModelProfile()
        {
            CreateMap<BasicUserDto, ApplicationUser>().ReverseMap();

            CreateMap<BasicUserRoleDto, ApplicationUser>().ReverseMap();

            CreateMap<UpdateUserDto, ApplicationUser>().ReverseMap();
        }
    }
}