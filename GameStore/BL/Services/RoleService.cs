using GameStore.BL.Enums;
using GameStore.BL.ResultWrappers;
using GameStore.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameStore.BL.Services
{
    public class RoleService : IRoleService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public RoleService(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public ServiceResult<ActionResult<IList<ApplicationRole>>> GetRoles() => new() { Result = ServiceResultType.Success, Data = _roleManager.Roles.AsNoTracking().Take(30).ToList() };

        public async Task<ServiceResult> CreateAsync(string roleName)
        {
            if (!string.IsNullOrEmpty(roleName))
            {
                IdentityResult result = await _roleManager.CreateAsync(new ApplicationRole(roleName));
                if (result.Succeeded)
                {
                    return new(ServiceResultType.Success);
                }
            }
            return new(ServiceResultType.InternalError);
        }

        public async Task<ServiceResult> DeleteAsync(string id)
        {
            ApplicationRole role = await _roleManager.FindByIdAsync(id);
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

        public async Task EditAsync(string userId, string roleName)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                await _userManager.RemoveFromRolesAsync(user, userRoles);
                await _userManager.AddToRoleAsync(user, roleName);
            }
        }
    }
}