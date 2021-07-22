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
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole<int>> _roleManager;
        private readonly UserService _userService;
        private readonly AppSettings _appSettings;

        public AuthController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole<int>> roleManager, AppSettings appSettings)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _signInManager = signInManager;
            _userService = new(_userManager, _signInManager);
            _appSettings = appSettings;
        }

        [HttpPost("sign-up")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status402PaymentRequired)]
        public async Task<ActionResult<int>> SignUp([FromBody] UserDTO userDTO)
        {
            var IsRegistered = await _userService.SignUp(userDTO.Email, userDTO.Password, userDTO.Username);
            if (IsRegistered)
            {
                return Ok();
            }
            return StatusCodes.Status402PaymentRequired;

        }

        [HttpPost("sign-in")]
        public async Task<ActionResult<string>> SignIn([FromBody] UserDTO userDTO)
        {
            var Token = await _userService.SignIn(userDTO, _appSettings);
            if (Token is not null)
            {
                return Token;
            }
            return BadRequest();

        }

        [HttpPost("sign-out")]
        public async Task<IActionResult> LogOut()
        {
            // удаляем аутентификационные куки
            await _signInManager.SignOutAsync();
            return Ok();
        }
    }
}