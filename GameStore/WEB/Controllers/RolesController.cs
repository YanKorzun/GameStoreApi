using System.Threading.Tasks;
using GameStore.BL.Enums;
using GameStore.BL.Interfaces;
using GameStore.WEB.DTO.Roles;
using GameStore.WEB.DTO.UserModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

        /// <summary>
        ///     Create role with provided model properties
        /// </summary>
        /// <param name="roleDto">data transfer object for creating a new role in database</param>
        /// <response code="201">Created successfully</response>
        /// <response code="400">Something went wrong</response>
        /// <response code="401">User is not authenticated</response>
        /// <response code="403">User has no access to this resource</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Create([FromBody] RoleDto roleDto)
        {
            var result = await _roleService.CreateAsync(roleDto);
            if (result.Result is not ServiceResultType.Success)
            {
                return BadRequest();
            }

            return CreatedAtAction(nameof(Create), result);
        }

        /// <summary>
        ///     Deletes role by its id
        /// </summary>
        /// <param name="id">role id in database</param>
        /// <returns></returns>
        /// <response code="204">Role deleted successfully</response>
        /// <response code="400">If something went wrong</response>
        /// <response code="401">User is not authenticated</response>
        /// <response code="403">User has no access to this resource</response>
        /// <response code="404">Role doesn't exist</response>
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Delete(string id)
        {
            var isDeleted = await _roleService.DeleteAsync(id);

            if (isDeleted.Result is not ServiceResultType.Success)
            {
                return BadRequest();
            }

            return NoContent();
        }

        /// <summary>
        ///     Edits role with provided model properties
        /// </summary>
        /// <param name="basicUserRoleModel">data transfer object for updating a new role in database</param>
        /// <response code="204">Edited successfully</response>
        /// <response code="401">User is not authenticated</response>
        /// <response code="403">User has no access to this resource</response>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Edit([FromBody] BasicUserRoleModel basicUserRoleModel)
        {
            await _roleService.EditAsync(basicUserRoleModel);

            return NoContent();
        }
    }
}