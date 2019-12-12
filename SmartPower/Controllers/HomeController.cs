using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SmartPower.DataContext;
using SmartPower.Models;
using SmartPower.Services;

namespace SmartPower.Controllers
{
    public class HomeController : Controller
    {
        private readonly PowerDbContext _con;

        public HomeController(PowerDbContext context)
        {
            _con = context;
        }
        public IActionResult Index()
        {
           return RedirectToAction("Logo", "Logo");
        }
       public IActionResult Configuration()
        {
           return View();
        }


        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult Status()
        {
            ReportService rs = new ReportService(_con);
            PrimarySourceSerivce ps = new PrimarySourceSerivce(_con);
            SecoundrySourceService ss = new SecoundrySourceService(_con);
            ViewBag.primaries = ps.GetAllPrimarySources();
            ViewBag.secondaries = ss.GetAllSecoundrySources();
           var model = rs.GetSourceStatus();
            return View(model);
        }
    }
}
