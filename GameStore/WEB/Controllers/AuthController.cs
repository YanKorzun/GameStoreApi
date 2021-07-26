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
        public async Task<ActionResult> SignUp([FromBody] UserModel userDTO)
        {
            var signUpResult = await _userService.SignUp(userDTO.Email, userDTO.Password, userDTO.Username);

            if (signUpResult.Result is ServiceResultType.Success)
            {
                var confirmationLink = Url.Action("ConfirmEmail", "Auth", new { id = signUpResult.Data.user.Id, signUpResult.Data.confirmToken }, Request.Scheme);
                await _emailSender.SendEmailAsync(signUpResult.Data.user.Email, EmailSubjects.AccountConfirmation, $"<a href='{confirmationLink}'>confirm</a>");
                return CreatedAtAction("SignUp", signUpResult.Data.user);
            }
            return BadRequest();
        }

        [HttpPost("sign-in")]
        public async Task<ActionResult<string>> SignIn([FromBody] UserModel userDTO)
        {
            var Token = await _userService.SignIn(userDTO, _appSettings);

            if (Token is not null)
            {
                return Ok(Token);
            }
            return Unauthorized();
        }

        [HttpGet("email-confirmation")]
        public IActionResult ConfirmEmail(string id, string confirmToken)
        {
            if (_userService.Confirm(id, confirmToken).Result.Result is ServiceResultType.Success)
            {
                return NoContent();
            }
            return BadRequest();
        }

        [HttpPost("sign-out")]
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return Ok();
        }
    }
}