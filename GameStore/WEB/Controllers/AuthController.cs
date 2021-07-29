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
            var signUpResult = await _userService.SignUp(userDTO);

            if (signUpResult.Result is not ServiceResultType.Success)
            {
                return BadRequest();
            }
            var confirmationLink = Url.Action(nameof(ConfirmEmail), "Auth", new { id = signUpResult.Data.user.Id, signUpResult.Data.confirmToken }, Request.Scheme);
            await _emailSender.SendEmailAsync(signUpResult.Data.user.Email, EmailSubjects.AccountConfirmation, $"<a href='{confirmationLink}'>confirm</a>");

            return CreatedAtAction(nameof(SignUp), signUpResult.Data.user);
        }

        [HttpPost("sign-in")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<string>> SignIn([FromBody] UserWithPasswordModel userDTO)
        {
            var token = await _userService.SignIn(userDTO, _appSettings);

            if (token is not null)
            {
                return Ok(token);
            }
            return Unauthorized();
        }

        [HttpGet("email-confirmation")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult ConfirmEmail(string id, string confirmToken)
        {
            if (_userService.Confirm(id, confirmToken).Result.Result is not ServiceResultType.Success)
            {
                return BadRequest();
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