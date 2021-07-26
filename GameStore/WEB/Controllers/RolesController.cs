using GameStore.BL.Enums;
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
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IRoleService _roleService;

        public RolesController(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, IRoleService roleService)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _roleService = roleService;
        }

        [HttpGet("list")]
        public ActionResult<IList<ApplicationRole>> GetRoles() => _roleService.GetRoles().Data;

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RoleModel role)
        {
            var result = await _roleService.CreateAsync(role.RoleName);
            if (result.Result is ServiceResultType.Success)
            {
                return Ok();
            }
            return BadRequest();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            var IsDeleted = await _roleService.DeleteAsync(id);

            if (IsDeleted.Result is ServiceResultType.Success)
            {
                return Ok();
            }
            return BadRequest();
        }

        [HttpPut]
        public async Task<IActionResult> Edit([FromBody] RoleModel role)
        {
            await _roleService.EditAsync(role.UserId, role.RoleName);
            return Ok();
        }
    }
}