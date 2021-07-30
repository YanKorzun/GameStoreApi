using GameStore.BL.Enums;
using GameStore.BL.Services;
using GameStore.WEB.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GameStore.WEB.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateUser(UserModel user)
        {
            var profileUpdateResult = await _userService.UdpateUserProfileAsync(HttpContext.User, user);
            if (profileUpdateResult.Result is not ServiceResultType.Success)
            {
                return BadRequest();
            }

            return NoContent();
        }

        [HttpPatch("password")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdatePassword([FromBody] JsonPatchDocument<UserWithPasswordModel> patch)
        {
            var passwordUpdateResult = await _userService.UdpateUserPasswordAsync(HttpContext.User, patch);
            if (passwordUpdateResult.Result is not ServiceResultType.Success)
            {
                return BadRequest();
            }

            return NoContent();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetInfo()
        {
            var searchResult = await _userService.GetUserFromClaimsAsync(HttpContext.User);
            if (searchResult.Result is not ServiceResultType.Success)
            {
                return BadRequest();
            }

            return Ok(searchResult.Data);
        }
    }
}