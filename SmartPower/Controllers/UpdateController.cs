using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;
using System.Data.SqlClient;
using System.Data;
using SmartPower.DataContext;
using SmartPower.Models;
using SmartPower.Domin;
using SmartPower.Models.report;
using Microsoft.EntityFrameworkCore;

namespace SmartPower.Controllers
{
    [Produces("application/json")]
    [Route("api/[action]")]
    public class UpdateController : Controller
    {
        private IConfiguration _Configuration;
        public readonly PowerDbContext Context;
        private IMemoryCache _MemoryCache { get; }
        public UpdateController(PowerDbContext context, IMemoryCache MemoryCache, IConfiguration Configuration)
        {
            _Configuration = Configuration;
            Context = context;
            this._MemoryCache = MemoryCache;
            threshold = Context.ReportConstants.FirstOrDefault(r => r.Name == "threshold").Value;
        }

        static List<decimal> CurrentValuesList = new List<decimal>();
        static List<decimal> CurrentValues2List = new List<decimal>();
        static List<decimal> CurrentValues3List = new List<decimal>();
        static int counter = 0;
        static decimal CurrentValue = 0;
        static decimal CurrentValue2 = 0;
        static decimal CurrentValue3 = 0;
        decimal threshold;
        long timevalue;

        //Define Route [Primary Source -- Secoundry Source ]
        //Calculate Avg And insert It 
        // api
        ///api/source?pid=12&pv1=0.0&pv2=0.0&pv3=0.0&pI1=0.0&pI2=0.0&pI3=0.0&ppf1=0.0&ppf2=0.0&ppf3=0.0&pfreq1=0.0&pfreq2=0.0&pfreq3=0.0&pp1=0.0&pp2=0.0&pp3=0.0&pho=0.0&pIo=0.0&ptime=156777789877



