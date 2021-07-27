using AutoMapper;
using GameStore.BL.Enums;
using GameStore.BL.ResultWrappers;
using GameStore.DAL.Entities;
using GameStore.WEB.DTO;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace GameStore.BL.Services
{
    public class RoleService : IRoleService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IMapper _mapper;

        public RoleService(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, IMapper mapper)
        {
            _roleManager = roleManager;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<ServiceResult> CreateAsync(RoleModel roleModel)
        {
            var applicationRole = _mapper.Map<ApplicationRole>(roleModel);
            var result = await _roleManager.CreateAsync(applicationRole);
            if (!result.Succeeded)
            {
                return new(ServiceResultType.InternalError);
            }
            return new(ServiceResultType.Success);
        }

        public async Task<ServiceResult> DeleteAsync(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role != null)
            {
                var res = await _roleManager.DeleteAsync(role);
                if (res.Succeeded)
                {
                    return new(ServiceResultType.Success);
                }
            }
            return new(ServiceResultType.InternalError);
        }

        public async Task EditAsync(UserWithRoleModel userModel)
        {
            var user = await _userManager.FindByEmailAsync(userModel.Email);
            if (user != null)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                await _userManager.RemoveFromRolesAsync(user, userRoles);
                await _userManager.AddToRoleAsync(user, userModel.Role);
            }
        }
    }
}