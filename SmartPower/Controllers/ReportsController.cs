using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SmartPower.DataContext;
using SmartPower.Services;
using SmartPower.Domin.Report;
using SmartPower.Domin;
using SmartPower.Domin.report;
using SmartPower.Models;
using System.Net.Http;

namespace SmartPower.Controllers
{
    public class ReportsController : Controller
    {
        private readonly PowerDbContext _Context;
        private readonly ReportService ReportService;

        public ReportsController(PowerDbContext _Context)
        {
            this._Context = _Context;
            ReportService = new ReportService(_Context);
        }

        public IActionResult Index()
        {
            var ViewModel = ReportService.AvgCalculaion();

            return View(ViewModel);
        }
        public IActionResult BalanceRetio()
        {
            var viewModel = ReportService.CalculateBalanceRetio();
            return View(viewModel);
        }
        public IActionResult SpikeLogs()
        {
            var viewModel = ReportService.getAllSpikes();
            return View(viewModel);

        }


        // ADEL
        public IActionResult LoadTimeDownRatio(DateTime from, DateTime to, int Id = -1)
        {
            ViewBag.load = _Context.Load;

            List<LoadTimeDownRatio> model = new List<LoadTimeDownRatio>();
            ReportService rs = new ReportService(_Context);
            if (Id == -1)
            {
                var loads = _Context.Load.ToList();
                foreach (var ld in loads)
                    model.Add(rs.LoadTimeDownRatio(ld.Id, from, to));
            }
            else
            {
                model.Add(rs.LoadTimeDownRatio(Id, from, to));
            }
            return View(model);
        }
        public IActionResult TransitTimeAnalysis(int N, DateTime day, int LoadId = -1)
        {
            ViewBag.load = _Context.Load;
            var model = new ReportService(_Context).TransitTimeAnalysis(LoadId, N, day);
            return View(model);
        }
        //ABDO
        public IActionResult powerpeak(int primId, int loadId, int type = -1) //type 1 -> prim , 2 -> load
        {
            ReportService rs = new ReportService(_Context);
            PrimarySourceSerivce ps = new PrimarySourceSerivce(_Context);
            ViewBag.Primary = ps.GetAllPrimarySources();
            LoadsServices ls = new LoadsServices(_Context);
            ViewBag.loads = ls.GetAllLoads();
            PowerPeakViewModel res = new PowerPeakViewModel();

            if (type == 1)
            {
                res = rs.PowerPeakBySourceID(primId, type);
            }
            else if (type == 2)
            {
                res = rs.PowerPeakBySourceID(loadId, type);

            }

            return View(res);
        }

        public IActionResult RushHour(DateTime date, int loadId = -1)
        {

            RushHourViewModel res = new RushHourViewModel();
            ReportService rs = new ReportService(_Context);
            LoadsServices ls = new LoadsServices(_Context);
            ViewBag.loads = ls.GetAllLoads();
            if (loadId != -1)
            {
                res = rs.RushHourForLoadById(loadId, date);
                ViewBag.LoadName = (ls.GetLoadById(loadId)).name;
            }
            return View(res);
        }

        public IActionResult HarmonicOrder(DateTime date, int HarmOrder = -1, int loadId = -1)
        {

            RushHourViewModel res = new RushHourViewModel();
            ReportService rs = new ReportService(_Context);
            LoadsServices ls = new LoadsServices(_Context);
            ViewBag.loads = ls.GetAllLoads();
            if (HarmOrder != -1)
            {
                res = rs.HaemonicOrderForLoadId(loadId, date, HarmOrder);
                ViewBag.LoadName = (ls.GetLoadById(loadId)).name;
            }
            return View(res);
        }

        public IActionResult Production(DateTime fromdate, DateTime todate, int Id = -1) // 1 -> for primaries
        {
            ReportService ps = new ReportService(_Context);
            FactoryService fs = new FactoryService(_Context);
            ViewBag.factories = fs.GetAllFactoriesSimple();
            List<ProductionViewModel> res = new List<ProductionViewModel>();
            if (Id != -1)
            {
                res = ps.ProductionReport(fromdate, todate, Id, 1);
            }
            return View(res);
        }

        public IActionResult EnergyConsumed()
        {
            FactoryService fs = new FactoryService(_Context);
            LoadsServices ls = new LoadsServices(_Context);
            ViewBag.factories = fs.GetAllFactoriesSimple();
            ViewBag.loads = ls.GetAllLoads();
            ViewBag.functions = ls.GetAllFunctions();
            return View();
        }


