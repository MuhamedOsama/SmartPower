using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SmartPower.Controllers.Domin;
using SmartPower.DataContext;
using SmartPower.Services;
using SmartPower.Models; 
namespace SmartPower.Controllers
{
    public class PrimarySourceController : Controller
    {
        private readonly PowerDbContext _Context;

        public PrimarySourceController(PowerDbContext _context )
        {
            _Context = _context;
          
        }

        public List<PrimarySource> GetprimarySourcebyfacid(int factoryid)  //type of distination 
        {
            PrimarySourceSerivce ps = new PrimarySourceSerivce(_Context);
            return ps.GetprimarySourcebyfacid(factoryid); 
        }

        public IActionResult Index(int Id = -1)
        {
            PrimarySourceSerivce ps = new PrimarySourceSerivce(_Context);

            List<PrimarySourceDataModel> Model = null;
            FactoryService fs = new FactoryService(_Context);
            ViewBag.factories = fs.GetAllFactoriesSimple();
            if (Id != -1)
            {
               Model = ps.GetAllPrimarySourcesbyfacId(Id); 
            }
            else
            {
                Model = ps.GetAllPrimarySources();
            }
            return View(Model);
        }
        [HttpGet]
        public IActionResult Create(int Id)
        {
            if (Id != -1)
            {
                PrimarySourceDataModel viewModel = new PrimarySourceDataModel()
                {
                    FacId = Id
                };
                return View(viewModel);
            }
            else
            {

                FactoryService fs = new FactoryService(_Context);
                PrimarySourceSerivce ps = new PrimarySourceSerivce(_Context);
                ViewBag.factories = fs.GetAllFactoriesSimple();
                ViewBag.primaries = ps.GetAllPrimarySources();
                
              
                return View("homeCreate");
            }
            
        }
        [HttpPost]
        public async Task<IActionResult> Create(PrimarySourceDataModel obj)
        {
            PrimarySourceSerivce ps = new PrimarySourceSerivce(_Context);
            var y = ps.GetPrimaryByName(obj.Name , obj.FacId);
            if (y != null)
            {
                FactoryService fs = new FactoryService(_Context);
                ViewBag.factories = fs.GetAllFactoriesSimple();
                ModelState.AddModelError("Name", "Name is already exist");
                return View("homeCreate", obj);

            }


            bool created =   await  ps.CreatePrimarySourceAsync(obj);
            return RedirectToAction("Index", "PrimarySource");
        }
        public async Task<IActionResult> Delete(int? Id)
        {
            PrimarySourceSerivce ps = new PrimarySourceSerivce(_Context);
            int FacId = ps.GetFactoryId(Id);
            bool deleted = await ps.Delete(Id);

            
            return RedirectToAction("index"/* , new { Id = FacId }*/);

        }
        public IActionResult Details(int Id)
        {
            PrimarySourceSerivce ps = new PrimarySourceSerivce(_Context);
            var ViewModel =  ps.GetPrimarySourceWithAllData(Id);
            return View(ViewModel);
        }

        [HttpGet]

        public IActionResult Edit(int Id)
        {
            PrimarySourceSerivce ps = new PrimarySourceSerivce(_Context);
            var prim = ps.MappingToPrimarySourceDataModel (ps.GetPrimarySourceFromDB(Id));
            return View(prim); 
        }


      

        [HttpPost]
        public async Task<IActionResult> Edit(PrimarySourceDataModel obj)
        {
            PrimarySourceSerivce ps = new PrimarySourceSerivce(_Context);
           
            bool tr = await ps.EditPrimarySource(obj);  
           return RedirectToAction(nameof(Index));
        }


    }

  
}