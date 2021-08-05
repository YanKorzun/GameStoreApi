using AutoMapper;
using GameStore.BL.Enums;
using GameStore.BL.Interfaces;
using GameStore.BL.ResultWrappers;
using GameStore.DAL.Entities;
using GameStore.WEB.DTO;
using GameStore.WEB.DTO.UserModels;
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
            if (role is null)
            {
                return new(ServiceResultType.NotFound);
            }

            var res = await _roleManager.DeleteAsync(role);
            if (!res.Succeeded)
            {
                return new(ServiceResultType.InternalError);
            }

            return new(ServiceResultType.Success);
        }

        public async Task<ServiceResult> EditAsync(BasicUserRoleModel basicUserRoleModel)
        {
            var user = await _userManager.FindByEmailAsync(basicUserRoleModel.Email);
            if (user is null)
            {
                return new(ServiceResultType.NotFound);
            }

            var userRoles = await _userManager.GetRolesAsync(user);

            await _userManager.RemoveFromRolesAsync(user, userRoles);

            var identityRole = await _roleManager.FindByNameAsync(basicUserRoleModel.Role);
            if (identityRole is null)
            {
                await CreateAsync(new(basicUserRoleModel.Role));
            }

            await _userManager.AddToRoleAsync(user, basicUserRoleModel.Role);

            return new(ServiceResultType.Success);
        }
    }
}