        public List<EnergyC> EnergyConsumedAjax(int id, int sort, string val, DateTime datefrom, DateTime dateto)
        {
            List<EnergyC> res = new List<EnergyC>();
            List<EnergyC> results = new List<EnergyC>();

            FactoryService fs = new FactoryService(_Context);

            LoadsServices ls = new LoadsServices(_Context);
            ReportService rs = new ReportService(_Context);

            var i = 0;
            ViewBag.functions = ls.GetAllFunctions();
            if (sort == 1)
            {
                while (datefrom != dateto.AddDays(1))
                {
                    res = rs.GetEnergies(id, sort, val, datefrom);
                    foreach (var item in res)
                    {
                        results.Add(item);
                    }

                    datefrom = datefrom.AddDays(1);
                }
            }
            else
            {
                while (datefrom != dateto.AddDays(1))
                {
                    var loads = _Context.Load.Where(l => l.Function == val).ToList();
                    foreach (var load in loads)
                    {
                        res = rs.GetEnergies(id, sort, Convert.ToString(load.Id), datefrom);

                        foreach (var item in res)
                        {
                            results.Add(item);
                        }
                        sort = 2;
                    }
                    datefrom = datefrom.AddDays(1);
                    i++;
                }


            }
            return results;
        }







        //if (Id != -1)
        //{
        //    res = ps.ProductionReport(fromdate, todate, Id,PrimOrSec);

        //}


        public IActionResult AveragePerDay()
        {
            FactoryService fs = new FactoryService(_Context);
            LoadsServices ls = new LoadsServices(_Context);
            ViewBag.factories = fs.GetAllFactoriesSimple();
            ViewBag.loads = ls.GetAllLoads();
            ViewBag.functions = ls.GetAllFunctions();
            return View();
        }
        /*

            sort equal sort by values :
            1 -> sort by load
            2-> sort by type


              if sortValue equal 1 val will be value of load

           if sortValue equal 2 val will be value of type
            type:  1-> current
          type:2->voltage
            type:3->powerFactor
 
        */


        public List<querySource> AveragePerDayfromajax(int id, int sort, string val, DateTime date)
        {
            FactoryService fs = new FactoryService(_Context);
            ViewBag.factories = fs.GetAllFactoriesSimple();
            ViewBag.factoryname = fs.GetFactoryNameById(id);
            ViewBag.date = date.ToString("yyyy/MM/dd hh");
            ReportService r = new ReportService(_Context);
            List<querySource> res = new List<querySource>();

            int i = 0;


            if (sort == 1)
            {
                var load = _Context.Load.FirstOrDefault(l => l.Id == Convert.ToInt32(val));
                SourceReading src;
                src = _Context.SourceReading.LastOrDefault(s => s.PrimarySourceId == load.SourceId && s.TimeStamp.Day == date.Day && s.Fac_Id == id);
                if (src != null)
                {
                    res.Add(new querySource());
                    r.GetAveragePerDay(id, date, ref res[i].CurrentAvg, ref res[i].HarmAvg, ref res[i].VoltageAvg, ref res[i].PowerFactorAvg, load.SourceId);
                    res[i].name = load.name;
                    res[i].sourceid = load.SourceId;
                    res[i].id = load.Id;
                    res[i].facid = id;
                    res[i].Timestamp = date;

                }

            }
            else
            {

                var loads = _Context.Load.Where(l => l.Function == val).ToList();





                foreach (var x in loads)
                {
                    SourceReading src;
                    if (x.SourceId % 2 == 0)
                    {

                        // src = _Context.SourceReading.Last(s => s.SecondarySourceId == x.SourceId && s.TimeStamp.Day == date.Day && s.Fac_Id == id);
                        src = _Context.SourceReading.LastOrDefault(s => s.SecondarySourceId == x.SourceId && s.TimeStamp.Day == date.Day && s.Fac_Id == id);

                    }

                    else
                    {
                        src = _Context.SourceReading.LastOrDefault(s => s.PrimarySourceId == x.SourceId && s.TimeStamp.Day == date.Day && s.Fac_Id == id);
                    }
                    if (src != null)
                    {
                        res.Add(new querySource());
                        r.GetAveragePerDay(id, date, ref res[i].CurrentAvg, ref res[i].HarmAvg, ref res[i].VoltageAvg, ref res[i].PowerFactorAvg, x.SourceId);
                        res[i].name = x.name;
                        res[i].sourceid = x.SourceId;
                        res[i].id = x.Id;
                        res[i].facid = id;
                        res[i].Timestamp = date;
                        i++;
                    }
                    else
                    {
                        continue;
                    }

                }
            }

            return res;
        }

