using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using System;

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

        [HttpGet("info")]
        public ActionResult<string> GetInfo()
        {
            if (User.IsInRole("admin"))
            {
                return "User is admin";
            }
            _logger.LogInformation("Log in home controller is working fine");
            return User.Identity.Name + "\n" + User.Identity.IsAuthenticated + "\n";
        }

        [HttpGet("error")]
        public ActionResult GetError()
        {
            throw new Exception("Access denied");
        }

        [Authorize(Roles = "admin")]
        [HttpPost("secret")]
        public ActionResult<string> GetSecret()
        {
            return "Welcome";
        }
    }
}