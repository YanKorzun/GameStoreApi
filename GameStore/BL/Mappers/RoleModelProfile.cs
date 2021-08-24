using AutoMapper;
using GameStore.DAL.Entities;
using GameStore.WEB.DTO.Roles;

namespace GameStore.BL.Mappers
{
    public class RoleModelProfile : Profile
    {
        public RoleModelProfile()
        {
            CreateMap<RoleDto, ApplicationRole>().ReverseMap();
        }
    }
}