        public List<SourceReading> ReadingsPerHour(int phsnumber, int SourceID, DateTime time, int hour, int facid)
        {
            ReportService rs = new ReportService(_Context);
            var res = rs.getReadingPerHour(phsnumber, SourceID, time, hour, facid);
            return res;
        }
        /*  [HttpGet]
          public IActionResult PeakPerDay()
          {
              FactoryService fs = new FactoryService(_Context);
              ViewBag.factories = fs.GetAllFactoriesSimple();
              PrimarySourceSerivce ps = new PrimarySourceSerivce(_Context);
              ViewBag.Primary = ps.GetAllPrimarySources();
              LoadsServices ls = new LoadsServices(_Context);
              ViewBag.Loads = ls.GetAllLoads();
              return View();
          }
          [HttpPost]*/
        public IActionResult PeakPerDay(DateTime date, int type = -1, int primId = -1, int loadId = -1)
        {
            FactoryService fs = new FactoryService(_Context);
            ViewBag.factories = fs.GetAllFactoriesSimple();
            PrimarySourceSerivce ps = new PrimarySourceSerivce(_Context);
            ViewBag.Primary = ps.GetAllPrimarySources();
            LoadsServices ls = new LoadsServices(_Context);
            ViewBag.Loads = ls.GetAllLoads();
            ReportService rs = new ReportService(_Context);
            PowerPeakViewModel res = new PowerPeakViewModel();
            if (type == 1)
            {
                res = rs.PowerPeakPerDay(date, type, primId);
            }
            else
            {
                res = rs.PowerPeakPerDay(date, type, loadId);

            }




            return View(res);
        }

        public IActionResult Instantaneous()
        {
            FactoryService fs = new FactoryService(_Context);
            LoadsServices ls = new LoadsServices(_Context);
            ViewBag.factories = fs.GetAllFactoriesSimple();
            ViewBag.loads = ls.GetAllLoads();

            return View();
        }


        public List<Instant> INSfromAjax(int id, int sort, int val, DateTime date)
        {

            ReportService rs = new ReportService(_Context);

            var model = rs.GetInstants(id, sort, val, date);

            return model;

        }

        public List<querySource> AverageFromTo(int id, int sort, string val, DateTime datefrom, DateTime dateto)
        {
            List<querySource> result = new List<querySource>();
            List<querySource> res = new List<querySource>();
            while (datefrom != dateto.AddDays(1))
            {
                res = AveragePerDayfromajax(id, sort, val, datefrom);
                foreach (var item in res)
                {
                    result.Add(item);
                }

                datefrom = datefrom.AddDays(1);
            }
            return result;
        }

        public IActionResult HarmonicStat()
        {
            FactoryService fs = new FactoryService(_Context);
            LoadsServices ls = new LoadsServices(_Context);
            ViewBag.factories = fs.GetAllFactoriesSimple();
            ViewBag.loads = ls.GetAllLoads();
            ViewBag.functions = ls.GetAllFunctions();
            return View();
        }
        public List<HarmSt> AverageHarmonicFromTo(int id, int sort, string val, DateTime datefrom, DateTime dateto, int harm)
        {
            ReportService rs = new ReportService(_Context);
            //  GetHarmSts(int id, int sort, string val, DateTime date, int harm)
            List<HarmSt> Results = new List<HarmSt>();
            List<HarmSt> Resu = new List<HarmSt>();
            var i = 0;
            if (sort == 1)
            {
                if (harm != -1)
                {
                    while (datefrom != dateto.AddDays(1))
                    {
                        Resu = rs.GetHarmSts(id, sort, val, datefrom, harm);
                        foreach (var item in Resu)
                        {
                            Results.Add(item);
                        }

                        datefrom = datefrom.AddDays(1);
                    }
                }
                else
                {
                    for (int k = 3; k < 14; k = k + 2)
                    {
                        while (datefrom != dateto.AddDays(1))
                        {
                            Resu = rs.GetHarmSts(id, sort, val, datefrom, k);
                            foreach (var item in Resu)
                            {
                                Results.Add(item);
                            }

                            datefrom = datefrom.AddDays(1);
                        }
                    }
                }

            }
            else
            {
                while (datefrom != dateto.AddDays(1))
                {
                    var loads = _Context.Load.Where(l => l.Function == val).ToList();
                    foreach (var load in loads)
                    {
                        if (harm != -1)
                        {
                            Resu = (rs.GetHarmSts(id, sort, Convert.ToString(load.Id), datefrom, harm));

                            Results.AddRange(Resu);
                        }
                        else
                        {
                            for (int k = 3; k < 14; k = k + 2)
                            {
                                Resu = (rs.GetHarmSts(id, sort, Convert.ToString(load.Id), datefrom, k));

                                Results.AddRange(Resu);
                            }
                        }
                    }
                    datefrom = datefrom.AddDays(1);
                    i++;
                }


            }

            var d = Results.Count;
            for (int x = 0; x < d; x++)
            {
                if (Results[x].cnt == 0)
                {
                    Results.RemoveAt(x);
                    x++;

                }
            }

            return Results;

        }

