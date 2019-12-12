using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SmartPower.Models;
using SmartPower.Services;
using SmartPower.DataContext;
using SmartPower.Controllers.Domin;

namespace SmartPower.Controllers
{
    public class PhaseConnectionController : Controller
    {



        private readonly PowerDbContext _con;

        public PhaseConnectionController(PowerDbContext con)
        {
            _con = con;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public List<PrimarySource> GetValidPrimarySource(string type , int factoryid)  //type of distination 
        {

            List<PrimarySource> x = new PhasesConnectionService(_con).GetValidPrimarySource(type, factoryid);
            return x;
        }
        public List<PrimarySource> GetprimarySourcebyfacid(int factoryid)  //type of distination 
        {

            List<PrimarySource> x = _con.PrimarySource.Where(s => s.FactoryId == factoryid).ToList(); 
            return x;
        }
        public  List<secondarySource> GetValidsecondrySource(string type, int factoryid)
        {

            List<secondarySource> x = new PhasesConnectionService(_con).GetValidsecondrySource(type ,factoryid);
            return x;
        }
        public validNodes GetValidNodes(int id) // from ajax id = sourceid 
        {
            validNodes x = new PhasesConnectionService(_con).GetValidNodes(id);
            return x;
        }

        //[HttpGet]
        //public IActionResult Create(int sourceid ,  int destid)
        //{
        //    PhasesConnection pc = new PhasesConnection
        //    {
        //         SourceID = sourceid,  
        //         DestinationID =  destid
        //    };
        //    return View(pc);  
        //}

        //}


    }
}