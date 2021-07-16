using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;

namespace GameStore.Controllers
{
    [ApiController]
    [Route("api/home")]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        [Route("info")]
        public ActionResult<string> GetInfo()
        {
            Log.Information("Now im throwing a new exception, lovi aptechku");
            throw new Exception("Exception message without middleware");
            return "Hello World";
        }
    }
}