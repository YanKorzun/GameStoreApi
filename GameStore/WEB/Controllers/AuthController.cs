using GameStore.BL.Constants;
using GameStore.BL.Enums;
using GameStore.BL.Interfaces;
using GameStore.BL.Services;
using GameStore.DAL.Entities;
using GameStore.WEB.DTO;
using GameStore.WEB.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GameStore.WEB.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IUserService _userService;
        private readonly AppSettings _appSettings;
        private readonly IEmailSender _emailSender;

        public AuthController(SignInManager<ApplicationUser> signInManager, IEmailSender emailSender, AppSettings appSettings, IUserService userService)
        {
            _emailSender = emailSender;
            _signInManager = signInManager;
            _appSettings = appSettings;
            _userService = userService;
        }

        [HttpPost("sign-up")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> SignUp([FromBody] UserWithPasswordModel userDTO)
        {
            var signUpResult = await _userService.SignUpAsync(userDTO);

            if (signUpResult.Result is not ServiceResultType.Success)
            {
                return BadRequest(signUpResult.ErrorMessage);
            }
            await _userService.SendConfirmationMessageAsync(nameof(ConfirmEmail), "Auth", (signUpResult.Data.user, signUpResult.Data.confirmToken), Request.Scheme);

            return CreatedAtAction(nameof(SignUp), signUpResult.Data.user);
        }

        [HttpPost("sign-in")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<string>> SignIn([FromBody] UserWithPasswordModel userDTO)
        {
            var signInResult = await _userService.SignInAsync(userDTO, _appSettings);

            if (signInResult.Result is not ServiceResultType.Success)
            {
                return StatusCode((int)signInResult.Result, signInResult.ErrorMessage);
            }

            return Ok(signInResult.Data);
        }

        [HttpGet("email-confirmation")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ConfirmEmail(string id, string token)
        {
            var confirmResult = await _userService.ConfirmAsync(id, token);
            if (confirmResult.Result is not ServiceResultType.Success)
            {
                return StatusCode((int)confirmResult.Result, confirmResult.ErrorMessage);
            }

            return NoContent();
        }

        [HttpPost("sign-out")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return Ok();
        }
    }
}