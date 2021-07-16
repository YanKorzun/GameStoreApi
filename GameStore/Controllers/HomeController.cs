using Microsoft.AspNetCore.Mvc;
using Serilog;

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
            Log.Information("Writing to log file with INFORMATION severity level.");
            Log.Debug("Writing to log file with DEBUG severity level.");
            Log.Warning("Writing to log file with WARNING severity level.");
            Log.Error("Writing to log file with ERROR severity level.");
            Log.Fatal("Writing to log file with Fatal severity level.");
            return "Hello World";
        }
    }
}