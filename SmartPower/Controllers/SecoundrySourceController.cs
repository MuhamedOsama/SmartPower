using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SmartPower.Controllers.Domin;
using SmartPower.DataContext;
using SmartPower.Models;
using SmartPower.Services;


namespace SmartPower.Controllers
{
    public class SecoundrySourceController : Controller
    {
        private readonly PowerDbContext _Context;

        public SecoundrySourceController(PowerDbContext _context)
        {
            _Context = _context;
        }

        public List<secondarySource> GetAllSecondarySourcesbyfacId(int factoryid)  //type of distination 
        {
            SecoundrySourceService ss = new SecoundrySourceService(_Context);
            return ss.GetAllSecondarySourcesbyfacId(factoryid);
        }

        public IActionResult Index(int facid = -1 , int primary = -1)
        {
            List<SecoundrySouresDataModelSim> allsec; 
            FactoryService fs = new FactoryService(_Context);
            ViewBag.factories = fs.GetAllFactoriesSimple();
            SecoundrySourceService ss = new SecoundrySourceService(_Context);

            if (facid != -1)
                allsec = ss.GetSpecificSecondary(facid, primary);
            else
                allsec = ss.GetAllSecoundrySources();   

     
            return View(allsec);
        }
        [HttpGet]
        public IActionResult homeCreate(int Id)
        {
            FactoryService fs = new FactoryService(_Context);
            SecoundrySourceService ss = new SecoundrySourceService(_Context);
            ViewBag.factories = fs.GetAllFactoriesSimple();
            ViewBag.secondaries = ss.GetAllSecoundrySources();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> homeCreate(SecoundrySouresDataModelSim obj)
        {
            SecoundrySourceService ss = new SecoundrySourceService(_Context);

            var y = ss.GetSecondaryByName(obj.Name, obj.PS_Id);
            if (y != null)
            {
                FactoryService fs = new FactoryService(_Context);
                ViewBag.factories = fs.GetAllFactoriesSimple();
                ModelState.AddModelError("Name", "Name is already exist");
                return View();

            }



            bool created = await ss.CreateSecoundrySourceAsync(obj);

            return RedirectToAction("Index");

        }

        public async Task<IActionResult> Delete(int Id)
        {
            SecoundrySourceService ps = new SecoundrySourceService(_Context);
            int P_Id = ps.GetPrimarySourceId(Id);
            bool deleted = await ps.Delete(Id);
            return RedirectToAction("Index");
        }
        public IActionResult Details(int Id)
        {
            SecoundrySourceService ss = new SecoundrySourceService(_Context);
            var SecondryDetails = ss.GetSecondaryLoads(Id);
            return View(SecondryDetails);

        }
        [HttpGet]
        public IActionResult Edit(int Id)
        {
            SecoundrySourceService ps = new SecoundrySourceService(_Context);
            var sec = ps.MappingToSecoundrySourceDataModel(ps.GetSecoundrySourceFromDBByCode(Id));
            return View(sec);
        }




        [HttpPost]
        public async Task<IActionResult> Edit(SecoundrySouresDataModelSim obj)
        {
            SecoundrySourceService ps = new SecoundrySourceService(_Context);
            bool tr = await ps.EditSecondarySource(obj);
            return RedirectToAction(nameof(Index));
        }

    }
}