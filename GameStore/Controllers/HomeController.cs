using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;

namespace GameStore.Controllers
{
    [ApiController]
    [Route("api/home")]
    public class HomeController : ControllerBase
    {
        [HttpGet("info")]
        public ActionResult<string> GetInfo()
        {
            Log.Information("Some test information here");
            return "Hello World";
        }

        [HttpGet("error")]
        public ActionResult GetError()
        {
            throw new Exception("Access denied");
        }
    }
}