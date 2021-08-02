using AutoMapper;
using GameStore.BL.Enums;
using GameStore.BL.Interfaces;
using GameStore.BL.Services;
using GameStore.DAL.Entities;
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
        private readonly IClaimsUtility _claimsUtility;
        private readonly IMapper _mapper;

        public UserController(IUserService userService, IClaimsUtility claimsUtility, IMapper mapper)
        {
            _userService = userService;
            _claimsUtility = claimsUtility;
            _mapper = mapper;
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateUser(UserModel user)
        {
            var profileUpdateResult = await _userService.UdpateUserProfileAsync(User, user);
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
            var user = await _claimsUtility.GetUserFromClaimsAsync(User);
            var userModel = _mapper.Map<UserWithPasswordModel>(user.Data);
            patch.ApplyTo(userModel);

            var passwordUpdateResult = await _userService.UdpateUserPasswordAsync(user.Data, userModel);
            if (passwordUpdateResult.Result is not ServiceResultType.Success)
            {
                return BadRequest();
            }

            return NoContent();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApplicationUser>> GetInfo()
        {
            var searchResult = await _claimsUtility.GetUserFromClaimsAsync(User);
            if (searchResult.Result is not ServiceResultType.Success)
            {
                return BadRequest();
            }

            return Ok(searchResult.Data);
        }
    }
}