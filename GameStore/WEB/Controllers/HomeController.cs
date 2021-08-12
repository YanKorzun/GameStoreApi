using GameStore.WEB.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GameStore.WEB.Controllers
{
    [ApiController]
    [Route("api/home")]
    public class HomeController : ControllerBase
    {
        private readonly ILogger _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [Authorize(Roles = UserRoleConstants.Admin)]
        [HttpGet("info")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public ActionResult<string> GetInfo()
        {
            _logger.LogInformation("Log in home controller is working fine");
            return User.Identity.Name + "\n" + User.Identity.IsAuthenticated + "\n";
        }

        [HttpGet("error")]
        public ActionResult GetError() => throw new("Access denied");
    }
}