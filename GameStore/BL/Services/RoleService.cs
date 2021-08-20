using System.Threading.Tasks;
using AutoMapper;
using GameStore.BL.Enums;
using GameStore.BL.Interfaces;
using GameStore.BL.ResultWrappers;
using GameStore.DAL.Entities;
using GameStore.WEB.DTO.Roles;
using GameStore.WEB.DTO.Users;
using Microsoft.AspNetCore.Identity;

namespace GameStore.BL.Services
{
    public class RoleService : IRoleService
    {
        private readonly IMapper _mapper;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public RoleService(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager,
            IMapper mapper)
        {
            _roleManager = roleManager;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<ServiceResult> CreateAsync(RoleDto roleDto)
        {
            var applicationRole = _mapper.Map<ApplicationRole>(roleDto);

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

        public async Task<ServiceResult> EditAsync(BasicUserRoleDto basicUserRoleModel)
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