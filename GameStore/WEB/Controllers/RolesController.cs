using GameStore.BL.Enums;
using GameStore.BL.Services;
using GameStore.WEB.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GameStore.WEB.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/roles")]
    public class RolesController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RolesController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] RoleModel role)
        {
            var result = await _roleService.CreateAsync(role);
            if (result.Result is not ServiceResultType.Success)
            {
                return BadRequest();
            }
            return CreatedAtAction(nameof(Create), result);
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(string id)
        {
            var IsDeleted = await _roleService.DeleteAsync(id);

            if (IsDeleted.Result is not ServiceResultType.Success)
            {
                return BadRequest();
            }
            return Ok();
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Edit([FromBody] UserWithRoleModel role)
        {
            await _roleService.EditAsync(role);

            return NoContent();
        }
    }
}