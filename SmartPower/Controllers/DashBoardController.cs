using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SmartPower.DataContext;
using SmartPower.Services;
using SmartPower.Domin;
using SmartPower.Controllers.Domin;

namespace SmartPower.Controllers
{
    public class DashBoardController : Controller
    {
        private readonly PowerDbContext _Context;
        public DashBoardController(PowerDbContext _context)
        {
            _Context = _context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult DashBoard()
        {
            FactoryService fs = new FactoryService(_Context);
            ViewBag.factories = fs.GetAllFactoriesSimple();
            DashboardViewModel res = new DashboardViewModel
            {
                datetime = DateTime.Now
            };
            return View(res) ;
        }
        public DashboardViewModel DashBoard2(DateTime ChosenDate, int fac_id = -1) // primary
        {
            //FactoryService fs = new FactoryService(_Context);
            //ViewBag.factories = fs.GetAllFactoriesSimple();

            DashBoardServices ds = new DashBoardServices(_Context);
            DashboardViewModel res = new DashboardViewModel();

            if (fac_id != -1)
            {
                ds.GetdateOfSourcesOfPrimaries(fac_id, ChosenDate, ref res);
                res.fac_id = fac_id;
                res.Fac_Name = (_Context.Factory.Single(s => s.Id == fac_id)).Name;
                res.bol = true;
            }
            else
            {
                res.datetime = DateTime.Now;
            }

            
            return res;
        }

        public IActionResult LoadDashBoard()
        {
            FactoryService fs = new FactoryService(_Context);
            ViewBag.factories = fs.GetAllFactoriesSimple();
            DashboardViewModel res = new DashboardViewModel
            {
                datetime = DateTime.Now
            };
            return View(res);
        }
        public DashboardViewModel getLoadDashBoard(DateTime ChosenDate, int fac_id = -1) // primary
        {
            DashBoardServices ds = new DashBoardServices(_Context);
            DashboardViewModel res = new DashboardViewModel();

            if (fac_id != -1)
            {
                ds.GetdateOfSourcesOfLoads(fac_id, ChosenDate, ref res);
                res.fac_id = fac_id;
                res.Fac_Name = (_Context.Factory.Single(s => s.Id == fac_id)).Name;
                res.bol = true;
            }
            else
            {
                res.datetime = DateTime.Now;
            }


            return res;
        }
        public LoadDataModel GetLoadDetails(int Id)
        {
            LoadsServices ls = new LoadsServices(_Context);
            return ls.GetLoadById(Id);  
           
        }

    }
}