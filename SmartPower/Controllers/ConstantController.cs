using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SmartPower.DataContext;
using SmartPower.Models;
using SmartPower.Services;

namespace SmartPower.Controllers
{
    public class ConstantController : Controller
    {
        private readonly PowerDbContext _Context;

        public ConstantController(PowerDbContext _Context)
        {
            this._Context = _Context;
        }
        public IActionResult Index()
        {
            ReportConstantServices Rs = new ReportConstantServices(_Context);
            var viewModel = Rs.GetAllReportConst();
            return View(viewModel);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(ReportConstant obj)
        {
            ReportConstantServices Rs = new ReportConstantServices(_Context);
            await Rs.CreateAsync(obj);

            return RedirectToAction(nameof(Index));
        }
        public IActionResult Delete(int Id)
        {
            ReportConstantServices Rs = new ReportConstantServices(_Context);
            Rs.Delete(Id);
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public IActionResult Edit(int Id)
        {
            ReportConstantServices Rs = new ReportConstantServices(_Context);
            var viewModel = Rs.GetReportConstant(Id);
            return View(viewModel);
        }
        [HttpPost]
        public IActionResult Edit(ReportConstant obj)
        {
            ReportConstantServices Rs = new ReportConstantServices(_Context);
            var viewModel = Rs.Edit(obj);
            return RedirectToAction(nameof(Index));
        }
    }
}