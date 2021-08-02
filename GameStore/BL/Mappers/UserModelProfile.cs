using AutoMapper;
using GameStore.DAL.Entities;
using GameStore.WEB.DTO;

namespace GameStore.BL.Mappers
{
    public class UserModelProfile : Profile
    {
        public UserModelProfile()
        {
            CreateMap<UserWithPasswordModel, ApplicationUser>().ReverseMap();

            CreateMap<UserWithRoleModel, ApplicationUser>().ReverseMap();

            CreateMap<UserModel, ApplicationUser>().ReverseMap();
        }
    }
}