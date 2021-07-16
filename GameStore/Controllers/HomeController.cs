using Microsoft.AspNetCore.Mvc;

namespace GameStore.Controllers
{
    [ApiController]
    [Route("api/home")]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        [Route("info")]
        public ActionResult<string> GetInfo() => "Hello World";
    }
}