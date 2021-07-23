using GameStore.BL.Services;
using GameStore.DAL.Entities;
using GameStore.WEB.DTO;
using GameStore.WEB.Settings;
using GameStore.WEB.Validators;
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
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserService _userService;
        private readonly AppSettings _appSettings;

        public AuthController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, AppSettings appSettings)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _appSettings = appSettings;
            _userService = new(_userManager, _signInManager);
        }

        [HttpPost("sign-up")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<int>> SignUp([FromBody] UserDTO userDTO)
        {
            var IsEmailValid = UserDTOValidator.IsValidEmail(userDTO.Email);
            if (!IsEmailValid)
            {
                return BadRequest("Wrong email format");
            }
            var IsPasswordValid = UserDTOValidator.IsPasswordValid(userDTO.Password);
            if (!IsPasswordValid)
            {
                return BadRequest("Wrong password format");
            }

            var IsRegistered = await _userService.SignUp(userDTO.Email, userDTO.Password, userDTO.Username);

            if (IsRegistered)
            {
                return StatusCodes.Status201Created;
            }
            return BadRequest();
        }

        [HttpPost("sign-in")]
        public async Task<ActionResult<string>> SignIn([FromBody] UserDTO userDTO)
        {
            var IsEmailValid = UserDTOValidator.IsValidEmail(userDTO.Email);
            if (!IsEmailValid)
                return Unauthorized();
            var IsPasswordValid = UserDTOValidator.IsPasswordValid(userDTO.Password);
            if (!IsPasswordValid)
            {
                return Unauthorized();
            }
            var Token = await _userService.SignIn(userDTO, _appSettings);

            if (Token is not null)
            {
                return Ok(Token);
            }
            return Unauthorized();
        }

        [HttpGet("email-confirmation")]
        public IActionResult Confirm(int id, string token)
        {
            if (_userService.Confirm(id, token))
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