using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using System;

namespace GameStore.Controllers
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
            _logger.LogInformation("Log in home controller is working fine");
            return "Hello World";
        }

        [HttpGet("error")]
        public ActionResult GetError()
        {
            throw new Exception("Access denied");
        }
    }
}