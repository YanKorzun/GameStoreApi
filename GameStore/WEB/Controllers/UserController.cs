using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoMapper;
using GameStore.BL.Enums;
using GameStore.BL.Interfaces;
using GameStore.BL.Utilities;
using GameStore.DAL.Entities;
using GameStore.WEB.Constants;
using GameStore.WEB.DTO.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.WEB.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public UserController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        /// <summary>
        ///     Update user profile with provided information
        /// </summary>
        /// <param name="updateUserModel">User data transfer object</param>
        /// <returns>No content</returns>
        /// <response code="204">Updated successfully</response>
        /// <response code="400">If the item is null</response>
        /// <response code="401">User is not authenticated</response>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> UpdateUser(UpdateUserModel updateUserModel)
        {
            var userIdResult = ClaimsUtility.GetUserIdFromClaims(User);
            if (userIdResult.Result is not ServiceResultType.Success)
            {
                return StatusCode((int)userIdResult.Result, userIdResult.ErrorMessage);
            }


            var profileUpdateResult = await _userService.UpdateUserProfileAsync(userIdResult.Data, updateUserModel);
            if (profileUpdateResult.Result is not ServiceResultType.Success)
            {
                return BadRequest();
            }

            return NoContent();
        }

        /// <summary>
        ///     Update user password with provided information
        /// </summary>
        /// <param name="patch">Special patch request for updating password</param>
        /// <returns>No content</returns>
        /// <response code="204">Updated successfully</response>
        /// <response code="400">If patch request is scrap</response>
        /// <response code="401">User is not authenticated</response>
        [HttpPatch("password")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> UpdatePassword([FromBody] JsonPatchDocument<BasicUserModel> patch)
        {
            var userIdResult = ClaimsUtility.GetUserIdFromClaims(User);
            if (userIdResult.Result is not ServiceResultType.Success)
            {
                return StatusCode((int)userIdResult.Result, userIdResult.ErrorMessage);
            }

            var user = await _userService.GetUserAsync(userIdResult.Data);
            if (user.Result is not ServiceResultType.Success)
            {
                return BadRequest();
            }

            var userModel = _mapper.Map<BasicUserModel>(user.Data);
            patch.ApplyTo(userModel);
            if (!Regex.IsMatch(userModel.Password, RegexConstants.PasswordRegex))
            {
                return BadRequest();
            }

            var passwordUpdateResult = await _userService.UpdateUserPasswordAsync(user.Data, userModel);
            if (passwordUpdateResult.Result is not ServiceResultType.Success)
            {
                return BadRequest();
            }

            return NoContent();
        }

        /// <summary>
        ///     Get information about user
        /// </summary>
        /// <returns>Returns information about user</returns>
        /// <response code="200">Information received successfully</response>
        /// <response code="400">If user claims is junk</response>
        /// <response code="401">User is not authenticated</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ApplicationUser>> GetInfo()
        {
            var userIdResult = ClaimsUtility.GetUserIdFromClaims(User);
            if (userIdResult.Result is not ServiceResultType.Success)
            {
                return StatusCode((int)userIdResult.Result, userIdResult.ErrorMessage);
            }

            var searchResult = await _userService.GetUserAsync(userIdResult.Data);

            if (searchResult.Result is not ServiceResultType.Success)
            {
                return BadRequest();
            }

            return Ok(searchResult.Data);
        }
    }
}