using AutoMapper;
using GameStore.DAL.Entities;
using GameStore.WEB.DTO;

namespace GameStore.BL.Mappers
{
    public class RoleModelProfile : Profile
    {
        public RoleModelProfile()
        {
            CreateMap<RoleModel, ApplicationRole>();
            CreateMap<ApplicationUser, UserModel>();
        }
    }
}