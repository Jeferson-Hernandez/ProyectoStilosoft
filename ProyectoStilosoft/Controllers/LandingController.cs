using Microsoft.AspNetCore.Mvc;
using Stilosoft.Business.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stilosoft.Controllers
{
    public class LandingController : Controller
    {       
        public IActionResult Index()
        {
            return View();
        }
    }
}