        public async Task<string> DoProcess(querySource query)
        {
            if (Context.PrimarySource.FirstOrDefault(n => n.Code == query.code) != null)
            {
                var line = Context.PrimarySource.Single(n => n.Code == query.code);

                if(query.PowerFactor1 == -1)
                {
                    query.PowerFactor1 = 0; 
                }
                if (query.PowerFactor2 == -1)
                {
                    query.PowerFactor2 = 0;
                }
                if (query.PowerFactor3 == -1)
                {
                    query.PowerFactor3 = 0;
                }
                if (query.frequency1 == -1)
                {
                    query.frequency1 = 0;
                }
                if (query.frequency2 == -1)
                {
                    query.frequency2 = 0;
                }
                if (query.frequency3 == -1)
                {
                    query.frequency3 = 0;
                }
             
                var Model = new SourceReading()
                {
                    Fac_Id = line.FactoryId,
                    
                    Voltage1 = query.Voltage1,
                    Current1 = query.Current1,
                    PowerFactor1 = query.PowerFactor1,
                    frequency1 = query.frequency1,
                    Power1 = query.Power1,

                    Voltage2 = query.Voltage2,
                    Current2 = query.Current2,
                    PowerFactor2 = query.PowerFactor2,
                    frequency2 = query.frequency2,
                    Power2 = query.Power2,

                    Voltage3 = query.Voltage3,
                    Current3 = query.Current3,
                    PowerFactor3 = query.PowerFactor3,
                    frequency3 = query.frequency3,
                    Power3 = query.Power3,


                    mCurrent1 = query.mCurrent1,
                    mCurrent2 = query.mCurrent2,
                    mCurrent3 = query.mCurrent3,

                    HarmonicOrder = query.HarmonicOrder,
                    HarmonicOrder1 =query.HarmonicOrder1,
                    HarmonicOrder2 =query.HarmonicOrder2,
                    HarmonicOrder3 =query.HarmonicOrder3,
                    ReturnCurrent = query.ReturnCurrent,
                    PrimarySourceId = Convert.ToInt32(line.Code),
                    SecondarySourceId = null,
                    TimeStamp = query.Timestamp
                };

                //avg calc by yasser
                CurrentValuesList.Add(query.Current1);
                CurrentValues2List.Add(query.Current2);
                CurrentValues3List.Add(query.Current3);
                if (CurrentValuesList.Count() == 10)
                {
                    var avg1 = CurrentValuesList.Average();
                    var avg2 = CurrentValues2List.Average();
                    var avg3 = CurrentValues3List.Average();
                    //
                    var avg = new SourceAvg()
                    {
                        Current1Avg = avg1,
                        Time = DateTime.Now,
                        PrimarySourceId = line.Id,
                        SecondarySourceId = null,
                        Current2Avg = avg2,
                        Current3Avg = avg3
                    };
                    //save into Database
                    await Context.SourceAvgs.AddAsync(avg);
                    await Context.SaveChangesAsync();
                    //clear list
                    CurrentValuesList = new List<decimal>();
                    CurrentValues2List = new List<decimal>();
                    CurrentValues3List = new List<decimal>();
                }
                Context.SourceReading.Add(Model);
                await Context.SaveChangesAsync();

                

                // peak calc 
                peakPower(Model);

                return $"Ok," + DateTimeOffset.UtcNow.ToUnixTimeSeconds();


            }
            else if (Context.secondarySource.FirstOrDefault(n => n.Code == query.code) != null)
            {
                var line = Context.secondarySource.Single(n => n.Code == query.code);
                if (query.PowerFactor1 == -1)
                {
                    query.PowerFactor1 = 0;
                }
                if (query.PowerFactor2 == -1)
                {
                    query.PowerFactor2 = 0;
                }
                if (query.PowerFactor3 == -1)
                {
                    query.PowerFactor3 = 0;
                }
                if (query.frequency1 == -1)
                {
                    query.frequency1 = 0;
                }
                if (query.frequency2 == -1)
                {
                    query.frequency2 = 0;
                }
                if (query.frequency3 == -1)
                {
                    query.frequency3 = 0;
                }
                var Model = new SourceReading()
                {
                    Fac_Id = line.Fac_Id,

                    Voltage1 = query.Voltage1,
                    Current1 = query.Current1,
                    PowerFactor1 = query.PowerFactor1,
                    frequency1 = query.frequency1,
                    Power1 = query.Power1,
                    Voltage2 = query.Voltage2,
                    Current2 = query.Current2,
                    PowerFactor2 = query.PowerFactor2,
                    frequency2 = query.frequency2,
                    Power2 = query.Power2,
                    Voltage3 = query.Voltage3,
                    Current3 = query.Current3,
                    PowerFactor3 = query.PowerFactor3,
                    frequency3 = query.frequency3,
                    Power3 = query.Power3,
                    ReturnCurrent = query.ReturnCurrent,
                    PrimarySourceId = null,
                    SecondarySourceId = Convert.ToInt32(line.Code),
                    TimeStamp = query.Timestamp,
                    mCurrent1 = query.mCurrent1,
                    mCurrent2 = query.mCurrent2,
                    mCurrent3 = query.mCurrent3,
                    HarmonicOrder = query.HarmonicOrder,
                    HarmonicOrder1 = query.HarmonicOrder1,
                    HarmonicOrder2 = query.HarmonicOrder2,
                    HarmonicOrder3 = query.HarmonicOrder3,
                };

                CurrentValuesList.Add(query.Current1);
                CurrentValues2List.Add(query.Current2);
                CurrentValues3List.Add(query.Current3);
                if (CurrentValuesList.Count() == 10)
                {
                    var avg1 = CurrentValuesList.Average();
                    var avg2 = CurrentValues2List.Average();
                    var avg3 = CurrentValues3List.Average();
                    // adding to avg tbl
                    var avg = new SourceAvg()
                    {
                        Current1Avg = avg1,
                        Time = DateTime.Now,
                        PrimarySourceId = null,
                        SecondarySourceId = Convert.ToInt32(line.Code),
                        Current2Avg = avg2,
                        Current3Avg = avg3
                    };
                    //save into Database
                    await Context.SourceAvgs.AddAsync(avg);
                    await Context.SaveChangesAsync();
                    //clear list
                    CurrentValuesList = new List<decimal>();
                    CurrentValues2List = new List<decimal>();
                    CurrentValues3List = new List<decimal>();

                }
                Context.SourceReading.Add(Model);
                await Context.SaveChangesAsync();

                peakPower(Model);

                return $"Ok," + DateTimeOffset.UtcNow.ToUnixTimeSeconds();

                // peak calc 



            }
            else
            {
                return "worng format or missing Code";
            }
        }

