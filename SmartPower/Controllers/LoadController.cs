using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SmartPower.Controllers.Domin;
using SmartPower.DataContext;
using SmartPower.Services;


namespace SmartPower.Controllers
{
    public class LoadController : Controller
    {
        private  PowerDbContext _Context;
        public LoadController(PowerDbContext _context)
        {
            _Context = _context;

        }
        public IActionResult Index(int facid = -1,int PrimOrSec = -1 , int sourceid = -1)
        {
            FactoryService fs = new FactoryService(_Context);
            ViewBag.factories = fs.GetAllFactoriesSimple();
            LoadsServices ls = new LoadsServices(_Context);
            List<LoadDataModel> ld = ls.GetAllLoads();         
            if (facid != -1)
            {
                if (sourceid != -1)
                   ld = ls.MappingToLsDM(_Context.Load.Where(s => s.SourceId == sourceid).ToList());              
                else 
                {
                    List<LoadDataModel> tmp = new List<LoadDataModel>();                 
                    foreach (var item in ld)
                    {
                        int fid = 0;

                        if (item.SourceId % 2 != 0)
                        {
                            fid = _Context.PrimarySource.SingleOrDefault(p => p.Code == Convert.ToString(item.SourceId)).FactoryId;
                            if (PrimOrSec == 1 && facid == fid) tmp.Add(item);
                        }
                        if (item.SourceId % 2 == 0)
                        {
                            fid = _Context.secondarySource.SingleOrDefault(p => p.Code == Convert.ToString(item.SourceId)).Fac_Id;
                            if (PrimOrSec == 2 && facid == fid) tmp.Add(item);

                        }

                        if (PrimOrSec == -1 && facid == fid) tmp.Add(item);

                    }
                    ld = tmp;  
                }

            }        
            return View(ld);
        }
        [HttpGet]
        public IActionResult Create(int Id)
        {
            LoadDataModel model = new LoadDataModel
            {
                secondarySourceId = Id // secondrysource id
            };       
            return View(model); 
        }

        [HttpPost]
        public async Task<IActionResult> Create(LoadDataModel obj)
        {
            LoadsServices ls = new LoadsServices(_Context);

            var y = ls.GetLoadByName(obj.name, obj.SourceId);
            if (y != null)
            {
                FactoryService fs = new FactoryService(_Context);
                ViewBag.factories = fs.GetAllFactoriesSimple();
                ModelState.AddModelError("Name", "Name is already exist");
                return View("homecreate", obj);

            }


            bool created = await ls.CreateLoadAsync(obj);

            return RedirectToAction("index", "load", new { Id = obj.secondarySourceId });
        }

        [HttpGet]
        public IActionResult Edit(int Id)
        {
            LoadsServices ld = new LoadsServices(_Context);
            var load = ld.GetLoadById(Id);
            ViewBag.Functions = ld.GetAllFunctions();
            return View(load); 

        }
        [HttpPost]
        public ActionResult Edit(LoadDataModel obj)
        {
            LoadsServices ld = new LoadsServices(_Context);
            var load = ld.EditLoadAsyn(obj);

            return RedirectToAction("Index");
        }


        [HttpGet]
        public IActionResult homeCreate(int Id)
        {
            FactoryService fs = new FactoryService(_Context);
            LoadsServices ls = new LoadsServices(_Context);

            ViewBag.factories = fs.GetAllFactoriesSimple();
            ViewBag.Functions = ls.GetAllFunctions();

            return View();
        }


        [HttpPost]
        public async Task<IActionResult> homeCreate(LoadDataModel obj)
        {
            LoadsServices ls = new LoadsServices(_Context);
            var y = ls.GetLoadByName(obj.name, obj.SourceId);
            if (y != null)
            {
                FactoryService fs = new FactoryService(_Context);
                ViewBag.factories = fs.GetAllFactoriesSimple();
                ModelState.AddModelError("Name", "Name is already exist");
                return View("homecreate", obj);

            }

            bool ch =  await ls.CreateLoadAsync(obj);
            return RedirectToAction("Index");
            
            
        }

        public async Task<IActionResult> Delete(int Id)
        {
            LoadsServices ls = new LoadsServices(_Context);         
            bool x = await ls.DeleteLoad(Id);
            return RedirectToAction("Index");
        }




    }
}