        //من اول هنا
        public IActionResult AveragePower()
        {
            return View();
        }

        public dynamic GetLoads()
        {
            return _Context.Load.Select(l => new { id = l.Id, l.name });
        }
        public dynamic GetFunctions()
        {
            return _Context.Functions.Select(l => new { id = l.Id, name = l.FunctionName });
        }

        [HttpPost]
        public IActionResult AverageLoadPowerToday([FromBody] dynamic data)
        {

            int LoadId = data.id;
            decimal p1avg=0, p2avg=0, p3avg = 0;
            DateTime FromDate = Convert.ToDateTime(((string)data.fromdate)).Date;
            DateTime ToDate = Convert.ToDateTime(((string)data.todate)).Date;
            
            Load load = _Context.Load.FirstOrDefault(l => l.Id == LoadId);
            
            List<SourceReading> reads = _Context.SourceReading.Where(r => r.PrimarySourceId == load.Id && (r.TimeStamp.Date>= FromDate && r.TimeStamp.Date <= ToDate) && r.Power1 != 0 && r.Power2 != 0 && r.Power3 !=0 ).ToList();
            if (reads.Any())
            {
                p1avg = reads.Average(r => r.Power1);
                p2avg = reads.Average(r => r.Power2);
                p3avg = reads.Average(r => r.Power3);
                return Ok(new
                {
                    LoadName = load.name,
                    FromDate = FromDate.ToShortDateString(),
                    ToDate = ToDate.ToShortDateString(),
                    LoadPower1Average = Math.Round(p1avg,3),
                    LoadPower2Average = Math.Round(p2avg,3),
                    LoadPower3Average = Math.Round(p3avg,3),
                    LoadPowerAverage = Math.Round((p1avg + p2avg + p3avg) / 3,3)
                });
            }
            else
            {
                return Ok(new
                {
                    LoadName = "No Readings Exist For this Load"
                });
            }
            

           
        }
        [HttpPost]
        public IActionResult AverageFunctionPowerToday([FromBody] dynamic data)
        {
            decimal totalAveragePower = 0;
            decimal p1avgAll = 0, p2avgAll = 0, p3avgAll = 0;
            int FunctionId = data.id;
            DateTime FromDate = Convert.ToDateTime(((string)data.fromdate)).Date;
            DateTime ToDate = Convert.ToDateTime(((string)data.todate)).Date;
            Function function = _Context.Functions.FirstOrDefault(f => f.Id == FunctionId);
            var loadsIds = _Context.Load.Where(l => l.Function == function.FunctionName).Select(l => l.Id).ToList();
            List<SourceReading> readings = new List<SourceReading>();
            loadsIds.ForEach((id) =>
            {
                List<SourceReading> reads = _Context.SourceReading.Where(r => r.PrimarySourceId == id && (r.TimeStamp.Date >= FromDate && r.TimeStamp.Date <= ToDate) && r.Power1 != 0 && r.Power2 != 0 && r.Power3 !=0 ).ToList();
                if (reads.Any())
                {
                    decimal p1a = reads.Average(r => r.Power1);
                    decimal p2a = reads.Average(r => r.Power2);
                    decimal p3a = reads.Average(r => r.Power3);
                    decimal pa = (p1a + p2a + p3a) / 3;
                    p1avgAll += p1a;
                    p2avgAll += p2a;
                    p3avgAll += p3a;
                    totalAveragePower += pa;
                }
                
            });

            return Ok(new
            {
                Name = function.FunctionName,
                FromDate = FromDate.ToShortDateString(),
                ToDate = ToDate.ToShortDateString(),
                o1 = Math.Round(p1avgAll,3),
                p2 = Math.Round(p2avgAll,3),
                p3 = Math.Round(p3avgAll,3),
                pa = Math.Round(totalAveragePower,3)
            });
        }

        public IActionResult test([FromQuery] int id)
        {
            Function function = _Context.Functions.FirstOrDefault(f => f.Id == id);
            return Ok(_Context.Load.Where(l=>l.Function == function.FunctionName));
        }




        //الجديد