        public string test()
        {
            var reading = new PowerAvg()
            {
                P1 = 120,//model.Power1,
            P2 = 120, //model.Power2,
            P3 = 120, //model.Power3,
            primarySourceId = 5, //model.PrimarySourceId,
            secondrySourceId = null, // model.SecondarySourceId,
            readingDate = DateTime.Now //model.TimeStamp
        };
            Context.PowerAvg.Add(reading);
            Context.SaveChanges();

            return $"Ok,";

        }

        //read
        [HttpGet]
        public async Task<string> source()
        {
            var queryString = HttpContext.Request.Query;

            if (counter == 0)
            {
                //first Request
                counter++;
                var values = queryString.Where(o => o.Key.StartsWith("x")).ToDictionary(o => o.Key);

                var data = new querySource();

                if (values["xt"].Value == "-1")
                {
                    data.Timestamp = DateTime.Now;
                }
                else
                {
                    if (Int64.TryParse(values["xt"].Value, out timevalue))
                    {
                        DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(timevalue);
                        data.Timestamp = dateTimeOffset.LocalDateTime;
                    }
                }               
                data.code = values["xid"].Value;
                data.Voltage1 = Convert.ToDecimal(values["xv1"].Value);
                data.Voltage2 = Convert.ToDecimal(values["xv2"].Value);
                data.Voltage3 = Convert.ToDecimal(values["xv3"].Value);
                data.Current1 = Convert.ToDecimal(values["xi1"].Value);
                data.Current2 = Convert.ToDecimal(values["xi2"].Value);
                data.Current3 = Convert.ToDecimal(values["xi3"].Value);
                data.mCurrent1 = Convert.ToDecimal(values["xii1"].Value);
                data.mCurrent2 = Convert.ToDecimal(values["xii2"].Value);
                data.mCurrent3 = Convert.ToDecimal(values["xii3"].Value);
                data.PowerFactor1 = Convert.ToDecimal(values["xpf1"].Value);
                data.PowerFactor2 = Convert.ToDecimal(values["xpf2"].Value);
                data.PowerFactor3 = Convert.ToDecimal(values["xpf3"].Value);
                data.frequency1 = Convert.ToDecimal(values["xf1"].Value);
                data.frequency2 = Convert.ToDecimal(values["xf2"].Value);
                data.frequency3 = Convert.ToDecimal(values["xf3"].Value);
                data.Power1 = Convert.ToDecimal(values["xp1"].Value);
                data.Power2 = Convert.ToDecimal(values["xp2"].Value);
                data.Power3 = Convert.ToDecimal(values["xp3"].Value);
                data.HarmonicOrder = Convert.ToDecimal(values["xho"].Value);
                data.HarmonicOrder1 = Convert.ToDecimal(values["xho1"].Value);
                data.HarmonicOrder2 = Convert.ToDecimal(values["xho2"].Value);
                data.HarmonicOrder3 = Convert.ToDecimal(values["xho3"].Value);

                data.ReturnCurrent = Convert.ToDecimal(values["xi0"].Value);


                //past Value
                CurrentValue = Convert.ToDecimal(values["xi1"].Value);
                CurrentValue2 = Convert.ToDecimal(values["xi2"].Value);
                CurrentValue3 = Convert.ToDecimal(values["xi3"].Value);

                var result = await DoProcess(data);
                //return await result;
                return $"Ok," + DateTimeOffset.UtcNow.ToUnixTimeSeconds();


            }
            else
            {
                //Secound Request
                counter = 0;
                var values = queryString.Where(o => o.Key.StartsWith("x")).ToDictionary(o => o.Key);
                var data = new querySource();
            


                if (values["xt"].Value == "-1")
                {
                    data.Timestamp = DateTime.Now;
                }
                else
                {
                    if (Int64.TryParse(values["xt"].Value, out timevalue))
                    {
                        DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(timevalue);
                        data.Timestamp = dateTimeOffset.LocalDateTime;
                    }
                }

                data.code = values["xid"].Value;
                data.Voltage1 = Convert.ToDecimal(values["xv1"].Value);
                data.Voltage2 = Convert.ToDecimal(values["xv2"].Value);
                data.Voltage3 = Convert.ToDecimal(values["xv3"].Value);
                data.Current1 = Convert.ToDecimal(values["xi1"].Value);
                data.Current2 = Convert.ToDecimal(values["xi2"].Value);
                data.Current3 = Convert.ToDecimal(values["xi3"].Value);
                data.mCurrent1 = Convert.ToDecimal(values["xii1"].Value);
                data.mCurrent2 = Convert.ToDecimal(values["xii2"].Value);
                data.mCurrent3 = Convert.ToDecimal(values["xii3"].Value);
                data.PowerFactor1 = Convert.ToDecimal(values["xpf1"].Value);
                data.PowerFactor2 = Convert.ToDecimal(values["xpf2"].Value);
                data.PowerFactor3 = Convert.ToDecimal(values["xpf3"].Value);
                data.frequency1 = Convert.ToDecimal(values["xf1"].Value);
                data.frequency2 = Convert.ToDecimal(values["xf2"].Value);
                data.frequency3 = Convert.ToDecimal(values["xf3"].Value);
                data.Power1 = Convert.ToDecimal(values["xp1"].Value);
                data.Power2 = Convert.ToDecimal(values["xp2"].Value);
                data.Power3 = Convert.ToDecimal(values["xp3"].Value);
                data.HarmonicOrder = Convert.ToDecimal(values["xho"].Value);
                data.HarmonicOrder1 = Convert.ToDecimal(values["xho1"].Value);
                data.HarmonicOrder2 = Convert.ToDecimal(values["xho2"].Value);
                data.HarmonicOrder3 = Convert.ToDecimal(values["xho3"].Value);
                data.ReturnCurrent = Convert.ToDecimal(values["xi0"].Value);


                var newCurrent = Convert.ToDecimal(values["xi1"].Value);
                var newCurrent2 = Convert.ToDecimal(values["xi2"].Value);
                var newCurrent3 = Convert.ToDecimal(values["xi3"].Value);
                // abs values For [new values - old values ]
                var currentSub = CurrentValue - newCurrent;
                var currentSub2 = CurrentValue2 - newCurrent2;
                var currentSub3 = CurrentValue3 - newCurrent3;
                // abs 
                currentSub = Math.Abs(currentSub);
                currentSub2 = Math.Abs(currentSub2);
                currentSub3 = Math.Abs(currentSub3);
                //Detect Spike
                if (currentSub >= threshold || currentSub2 >= threshold || currentSub3 >= threshold)
                {
                    var lineId = 0;
                    var code = values["xid"].Value;
                    if ( Context.secondarySource.FirstOrDefault(n => n.Code == code) != null )
                    {
                        lineId = Context.secondarySource.Single(n => n.Code == code).Id;
                    }
                    else
                    {
                        lineId = Context.PrimarySource.Single(n => n.Code == code).Id;
                    }

                    if (currentSub >= threshold)
                    {
                        var Spikelogs = new CurrentSpikeLogs()
                        {
                            Value = currentSub,
                            Time = DateTime.Now,
                            //SourceId = lineId,
                            PhaseName = "I1", 
                            SourceCode = code,
                        };
                        await Context.CurrentSpikeLogs.AddAsync(Spikelogs);
                        await Context.SaveChangesAsync();

                    }
                    if (currentSub2 >= threshold)
                    {


                        var Spikelogs = new CurrentSpikeLogs()
                        {
                            Value = currentSub2,
                            Time = DateTime.Now,
                            //SourceId = lineId,
                            PhaseName = "I2",
                            SourceCode = code

                        };
                        await Context.CurrentSpikeLogs.AddAsync(Spikelogs);
                        await Context.SaveChangesAsync();

                    }
                    if (currentSub3 >= threshold)
                    {

                        var Spikelogs = new CurrentSpikeLogs()
                        {
                            Value = currentSub3,
                            Time = DateTime.Now,
                          //  SourceId = lineId,
                            PhaseName = "I3",
                            SourceCode = code

                        };
                        await Context.CurrentSpikeLogs.AddAsync(Spikelogs);
                        await Context.SaveChangesAsync();

                    }
                    CurrentValue = 0;
                    CurrentValue2 = 0;
                    CurrentValue3 = 0;

                    var result = await DoProcess(data);

                     //return await result;
                    return  $"Ok," + DateTimeOffset.UtcNow.ToUnixTimeSeconds();


                }
                else
                {
                    var result =await DoProcess(data);
                   // return await result;
                    return  $"Ok," + DateTimeOffset.UtcNow.ToUnixTimeSeconds();

                }

            }
        }
        [HttpGet]
        public string load()
        {
            var queryString = HttpContext.Request.Query;
            var values = queryString.Where(o => o.Key.StartsWith("x")).ToDictionary(o => o.Key);
            
            int code = Convert.ToInt32(values["xid"].Value);

            if (Context.Load.Single(n => n.code == code) != null)
            {
                
                var line = Context.Load.Single(n => n.code == code);
                var Model = new LoadReading()
                {
                    Vibration = Convert.ToDecimal(values["xvib"].Value),
                    Speed = Convert.ToDecimal(values["xs"].Value),
                    Temperature = Convert.ToDecimal(values["xtemp"].Value),

                    LoadId = line.Id,
                    TimeStamp = DateTime.Now
                   
                };
               
                Context.LoadReading.Add(Model);
                Context.SaveChanges();

                return $"Ok,"+ DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            }

            else
            {
                return "worng format or missing Code";
            }
        }

