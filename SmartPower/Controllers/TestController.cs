using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SmartPower.Controllers.Domin;
using SmartPower.DataContext;

namespace SmartPower.Controllers
{
    public class TestController : Controller
    {

        private readonly PowerDbContext _con;

        public TestController(PowerDbContext con)
        {
            _con = con;
        }
        public IActionResult Index1()
        {          
            var Q= _con.SourceReading.OrderByDescending(s=>s.Id).Take(20).AsEnumerable();
            List<SourceReadingsWithCode> Data = new List<SourceReadingsWithCode>();

            foreach(var item in Q)
            {
                var Code = "xx";

                if(item.PrimarySourceId == null)
                {
                  Code =  _con.secondarySource.SingleOrDefault(s => s.Code == item.SecondarySourceId.ToString()).Code;
                }
                else
                {
                    Code = _con.PrimarySource.SingleOrDefault(s => s.Code == item.PrimarySourceId.ToString()).Code;

                }


                Data.Add(new SourceReadingsWithCode() {
                    Current1 = Convert.ToDecimal(item.Current1),
                    Current2 = item.Current2,
                    Current3 = item.Current3,
                    mCurrent1 = item.mCurrent1,
                    mCurrent2 = item.mCurrent2,
                    mCurrent3 = item.mCurrent3,
                    frequency1 = item.frequency1,
                    frequency2 = item.frequency2,
                    frequency3 = item.frequency3,
                    Power1 = item.Power1,
                    Power2 = item.Power2,
                    Power3 = item.Power3,
                    PowerFactor1 = item.PowerFactor1,
                    PowerFactor2 = item.PowerFactor2,
                    PowerFactor3 = item.PowerFactor3,
                    HarmonicOrder = item.HarmonicOrder,
                    HarmonicOrder1 = item.HarmonicOrder1,
                    HarmonicOrder2 = item.HarmonicOrder2,
                    HarmonicOrder3 = item.HarmonicOrder3,
                    ReturnCurrent = item.ReturnCurrent,
                    Voltage1 = item.Voltage1,
                    Voltage2 = item.Voltage2,
                    Voltage3 = item.Voltage3,
                    TimeStamp = item.TimeStamp,
                    Code = Code
                    
                });
            }

            return View(Data);
        }

        public IActionResult Index2()
        {
            _con.secondarySource.ToList();
            return View();
        }
    }
}