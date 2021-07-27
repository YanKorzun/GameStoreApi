using AutoMapper;
using GameStore.DAL.Entities;
using GameStore.WEB.DTO;

namespace GameStore.BL.Mappers
{
    public class UserModelProfile : Profile
    {
        public UserModelProfile()
        {
            CreateMap<UserModel, ApplicationUser>();
            CreateMap<ApplicationUser, UserModel>();

            CreateMap<UserWithRoleModel, ApplicationUser>();
            CreateMap<ApplicationUser, UserWithRoleModel>();
        }
    }
}