        public void peakPower(SourceReading model)
        {
            var count = Context.PowerAvg.ToList();

            if (count.Count() != 0)
            {


                PowerAvg firstModel = Context.PowerAvg.First();


                if (firstModel != null && firstModel.readingDate.AddHours(1) < model.TimeStamp)
                {

                    var primaries = Context.PrimarySource.ToList();
                    var secondries = Context.secondarySource.ToList();
                    var avgs = Context.PowerAvg.ToList();

                    foreach (var primary in primaries)
                    {

                        decimal totalp1 = 0;
                        decimal totalp2 = 0;
                        decimal totalp3 = 0;
                        decimal avgp1 = 0;
                        decimal avgp2 = 0;
                        decimal avgp3 = 0;

                        
                        var pavg = count.Where(a => a.primarySourceId == Convert.ToInt32(primary.Code)).ToList();

                        if (pavg.Count != 0)
                        {
                            foreach (var avg in pavg)
                            {
                                totalp1 += avg.P1;
                                totalp2 += avg.P2;
                                totalp3 += avg.P3;
                            }
                            avgp1 = (totalp1) / (pavg.Count);
                            avgp2 = (totalp2) / (pavg.Count);
                            avgp3 = (totalp3) / (pavg.Count);

                            var peak = Context.powerPeak.DefaultIfEmpty().FirstOrDefault(p => p.primarySourceId == Convert.ToInt32(primary.Code));
                            if(peak != null) {
                            if (peak.peakP1 < avgp1 || peak.peakP1 == avgp1)
                            {
                                peak.peakP1 = avgp1;
                                peak.dateP1 = firstModel.readingDate;
                            }
                            if (peak.peakP2 < avgp2 || peak.peakP2 == avgp2)
                            {
                                peak.peakP2 = avgp2;
                                peak.dateP2 = firstModel.readingDate;

                            }
                            if (peak.peakP3 < avgp3 || peak.peakP3 == avgp3)
                            {
                                peak.peakP3 = avgp3;
                                peak.dateP3 = firstModel.readingDate;

                            }
                            Context.powerPeak.Update(peak);
                            Context.SaveChanges();
                            }
                        }





                    }

                    foreach (var secondry in secondries)
                    {
                        decimal totalp1 = 0;
                        decimal totalp2 = 0;
                        decimal totalp3 = 0;
                        decimal avgp1 = 0;
                        decimal avgp2 = 0;
                        decimal avgp3 = 0;


                        var savg = avgs.Where(a => a.secondrySourceId == Convert.ToInt32(secondry.Code)).ToList();
                        if (savg.Count != 0)
                        {
                            foreach (var avg in savg)
                            {
                                totalp1 += avg.P1;
                                totalp2 += avg.P2;
                                totalp3 += avg.P3;
                            }

                            avgp1 = (totalp1) / (savg.Count);
                            avgp2 = (totalp2) / (savg.Count);
                            avgp3 = (totalp3) / (savg.Count);

                            var peak = Context.powerPeak.FirstOrDefault(p => p.secondrySourceId == Convert.ToInt32(secondry.Code));
                            if (peak.peakP1 < avgp1 || peak.peakP1 == avgp1)
                            {
                                peak.peakP1 = avgp1;
                                peak.dateP1 = firstModel.readingDate;
                            }
                            if (peak.peakP2 < avgp2 || peak.peakP2 == avgp2)
                            {
                                peak.peakP2 = avgp2;
                                peak.dateP2 = firstModel.readingDate;

                            }
                            if (peak.peakP3 < avgp3 || peak.peakP3 == avgp3)
                            {
                                peak.peakP3 = avgp3;
                                peak.dateP3 = firstModel.readingDate;
                            }
                            Context.powerPeak.Update(peak);
                            Context.SaveChanges();
                        }
                    }
                    // clear table 
                    Context.Database.ExecuteSqlCommand("Delete From PowerAvg");

                    var reading = new PowerAvg()
                    {
                        P1 = model.Power1,
                        P2 = model.Power2,
                        P3 = model.Power3,
                        primarySourceId = model.PrimarySourceId,
                        secondrySourceId = model.SecondarySourceId,
                        readingDate = model.TimeStamp
                    };
                    Context.PowerAvg.Add(reading);
                    Context.SaveChanges();

                }
                else
                {
                    var reading = new PowerAvg()
                    {
                        P1 = model.Power1,
                        P2 = model.Power2,
                        P3 = model.Power3,
                        primarySourceId = model.PrimarySourceId,
                        secondrySourceId = model.SecondarySourceId,
                        readingDate = model.TimeStamp
                    };
                    Context.PowerAvg.Add(reading);
                    Context.SaveChanges();

                }
            }
            else
            {
                var reading = new PowerAvg()
                {
                    P1 = model.Power1,
                    P2 = model.Power2,
                    P3 = model.Power3,
                    primarySourceId = model.PrimarySourceId,
                    secondrySourceId = model.SecondarySourceId,
                    readingDate = model.TimeStamp
                };
                Context.PowerAvg.Add(reading);
                Context.SaveChanges();
            }



            // return $"Ok," + DateTimeOffset.UtcNow.ToUnixTimeSeconds();


        }



    }
}