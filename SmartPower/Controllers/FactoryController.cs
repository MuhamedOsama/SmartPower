    using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SmartPower.Controllers.Domin;
using SmartPower.DataContext;
using SmartPower.Services;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace SmartPower.Controllers
{
    public class FactoryController : Controller
    {
        private readonly PowerDbContext _con;
        public FactoryController(PowerDbContext con)
        {
            _con = con;
        }
        public IActionResult Index()
        {
            FactoryService fs = new FactoryService(_con);
            ViewBag.factories = fs.GetAllFactoriesSimple();
            return View();
        }
        [HttpGet]
        public IActionResult Create()
        {
            FactoryService fs = new FactoryService(_con);
            ViewBag.Business = fs.GetallBusinessType(); 
            return View();
        }
        [HttpPost]
        public async Task <IActionResult> Create(FactoryDataModel obj)
        {
            FactoryService fs = new FactoryService(_con);         
            var y = fs.GetFactoryByName(obj.Name);
            if (y != null)
            {
                ViewBag.Business = fs.GetallBusinessType();
                ModelState.AddModelError("Name", "Name is already exist");
                return View(obj);
            }
            bool x =await fs.CreateFactory(obj);
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int Id)
        {
            FactoryService fs = new FactoryService(_con);
            bool x = await fs.DeleteFactoryAsync(Id);
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public IActionResult Edit(int Id)
        {
            FactoryService fs = new FactoryService(_con);
            var Factory =  fs.GetFactoryById(Id);
            return View(Factory);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(FactoryDataModel obj)
        {
            FactoryService fs = new FactoryService(_con);
            bool FactoryEdit = await fs.EditFactoryAsync(obj);

            return RedirectToAction(nameof(Index));
        }
        public  IActionResult Details()
        {
            FactoryService fs = new FactoryService(_con);
            ViewBag.factories =  fs.GetAllFactoriesSimple();  
            return View();
        }
    }
}