    /*    public IActionResult EnergySum()
        {
            var loads = _Context.Load.Select(l =>
            new {
                    l.name,
                    l.Function
            }).ToList();

            return Ok(new { MyLoads = loads});
        }*/

        public IActionResult SumEnergy()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SumLoadEnergyToday([FromBody] dynamic data)
        {

            int LoadId = data.id;
            decimal p1Summ = 0, p2Summ = 0, p3Summ = 0;
            DateTime FromDate = Convert.ToDateTime(((string)data.fromdate)).Date;
            DateTime ToDate = Convert.ToDateTime(((string)data.todate)).Date;

            Load load = _Context.Load.FirstOrDefault(l => l.Id == LoadId);

            List<SourceReading> reads = _Context.SourceReading.Where(r => r.PrimarySourceId == load.Id && (r.TimeStamp.Date >= FromDate && r.TimeStamp.Date <= ToDate) && r.Power1 != 0 && r.Power2 != 0 && r.Power3 != 0).ToList();
           if (reads.Any())
            {
                decimal p1sum = reads.Aggregate(reads.First().Power1, (acc, x) => acc + x.Power1 / 120);
                decimal p2sum = reads.Aggregate(reads.First().Power1, (acc, x) => acc + x.Power2 / 120);
                decimal p3sum = reads.Aggregate(reads.First().Power1, (acc, x) => acc + x.Power3 / 120);
                return Ok(new
                {
                    LoadName = load.name,
                    FromDate = FromDate.ToShortDateString(),
                    ToDate = ToDate.ToShortDateString(),
                    LoadEnergy1Sum = Math.Round(p1sum, 3),
                    LoadEnergy2Sum = Math.Round(p2sum, 3),
                    LoadEnergy3Sum = Math.Round(p3sum, 3),
                    LoadEnergySum = Math.Round((p1sum + p2sum + p3sum) , 3)
                });
            }
            else
            {
                return Ok(new
                {
                    LoadName = "No Readings Exist For this Load"
                });
            }
            

        }





        [HttpPost]
        public IActionResult SumFunctionEnergyToday([FromBody] dynamic data)
        {
            decimal totalsumenergy = 0;
            decimal p1sumAll = 0, p2sumAll = 0, p3sumAll = 0;
            int FunctionId = data.id;
            DateTime FromDate = Convert.ToDateTime(((string)data.fromdate)).Date;
            DateTime ToDate = Convert.ToDateTime(((string)data.todate)).Date;
            Function function = _Context.Functions.FirstOrDefault(f => f.Id == FunctionId);
            var loadsIds = _Context.Load.Where(l => l.Function == function.FunctionName).Select(l => l.Id).ToList();
            List<SourceReading> readings = new List<SourceReading>();
            loadsIds.ForEach((id) =>
            {
                List<SourceReading> reads = _Context.SourceReading.Where(r => r.PrimarySourceId == id && (r.TimeStamp.Date >= FromDate && r.TimeStamp.Date <= ToDate) && r.Power1 != 0 && r.Power2 != 0 && r.Power3 != 0).ToList();
                if (reads.Any())
                {
                    decimal p1s = reads.Aggregate(reads.First().Power1, (acc, x) => acc + x.Power1 / 120);
                    decimal p2s = reads.Aggregate(reads.First().Power1, (acc, x) => acc + x.Power2 / 120);
                    decimal p3s = reads.Aggregate(reads.First().Power1, (acc, x) => acc + x.Power3 / 120);
                    decimal ps = (p1s + p2s + p3s);
                    p1sumAll += p1s;
                    p2sumAll += p2s;
                    p3sumAll += p3s;
                    totalsumenergy += ps;
                }
               
            });


            return Ok(new
            {
                Name = function.FunctionName,
                FromDate = FromDate.ToShortDateString(),
                ToDate = ToDate.ToShortDateString(),
                o1 = Math.Round(p1sumAll, 3),
                p2 = Math.Round(p2sumAll, 3),
                p3 = Math.Round(p3sumAll, 3),
                ps = Math.Round(totalsumenergy, 3)
            });
            

        }

     /*   public IActionResult EnergyWatt([FromQuery] int id)
        {
            List<SourceReading> reads = _Context.SourceReading.Where(r => r.PrimarySourceId == load.Id && (r.TimeStamp.Date >= FromDate && r.TimeStamp.Date <= ToDate) && r.Power1 != 0 && r.Power2 != 0 && r.Power3 != 0).ToList();
            decimal result = reads.Aggregate(reads.First().Power1, (acc, x) => acc + x.Power1/ 120);
            return Ok(result);
        }*/



    }
}







