using GameStore.BL.Services;
using GameStore.DAL.Entities;
using GameStore.WEB.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GameStore.WEB.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/roles")]
    public class RolesController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole<int>> _roleManager;
        private readonly RoleService _roleService;

        public RolesController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole<int>> roleManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _roleService = new(_userManager, _roleManager);
        }

        [HttpGet("list")]
        public ActionResult<IEnumerable<IdentityRole<int>>> GetRoles() => _roleService.GetRoles();

        [HttpGet("add")]
        public async Task<ActionResult<bool>> Create([FromBody] RoleDTO role)
        {
            var result = await _roleService.Create(role.RoleName);
            if (result)
            {
                return Ok();
            }

            return BadRequest();
        }

        [HttpPost("delete")]
        public async Task<IActionResult> Delete([FromBody] RoleDTO role)
        {
            var IsDeleted = await _roleService.Delete(role.RoleId);

            if (IsDeleted)
            {
                return Ok();
            }
            return BadRequest();
        }

        [HttpPost("edit")]
        public async Task<IActionResult> Edit([FromBody] RoleDTO role)
        {
            await _roleService.Edit(role.UserId, role.RoleName);
            return Ok();
        }
    }
}