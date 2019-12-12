using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SmartPower.DataContext;
using SmartPower.Services;
using SmartPower.Domin;

namespace SmartPower.Controllers
{
    public class ProductionController : Controller
    {
        private readonly PowerDbContext _Context;
        public ProductionController(PowerDbContext _context)
        {
            _Context = _context;
        }

        public IActionResult Index(int Id = -1)
        {
            ProductionService ps = new ProductionService(_Context);
            FactoryService fs = new FactoryService(_Context);
            ViewBag.factories = fs.GetAllFactoriesSimple();
            List<ProductionViewModel> res = new List<ProductionViewModel>();
            if (Id != -1)
            {
                res = ps.GetallProduction(Id);
            }
            return View(res);
        }
        [HttpGet]
        public IActionResult Create()
        {
            FactoryService fs = new FactoryService(_Context);
            ViewBag.factories = fs.GetAllFactoriesSimple();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(ProductionViewModel obj)
        {
            ProductionService ps = new ProductionService(_Context);
            bool t = await ps.CreatProductionAsync(obj);
            //return RedirectToAction("Index");
            return RedirectToAction("Index", new { id = obj.FacId });
        }
        public List<char> GetallDatesOroductionWithFacid(int facid)
        {
            ProductionService ps = new ProductionService(_Context);
            var res = ps.GetallDatesOroductionWithFacid(facid);
            return res;
        }
    }
}