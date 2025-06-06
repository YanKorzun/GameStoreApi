﻿using System.Threading.Tasks;
using GameStore.BL.Enums;
using GameStore.BL.Interfaces;
using GameStore.WEB.DTO.Users;
using GameStore.WEB.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.WEB.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly AppSettings _appSettings;
        private readonly IUserService _userService;

        public AuthController(AppSettings appSettings, IUserService userService)
        {
            _appSettings = appSettings;
            _userService = userService;
        }

        /// <summary>
        ///     Creates a new user in database and sends him a confirmation link
        /// </summary>
        /// <param name="userModel">User data transfer object</param>
        /// <returns>Returns a new user from database</returns>
        /// <response code="201">Returns the newly created item</response>
        /// <response code="400">If the item is null</response>
        [HttpPost("sign-up")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> SignUp([FromBody] BasicUserDto userModel)
        {
            var signUpResult = await _userService.SignUpAsync(userModel);

            if (signUpResult.Result is not ServiceResultType.Success)
            {
                return BadRequest(signUpResult.ErrorMessage);
            }

            await _userService.SendConfirmationMessageAsync(nameof(ConfirmEmail), "Auth",
                (signUpResult.Data.user, signUpResult.Data.confirmToken), Request.Scheme);

            return CreatedAtAction(nameof(SignUp), signUpResult.Data.user);
        }

        /// <summary>
        ///     Returns a new JWT token to registered users
        /// </summary>
        /// <param name="userModel">User data transfer object</param>
        /// <returns>Returns a new JWT token</returns>
        /// <response code="200">Token is generated</response>
        /// <response code="400">Unable to authenticate with provided email or password</response>
        [HttpPost("sign-in")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<string>> SignIn([FromBody] BasicUserDto userModel)
        {
            var signInResult = await _userService.SignInAsync(userModel, _appSettings);

            if (signInResult.Result is not ServiceResultType.Success)
            {
                return StatusCode((int) signInResult.Result, signInResult.ErrorMessage);
            }

            return Ok(signInResult.Data);
        }

        /// <summary>
        ///     Confirms user email
        /// </summary>
        /// <param name="id">User id</param>
        /// <param name="token">Email confirmation token</param>
        /// <returns>No content</returns>
        /// <response code="204">Email confirmed successfully</response>
        /// <response code="400">Email cannot be confirmed</response>
        [HttpGet("email-confirmation")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ConfirmEmail(string id, string token)
        {
            var confirmResult = await _userService.ConfirmAsync(id, token);
            if (confirmResult.Result is not ServiceResultType.Success)
            {
                return StatusCode((int) confirmResult.Result, confirmResult.ErrorMessage);
            }

            return NoContent();
        }
    }
}