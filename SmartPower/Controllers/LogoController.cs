using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace SmartPower.Controllers
{
    public class LogoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Logo()
        {
            return View();
        }
    }
}