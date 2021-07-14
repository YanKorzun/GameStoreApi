using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GameStore.Controllers
{
    [Controller]
    public class HomeController : Controller
    {
        public string GetInfo()
        {
            return JsonConvert.SerializeObject("Hello World!");
        }
    }
}
