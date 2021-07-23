using GameStore.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameStore.BL.Services
{
    public class RoleService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole<int>> _roleManager;

        public RoleService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole<int>> roleManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public ActionResult<IEnumerable<IdentityRole<int>>> GetRoles()
        {
            return _roleManager.Roles.ToList();
        }

        public async Task<bool> Create(string roleName)
        {
            if (!string.IsNullOrEmpty(roleName))
            {
                IdentityResult result = await _roleManager.CreateAsync(new IdentityRole<int>(roleName));
                if (result.Succeeded)
                {
                    return true;
                }
            }
            return false;
        }

        public async Task<bool> Delete(string id)
        {
            IdentityRole<int> role = await _roleManager.FindByIdAsync(id);
            if (role != null)
            {
                var res = await _roleManager.DeleteAsync(role);
                if (res.Succeeded)
                {
                    return true;
                }
            }
            return false;
        }

        public async Task Edit(string userId, string roleName)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                var userRoles = _userManager.GetRolesAsync(user).Result;
                await _userManager.RemoveFromRolesAsync(user, userRoles);
                await _userManager.AddToRoleAsync(user, roleName);
            }
        }
    }
}