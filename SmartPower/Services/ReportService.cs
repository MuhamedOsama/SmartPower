using Microsoft.EntityFrameworkCore;
using SmartPower.Controllers.Domin;
using SmartPower.DataContext;
using SmartPower.Domin;
using SmartPower.Domin.report;
using SmartPower.Domin.Report;
using SmartPower.Models;
using SmartPower.Models.report;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace SmartPower.Services
{
    public class ReportService
    {
        private readonly PowerDbContext _Context;
        private readonly SecoundrySourceService secoundrySourceService;
        private readonly PrimarySourceSerivce primarySourceSerivce;

        public new Dictionary<int, List<Tuple<string, bool>>> sourceStatus;

        public ReportService(PowerDbContext _context)
        {
            _Context = _context;
            this.secoundrySourceService = new SecoundrySourceService(_context);
            this.primarySourceSerivce = new PrimarySourceSerivce(_context);
        }

        public CurrentTotalAvg AvgCalculaion()
        {
            CurrentTotalAvg Result = new CurrentTotalAvg();
            DateTime End = DateTime.Now;
            DateTime Start = End.AddDays(-1);

            List<decimal> Phaseone = new List<decimal>();
            List<decimal> PhaseTwo = new List<decimal>();
            List<decimal> PhaseThree = new List<decimal>();


            decimal SumAvgsOne = 0;
            decimal SumAvgsTwo = 0;
            decimal SumAvgsThree = 0;

            int counter = 0;

            //var Query = _Context.SourceAvgs.Where(a => a.Time >= Start && a.Time <= End).ToList().AsEnumerable();
            var Query = _Context.SourceAvgs.ToList().AsEnumerable();

            if (Query.Any())
            {
                foreach (var item in Query)
                {
                    Result.DataList.Add(new CurrentAvgDataModel()
                    {
                        CurrentOne = item.Current1Avg,
                        CurrentTwo = item.Current2Avg,
                        CurrentThree = item.Current3Avg,
                        Id = item.Id,
                        PS_Id = item.PrimarySourceId,
                        SS_Id = item.SecondarySourceId,
                        PS_Name = primarySourceSerivce.GetPrimarySourceName(item.PrimarySourceId),
                        SS_Name = secoundrySourceService.GetSecoundrySourceName(item.SecondarySourceId)
                    });
                }
                foreach (var item in Result.DataList)
                {
                    if (item.CurrentOne == 0 || item.CurrentTwo == 0 || item.CurrentThree == 0)
                    {

                    }
                    else
                    {
                        SumAvgsOne += item.CurrentOne;
                        SumAvgsTwo += item.CurrentTwo;
                        SumAvgsThree += item.CurrentThree;

                        counter++;
                    }
                }

                Result.TotalAvgOne = SumAvgsOne / counter;
                Result.TotalAvgTwo = SumAvgsTwo / counter;
                Result.TotalAvgThree = SumAvgsThree / counter;
            }
            //var Query2 = _Context.SourceReading.Where(r => r.TimeStamp >= Start && r.TimeStamp <= End).ToArray().AsEnumerable();
            var Query2 = _Context.SourceReading.ToArray().AsEnumerable();
            if (Query2.Any())
            {
                foreach (var Item in Query2)
                {
                    Phaseone.Add(Item.Current1);
                    PhaseTwo.Add(Item.Current2);
                    PhaseThree.Add(Item.Current3);
                }
                Result.PeakOne = Phaseone.Max();
                Result.PeakTwo = PhaseTwo.Max();
                Result.PeakThree = PhaseThree.Max();

                Result.VarOne = Result.PeakOne - Result.TotalAvgOne;
                Result.varTwo = Result.PeakTwo - Result.TotalAvgTwo;
                Result.VarThree = Result.PeakThree - Result.TotalAvgThree;
            }

            return Result;
        }

        //UnPalance Retio {Genearic}
        public decimal UnbalancedRatio(decimal Data1, decimal data2, decimal data3)
        {
            decimal result = 0;
            decimal temp = Data1 - data2;
            result = temp / Data1;
            return result;
        }

        public List<SourceBalnceRetio> CalculateBalanceRetio()
        {
            List<SourceBalnceRetio> Results = new List<SourceBalnceRetio>();
            var PrimarySources = primarySourceSerivce.GetAllPrimarySources();
            var SecoundrySources = secoundrySourceService.GetAllSecoundrySources();
            //Get Data
            foreach (var item in PrimarySources)
            {
                var data = _Context.SourceReading.Where(p => p.PrimarySourceId == item.Id).OrderByDescending(p => p.Id).Take(30).ToList();
                if (data.Count != 0)
                {
                    var CurrentRetio = UnbalancedRatio(data[20].Current1, data[20].Current2, data[20].Current3);
                    var VoltRetio = UnbalancedRatio(data[20].Voltage1, data[20].Voltage2, data[20].Voltage3);
                    var PfRetio = UnbalancedRatio(3, 3, 3);


                    Results.Add(new SourceBalnceRetio()
                    {
                        SourceId = item.Id,
                        I_Retio = CurrentRetio,
                        V_Retio = VoltRetio,
                        pf_Retio = PfRetio,
                        SourceName = item.Name,
                        Source_tybe = "Primary"
                    });
                }


            }
            foreach (var item in SecoundrySources)
            {
                var data = _Context.SourceReading.Where(p => p.SecondarySourceId == item.Id).Take(30).OrderByDescending(p => p.Id).ToList();
                if (data.Count != 0)
                {
                    var CurrentRetio = UnbalancedRatio(data[10].Current1, data[10].Current2, data[10].Current3);
                    var VoltRetio = UnbalancedRatio(data[20].Voltage1, data[20].Voltage2, data[20].Voltage3);
                    var PfRetio = UnbalancedRatio(3, 3, 3);

                    Results.Add(new SourceBalnceRetio()
                    {
                        SourceId = item.Id,
                        I_Retio = CurrentRetio,
                        V_Retio = VoltRetio,
                        pf_Retio = PfRetio,
                        SourceName = item.Name,
                        Source_tybe = "Secoundry"
                    });
                }


            }

            return Results;
        }

        //spike logs
        public List<SpikeLogsDataModel> getAllSpikes()
        {
            List<SpikeLogsDataModel> Results = new List<SpikeLogsDataModel>();

            DateTime End = DateTime.Now;
            DateTime Start = End.AddDays(-1);
            var data = _Context.CurrentSpikeLogs.Where(l => l.Time >= Start && l.Time <= End).ToList();

            foreach (var item in data)
            {
                Results.Add(new SpikeLogsDataModel()
                {
                    Id = item.Id,
                    PhaseName = item.PhaseName,
                    Time = item.Time,
                    Value = item.Value,
                    // secondarySourceId = item.SourceId,
                    SourceCode = item.SourceCode,

                });
            }
            return Results;
        }


        //ADEL
        public LoadTimeDownRatio LoadTimeDownRatio(int Id, DateTime from, DateTime to)
        {
            LoadTimeDownRatio model = new LoadTimeDownRatio();

            if (Id != -1)
            {
                // bngeb al reading ll load dh fel yom we al wa2t al matlob 
                var load = _Context.Load.SingleOrDefault(L => L.Id == Id);
                List<SourceReading> readings;
                if (from == to)
                {
                    if (load.SourceId % 2 == 0)
                        readings = _Context.SourceReading.Where(r => r.SecondarySourceId == load.SourceId && r.TimeStamp.Date == from.Date).ToList();
                    else
                        readings = _Context.SourceReading.Where(r => r.PrimarySourceId == load.SourceId && r.TimeStamp.Date == from.Date).ToList();

                }
                else
                {
                    if (load.SourceId % 2 == 0)
                        readings = _Context.SourceReading.Where(r => r.SecondarySourceId == load.SourceId && r.TimeStamp.Date >= from.Date && r.TimeStamp.Date <= to.Date).ToList();
                    else
                        readings = _Context.SourceReading.Where(r => r.PrimarySourceId == load.SourceId && r.TimeStamp.Date >= from.Date && r.TimeStamp.Date <= to.Date).ToList();
                }



                // up : ya3ny feh tayar 
                // down : ya3ny mafe4 tyar 

                // how many seconds the load where on 
                double upTime = 0;
                // how many seconds the load where off 
                double downTime = 0;
                // how many seconds the load where up+down ( wa2t m3rof belnsbalna feh 7aet al load swa2 up aw down  ) 
                double totalTime = 0;
                // how many seconds the load hasn't has reading or the reading were -1 ( wa2t m4 m3rof belnsbalna wad3 al load kan eh )
                double totalUnkonwnTime = 0;


                if (readings.Count != 0)
                {
                    // al 2eraya belnsbaly m3nah ano fdl 30sec 3al 7ala de flma bykon akbr mn 0 fhwa kda up belnsbaly lmodet 30 sanya we al 3ks 
                    // lw hwa 0 yb2a hwa dowb belnsbaly lmodet 30 sanya 
                    if (load.PhaseType == "3")
                    {
                        for (int i = 0; i < readings.Count; i++)
                        {

                            // hena 3amel anding 3l4an lw al 3 currents b 0 yb2a m4 48al 5als 
                            if (readings[i].Current1 == 0 && readings[i].Current2 == 0 && readings[i].Current3 == 0)
                            {
                                downTime += 30;
                            }
                            // hena 3amel oring 3l4an lw phase bs da5lha tyar yb2a hwa kda 48al
                            else if (readings[i].Current1 > 0 || readings[i].Current2 > 0 || readings[i].Current3 > 0)
                            {
                                upTime += 30;
                            }

                        }
                    }
                    else
                    {
                        PhasesConnection ps = _Context.PhasesConnection.SingleOrDefault(p => p.SourceId == load.SourceId);
                        if (ps.sN1 == 'l' + load.Id.ToString())
                        {

                            for (int i = 0; i < readings.Count; i++)
                            {

                                if (readings[i].Current1 == 0)
                                {
                                    downTime += 30;
                                }
                                else if (readings[i].Current1 > 0)
                                {
                                    upTime += 30;
                                }



                            }

                        }
                        else if (ps.sN2 == 'l' + load.Id.ToString())
                        {
                            for (int i = 0; i < readings.Count(); i++)
                            {

                                if (readings[i].Current2 == 0)
                                {
                                    downTime += 30;
                                }
                                else if (readings[i].Current2 > 0)
                                {
                                    upTime += 30;
                                }



                            }

                        }
                        else
                        {
                            for (int i = 0; i < readings.Count; i++)
                            {

                                if (readings[i].Current3 == 0)
                                {
                                    downTime += 30;
                                }
                                else if (readings[i].Current3 > 0)
                                {
                                    upTime += 30;
                                }



                            }

                        }


                    }



                    // how many seconds the load where up+down ( wa2t m3rof belnsbalna feh 7aet al load swa2 up aw down  ) 
                    totalTime = upTime + downTime;
                    // how many seconds the load hasn't has reading or the reading were -1 ( wa2t m4 m3rof belnsbalna wad3 al load kan eh )
                    // 3add al swany fel 24h 86400
                    totalUnkonwnTime = 86400 - totalTime;




                    //Time in hours 
                    model.totalKnownTime = (totalTime / 3600).ToString("0.00");
                    model.totalUnkonwnTime = (totalUnkonwnTime / 3600).ToString("0.00");
                    model.DownTimePerHour = (downTime / 3600).ToString("0.00");
                    model.DownTimePercentageToKnownTime = (((downTime / 3600) / (totalTime / 3600)) * 100).ToString("0.00");
                    model.loadId = load.Id;
                    model.LoadName = load.name;

                }


            }
            return model;

        }
        public TransitTimeViewModel TransitTimeAnalysis(int LoadId, int N, DateTime day)
        {
            TransitTimeViewModel model = new TransitTimeViewModel();
            if (LoadId != -1)
            {
                var load = _Context.Load.Include(l => l.LoadInfo).SingleOrDefault(L => L.Id == LoadId);

                List<SourceReading> readings;
                if (load.SourceId % 2 == 0)
                    readings = _Context.SourceReading.Where(r => r.SecondarySourceId == load.SourceId && r.TimeStamp.Date == day.Date).ToList();
                else
                    readings = _Context.SourceReading.Where(r => r.PrimarySourceId == load.SourceId && r.TimeStamp.Date == day.Date).ToList();


                var part = (load.LoadInfo.RatingCurrent) / N;
                //levels of t2sem
                if (readings.Count != 0)
                {
                    var parts = new List<decimal>();
                    parts.Add(0);
                    for (int i = 0; i < N; i++)
                    {
                        parts.Add(part + (i * part));
                    }




                    if (load.PhaseType == "3")
                    {
                        var times1 = new List<double>();
                        List<double> precentages1 = new List<double>();

                        var times2 = new List<double>();
                        List<double> precentages2 = new List<double>();

                        var times3 = new List<double>();
                        List<double> precentages3 = new List<double>();

                        // just make the first input 0 to keep parlel ( part1 -- [1])
                        for (int i = 0; i < N + 1; i++)
                        {
                            times1.Add(0);
                            times2.Add(0);
                            times3.Add(0);
                        }


                        // calculating minutes
                        for (int i = 0; i < readings.Count; i++)
                        {
                            if (i + 1 != readings.Count)
                            {
                                for (int y = 1; y < N + 1; y++)
                                {

                                    if (readings[i].Current1 <= parts[y])
                                    {
                                        times1[y] += (readings[i + 1].TimeStamp.Subtract(readings[i].TimeStamp)).TotalMinutes;
                                        break;
                                    }
                                }

                                for (int y = 1; y < N + 1; y++)
                                {
                                    if (readings[i].Current2 <= parts[y])
                                    {
                                        times2[y] += (readings[i + 1].TimeStamp.Subtract(readings[i].TimeStamp)).TotalMinutes;
                                        break;
                                    }
                                }

                                for (int y = 1; y < N + 1; y++)
                                {
                                    if (readings[i].Current3 <= parts[y])
                                    {
                                        times3[y] += (readings[i + 1].TimeStamp.Subtract(readings[i].TimeStamp)).TotalMinutes;
                                        break;
                                    }
                                }
                            }
                        }

                        // precentages 
                        double totaltime = readings[readings.Count() - 1].TimeStamp.Subtract(readings[0].TimeStamp).TotalMinutes;
                        precentages1.Add(1);
                        precentages2.Add(1);
                        precentages3.Add(1);


                        for (int i = 1; i < times1.Count; i++)
                        {
                            precentages1.Add((times1[i] / totaltime) * 100);
                            precentages2.Add((times2[i] / totaltime) * 100);
                            precentages3.Add((times3[i] / totaltime) * 100);

                        }






                        model.times1 = times1;
                        model.precentages1 = precentages1;

                        model.times2 = times2;
                        model.precentages2 = precentages2;

                        model.times3 = times3;
                        model.precentages3 = precentages3;
                        model.parts = parts;







                    }
                    else
                    {
                        PhasesConnection ps = _Context.PhasesConnection.SingleOrDefault(p => p.SourceId == load.SourceId);

                        if (ps.sN1 == 'l' + load.Id.ToString())
                        {
                            var times1 = new List<double>();
                            List<double> precentages1 = new List<double>();
                            // just make the first input 0 to keep parlel ( part1 -- [1])
                            for (int i = 0; i < N + 1; i++)
                            {
                                times1.Add(0);

                            }



                            for (int i = 0; i < readings.Count; i++)
                            {
                                if (i + 1 != readings.Count)
                                {
                                    for (int y = 1; y < N + 1; y++)
                                    {
                                        if (readings[i].Current1 <= parts[y])
                                        {
                                            times1[y] += (readings[i + 1].TimeStamp.Subtract(readings[i].TimeStamp)).TotalMinutes;
                                            break;
                                        }
                                    }
                                }




                            }

                            // precentages 
                            double totaltime = readings[readings.Count() - 1].TimeStamp.Subtract(readings[0].TimeStamp).TotalMinutes;
                            precentages1.Add(1);

                            for (int i = 1; i < times1.Count; i++)
                            {
                                precentages1.Add((times1[i] / totaltime) * 100);
                            }


                            model.times1 = times1;
                            model.precentages1 = precentages1;
                            model.parts = parts;

                        }
                        else if (ps.sN2 == 'l' + load.Id.ToString())
                        {
                            var times2 = new List<double>();
                            List<double> precentages2 = new List<double>();
                            // just make the first input 0 to keep parlel ( part1 -- [1])
                            for (int i = 0; i < N + 1; i++)
                            {
                                times2.Add(0);
                            }
                            for (int i = 0; i < readings.Count; i++)
                            {
                                if (i + 1 != readings.Count)
                                {
                                    for (int y = 1; y < N + 1; y++)
                                    {
                                        if (readings[i].Current2 <= parts[y])
                                        {
                                            times2[y] += (readings[i + 1].TimeStamp.Subtract(readings[i].TimeStamp)).TotalMinutes;
                                            break;
                                        }
                                    }

                                }
                            }



                            // precentages 
                            double totaltime = readings[readings.Count() - 1].TimeStamp.Subtract(readings[0].TimeStamp).TotalMinutes;
                            precentages2.Add(1);

                            for (int i = 1; i < times2.Count; i++)
                            {
                                precentages2.Add((times2[i] / totaltime) * 100);
                            }


                            model.times2 = times2;
                            model.precentages2 = precentages2;
                            model.parts = parts;

                        }
                        else
                        {
                            var times3 = new List<double>();
                            List<double> precentages3 = new List<double>();
                            // just make the first input 0 to keep parlel ( part1 -- [1])
                            for (int i = 0; i < N + 1; i++)
                            {

                                times3.Add(0);
                            }
                            for (int i = 0; i < readings.Count; i++)
                            {
                                if (i + 1 != readings.Count)
                                {

                                    for (int y = 1; y < N + 1; y++)
                                    {
                                        if (readings[i].Current3 <= parts[y])
                                        {
                                            times3[y] += (readings[i + 1].TimeStamp.Subtract(readings[i].TimeStamp)).TotalMinutes;
                                            break;
                                        }
                                    }
                                }

                            }


                            // precentages 
                            double totaltime = readings[readings.Count() - 1].TimeStamp.Subtract(readings[0].TimeStamp).TotalMinutes;
                            precentages3.Add(1);

                            for (int i = 1; i < times3.Count; i++)
                            {
                                precentages3.Add((times3[i] / totaltime) * 100);
                            }


                            model.times3 = times3;
                            model.precentages3 = precentages3;
                            model.parts = parts;

                        }






                    }

                }


            }


            return model;
        }

        //ABDO
        public PowerPeakViewModel PowerPeakBySourceID(int Id, int Type) //type 1 -> prim , 2 -> load
        {
            ReportService rs = new ReportService(_Context);
            PowerPeakViewModel Result = new PowerPeakViewModel();

            if (Type == 1) // prim
            {
                var code = Convert.ToString(Id);
                var source = _Context.powerPeak.SingleOrDefault(s => s.primarySourceId == Id);
                var Name = (_Context.PrimarySource.SingleOrDefault(s => s.Code == code)).Name;
                Result = MappingToPowerViewModel(source, Name);
                Result.loadtype = "-1";

            }
            else
            {
                var ld = _Context.Load.SingleOrDefault(l => l.Id == Id);
                var phsS = _Context.PhasesConnection.Where(s => s.SourceId == ld.SourceId).ToList();
                string loadPhase = "l" + ld.Id;
                var phs = new PhasesConnection();

                for (int i = 0; i < phsS.Count; i++)
                {
                    if (phsS[i].sN1 == loadPhase || phsS[i].sN2 == loadPhase || phsS[i].sN3 == loadPhase)
                    {
                        phs = phsS[i];
                        break;
                    }
                }
                powerPeak source;
                if (ld.SourceId % 2 == 0)
                    source = _Context.powerPeak.SingleOrDefault(s => s.secondrySourceId == ld.SourceId);
                else
                    source = _Context.powerPeak.SingleOrDefault(s => s.primarySourceId == ld.SourceId);
                //var Name = (_Context.secondarySource.SingleOrDefault(s => s.Id == ld.SourceId)).Name;
                Result = MappingToPowerViewModel(source, ld.name);
                Result.loadtype = ld.PhaseType;
                if (ld.PhaseType == "1")
                {
                    string tst = "l" + ld.Id;
                    if (phs.sN1 == tst)
                        Result.phasenumber = 1;
                    if (phs.sN2 == tst)
                        Result.phasenumber = 2;
                    if (phs.sN3 == tst)
                        Result.phasenumber = 3;
                }
            }
            return Result;
        }
        public PowerPeakViewModel MappingToPowerViewModel(powerPeak obj, string name)
        {
            PowerPeakViewModel pp = new PowerPeakViewModel
            {
                name = name,
                dateP1 = obj.dateP1,
                dateP2 = obj.dateP2,
                dateP3 = obj.dateP3,
                peakP1 = obj.peakP1,
                peakP2 = obj.peakP2,
                peakP3 = obj.peakP3,
                Id = obj.Id
            };
            return pp;

        }
        public RushHourViewModel RushHourForLoadById(int Id, DateTime date)
        {
            //SqlConnection con = new SqlConnection("Server=WIN-P2A6HV3LSI9\\SQLEXPRESS;Database=power;Integrated Security=SSPI;");
            //SqlConnection con = new SqlConnection("Server=localhost;Database=power;Integrated Security=SSPI;");
            // con.Open();

            // SqlDataReader rdr;

            var ld = _Context.Load.SingleOrDefault(l => l.Id == Id);
            var phsS = _Context.PhasesConnection.Where(s => s.SourceId == ld.SourceId).ToList();
            string loadPhase = "l" + ld.Id;
            var phs = new PhasesConnection();

            for (int i = 0; i < phsS.Count; i++)
            {
                if (phsS[i].sN1 == loadPhase || phsS[i].sN2 == loadPhase || phsS[i].sN3 == loadPhase)
                {
                    phs = phsS[i];
                    break;
                }
            }

            RushHourViewModel rush = new RushHourViewModel();
            rush.name = ld.name;
            /*string tmpdate = date.ToString("yyyy-MM-dd")*/
            ;
            //SqlCommand cmd;
            DateTime yy;
            if (ld.SourceId % 2 == 0)
            {

                //var xx = _Context.SourceReading.Where(r => r.SecondarySourceId == ld.SourceId).Where(r => r.TimeStamp.Date == date.Date).MinAsync(r=>r.TimeStamp);
                yy = _Context.SourceReading.Where(r => r.SecondarySourceId == ld.SourceId).Where(r => r.TimeStamp.Date == date.Date).Select(r => r.TimeStamp).FirstOrDefault();

                //cmd = new SqlCommand("select top 1 TimeStamp from SourceReading" +
                //                           " where SecondarySourceId =" + ld.SourceId + "and CONVERT(date,TimeStamp)= '" + tmpdate + "'", con);
            }
            else
            {
                yy = _Context.SourceReading.Where(r => r.PrimarySourceId == ld.SourceId).Where(r => r.TimeStamp.Date == date.Date).Select(r => r.TimeStamp).FirstOrDefault();

                //cmd = new SqlCommand("select top 1 TimeStamp from SourceReading" +
                //                          " where PrimarySourceId =" + ld.SourceId + "and CONVERT(date,TimeStamp)= '" + tmpdate + "'", con);
            }


            //cmd.CommandType = CommandType.Text;
            //rdr = cmd.ExecuteReader();
            //if(rdr.Read())
            //{
            //    DateTime starttime = DateTime.Parse(rdr[0].ToString());
            //    DateTime endtime = starttime.AddHours(1);
            //    rdr.Close();

            while (yy.Day <= date.Day)
            {
                //        if (ld.SourceId % 2 == 0)
                //        {

                //            cmd = new SqlCommand("select AVG(Current1),AVG(Current2),AVG(Current3) from SourceReading" +
                //                                    " where TimeStamp between '" + starttime + "' and '" + endtime + "' and SecondarySourceId =" + ld.SourceId + "and CONVERT(date,TimeStamp)= '" + tmpdate + "'", con);
                //        }
                //        else
                //        {
                //            cmd = new SqlCommand("select AVG(Current1),AVG(Current2),AVG(Current3) from SourceReading" +
                //                                                            " where TimeStamp between '" + starttime + "' and '" + endtime + "' and PrimarySourceId =" + ld.SourceId + "and CONVERT(date,TimeStamp)= '" + tmpdate + "'", con);

                //        }

                Decimal AVG1 = 00.000m;
                Decimal AVG2 = 00.000m;
                Decimal AVG3 = 00.000m;

                if (ld.SourceId % 2 == 0) //sec source 
                {

                    AVG1 = _Context.SourceReading.Where(r => r.TimeStamp >= yy).Where(r => r.TimeStamp <= yy.AddHours(1)).Where(r => r.SecondarySourceId == ld.SourceId).DefaultIfEmpty().Average(r => r.Current1);
                    AVG2 = _Context.SourceReading.Where(r => r.TimeStamp >= yy).Where(r => r.TimeStamp <= yy.AddHours(1)).Where(r => r.SecondarySourceId == ld.SourceId).DefaultIfEmpty().Average(r => r.Current2);
                    AVG3 = _Context.SourceReading.Where(r => r.TimeStamp >= yy).Where(r => r.TimeStamp <= yy.AddHours(1)).Where(r => r.SecondarySourceId == ld.SourceId).DefaultIfEmpty().Average(r => r.Current3);

                }
                else
                {

                    AVG1 = _Context.SourceReading.Where(r => r.TimeStamp >= yy).Where(r => r.TimeStamp <= yy.AddHours(1)).Where(r => r.PrimarySourceId == ld.SourceId).DefaultIfEmpty().Average(r => r.Current1);
                    AVG2 = _Context.SourceReading.Where(r => r.TimeStamp >= yy).Where(r => r.TimeStamp <= yy.AddHours(1)).Where(r => r.PrimarySourceId == ld.SourceId).DefaultIfEmpty().Average(r => r.Current2);
                    AVG3 = _Context.SourceReading.Where(r => r.TimeStamp >= yy).Where(r => r.TimeStamp <= yy.AddHours(1)).Where(r => r.PrimarySourceId == ld.SourceId).DefaultIfEmpty().Average(r => r.Current3);
                }



                //cmd.CommandType = CommandType.Text;
                //rdr = cmd.ExecuteReader();
                //if (rdr.Read())
                //{
                //    if (rdr[0].ToString() != "")
                //    {
                AVG1 = Math.Round(AVG1, 2);
                AVG2 = Math.Round(AVG2, 2);
                AVG3 = Math.Round(AVG3, 2);

                if (AVG1 >= rush.peack1)
                { rush.peack1 = AVG1; rush.StartPeakonetime = yy.ToShortTimeString(); rush.EndPeakonetime = yy.AddHours(1).ToShortTimeString(); }
                if (AVG2 >= rush.peack2)
                { rush.peack2 = AVG2; rush.StartPeaktwotime = yy.ToShortTimeString(); rush.EndPeaktwotime = yy.AddHours(1).ToShortTimeString(); }
                if (AVG3 >= rush.peack3)
                { rush.peack3 = AVG3; rush.StartPeakthreetime = yy.ToShortTimeString(); rush.EndPeakthreetime = yy.AddHours(1).ToShortTimeString(); }
                //}
                //}
                //rdr.Close();
                //starttime = endtime;
                //endtime = endtime.AddHours(1);

                yy = yy.AddHours(1);

            }
            rush.loadtype = ld.PhaseType;
            if (ld.PhaseType == "1")
            {
                string tst = "l" + ld.Id;
                if (phs.sN1 == tst)
                    rush.phasenumber = 1;
                if (phs.sN2 == tst)
                    rush.phasenumber = 2;
                if (phs.sN3 == tst)
                    rush.phasenumber = 3;

            }
            // }


            return rush;
        }
        private List<double> CalculateStdDev(List<decimal> values)
        {
            double ret = 0;
            List<double> res = new List<double>();
            if (values.Count() > 0)
            {
                double avg = (double)values.Average();

                double sum = values.Sum(d => Math.Pow((double)d - avg, 2));
                res.Add(Math.Round((sum) / (values.Count()), 2));
                //Put it all together      
                ret = Math.Round(Math.Sqrt((sum) / (values.Count())), 2);
                res.Add(ret);
            }
            return res;
        }


        //Calculate Max avg of 1  hour in  The Day  
        // update standard Diviasion  if  Avg(i) >  Avg(i-1)
        // return Rush View Model  in Harmonic And rush Hour


        public RushHourViewModel HaemonicOrderForLoadId(int Id, DateTime date, int HarmoOrder)
        {
            //SqlConnection con = new SqlConnection("Server=localhost;Database=power;Integrated Security=SSPI;");
            ////SqlConnection con = new SqlConnection("Server=WIN-P2A6HV3LSI9\\SQLEXPRESS;Database=power;Integrated Security=SSPI;");
            //con.Open();
            //SqlDataReader rdr;
            var ld = _Context.Load.SingleOrDefault(l => l.Id == Id);

            var phsS = _Context.PhasesConnection.Where(s => s.SourceId == ld.SourceId).ToList();
            string loadPhase = "l" + ld.Id;
            var phs = new PhasesConnection();
            for (int i = 0; i < phsS.Count; i++)
            {
                if (phsS[i].sN1 == loadPhase || phsS[i].sN2 == loadPhase || phsS[i].sN3 == loadPhase)
                {
                    phs = phsS[i];
                    break;
                }
            }

            RushHourViewModel rush = new RushHourViewModel();
            rush.name = ld.name;
            //string tmpdate = date.ToString("yyyy-MM-dd");
            //SqlCommand cmd;

            DateTime kk;


            if (ld.SourceId % 2 == 0)
            {

                kk = _Context.SourceReading.Where(r => r.SecondarySourceId == ld.SourceId).Where(r => r.TimeStamp.Date == date.Date).Where(r => r.HarmonicOrder == HarmoOrder).Select(r => r.TimeStamp).FirstOrDefault();

                ////cmd = new SqlCommand("select top 1 TimeStamp from SourceReading" +
                ////                           " where SecondarySourceId =" + ld.SourceId + "and CONVERT(date,TimeStamp)= '" + tmpdate + "'and HarmonicOrder = " + HarmoOrder, con);
            }
            else
            {
                kk = _Context.SourceReading.Where(r => r.PrimarySourceId == ld.SourceId).Where(r => r.TimeStamp.Date == date.Date).Where(r => r.HarmonicOrder == HarmoOrder).Select(r => r.TimeStamp).FirstOrDefault();

                //cmd = new SqlCommand("select top 1 TimeStamp from SourceReading" +
                //                           " where PrimarySourceId =" + ld.SourceId + "and CONVERT(date,TimeStamp)= '" + tmpdate + "'and HarmonicOrder = " + HarmoOrder, con);
            }

            //cmd.CommandType = CommandType.Text;
            //rdr = cmd.ExecuteReader();

            //if (rdr.Read())
            //{
            //    DateTime starttime = DateTime.Parse(rdr[0].ToString());
            //    DateTime endtime = starttime.AddHours(1);
            //    rdr.Close();
            /*starttime.Day <= date.Day &&*/
            while (kk.Day == date.Day)
            {
                decimal AVG1 = 00.00m;
                decimal AVG2 = 00.00m;
                decimal AVG3 = 00.00m;
                if (ld.SourceId % 2 == 0)
                {



                    // AVG1 = _Context.SourceReading.Where(r => r.TimeStamp >= yy).Where(r => r.TimeStamp <= yy.AddHours(1)).Where(r => r.PrimarySourceId == ld.SourceId).AverageAsync(r => r.Current1).Result;
                    AVG1 = _Context.SourceReading.Where(r => r.TimeStamp >= kk).Where(r => r.TimeStamp.Date == date.Date).Where(r => r.TimeStamp <= kk.AddHours(1)).Where(r => r.SecondarySourceId == ld.SourceId).Where(r => r.HarmonicOrder == HarmoOrder).DefaultIfEmpty().Average(r => r.HarmonicOrder1);
                    AVG2 = _Context.SourceReading.Where(r => r.TimeStamp >= kk).Where(r => r.TimeStamp.Date == date.Date).Where(r => r.TimeStamp <= kk.AddHours(1)).Where(r => r.SecondarySourceId == ld.SourceId).Where(r => r.HarmonicOrder == HarmoOrder).DefaultIfEmpty().Average(r => r.HarmonicOrder2);
                    AVG3 = _Context.SourceReading.Where(r => r.TimeStamp >= kk).Where(r => r.TimeStamp.Date == date.Date).Where(r => r.TimeStamp <= kk.AddHours(1)).Where(r => r.SecondarySourceId == ld.SourceId).Where(r => r.HarmonicOrder == HarmoOrder).DefaultIfEmpty().Average(r => r.HarmonicOrder3);


                    //cmd = new SqlCommand("select AVG(HarmonicOrder1),AVG(HarmonicOrder2),AVG(HarmonicOrder3) from SourceReading" +
                    //                        " where TimeStamp between '" + starttime + "' and '" + endtime + "' and CONVERT(date,TimeStamp)= '" + tmpdate + "' and HarmonicOrder = " + HarmoOrder + " and SecondarySourceId = " + ld.SourceId, con);
                }
                else
                {
                    AVG1 = _Context.SourceReading.Where(r => r.TimeStamp >= kk).Where(r => r.TimeStamp.Date == date.Date).Where(r => r.TimeStamp <= kk.AddHours(1)).Where(r => r.PrimarySourceId == ld.SourceId).Where(r => r.HarmonicOrder == HarmoOrder).DefaultIfEmpty().Average(r => r.HarmonicOrder1);
                    AVG2 = _Context.SourceReading.Where(r => r.TimeStamp >= kk).Where(r => r.TimeStamp.Date == date.Date).Where(r => r.TimeStamp <= kk.AddHours(1)).Where(r => r.PrimarySourceId == ld.SourceId).Where(r => r.HarmonicOrder == HarmoOrder).DefaultIfEmpty().Average(r => r.HarmonicOrder2);
                    AVG3 = _Context.SourceReading.Where(r => r.TimeStamp >= kk).Where(r => r.TimeStamp.Date == date.Date).Where(r => r.TimeStamp <= kk.AddHours(1)).Where(r => r.PrimarySourceId == ld.SourceId).Where(r => r.HarmonicOrder == HarmoOrder).DefaultIfEmpty().Average(r => r.HarmonicOrder3);


                    //cmd = new SqlCommand("select AVG(HarmonicOrder1),AVG(HarmonicOrder2),AVG(HarmonicOrder3) from SourceReading" +
                    //                " where TimeStamp between '" + starttime + "' and '" + endtime + "' and CONVERT(date,TimeStamp)= '" + tmpdate + "' and HarmonicOrder = " + HarmoOrder + " and PrimarySourceId = " + ld.SourceId, con);
                }
                //cmd.CommandType = CommandType.Text;
                //rdr = cmd.ExecuteReader();
                //if (rdr.Read())
                //{
                //    if (rdr[0].ToString() != "")
                //    {
                List<decimal> har1;
                List<decimal> har2;
                List<decimal> har3;
                //Calculate  STandard Diviasion And Variance
                if (ld.SourceId % 2 == 0)
                {
                    har1 = _Context.SourceReading.Where(s => s.TimeStamp >= kk && s.TimeStamp <= kk.AddHours(1) && s.HarmonicOrder == HarmoOrder && s.SecondarySourceId == ld.SourceId).Select(a => a.HarmonicOrder1).DefaultIfEmpty().ToList();
                    har2 = _Context.SourceReading.Where(s => s.TimeStamp >= kk && s.TimeStamp <= kk.AddHours(1) && s.HarmonicOrder == HarmoOrder && s.SecondarySourceId == ld.SourceId).Select(a => a.HarmonicOrder2).DefaultIfEmpty().ToList();
                    har3 = _Context.SourceReading.Where(s => s.TimeStamp >= kk && s.TimeStamp <= kk.AddHours(1) && s.HarmonicOrder == HarmoOrder && s.SecondarySourceId == ld.SourceId).Select(a => a.HarmonicOrder3).DefaultIfEmpty().ToList();
                }
                else
                {
                    har1 = _Context.SourceReading.Where(s => s.TimeStamp >= kk && s.TimeStamp <= kk.AddHours(1) && s.HarmonicOrder == HarmoOrder && s.PrimarySourceId == ld.SourceId).Select(a => a.HarmonicOrder1).DefaultIfEmpty().ToList();
                    har2 = _Context.SourceReading.Where(s => s.TimeStamp >= kk && s.TimeStamp <= kk.AddHours(1) && s.HarmonicOrder == HarmoOrder && s.PrimarySourceId == ld.SourceId).Select(a => a.HarmonicOrder2).DefaultIfEmpty().ToList();
                    har3 = _Context.SourceReading.Where(s => s.TimeStamp >= kk && s.TimeStamp <= kk.AddHours(1) && s.HarmonicOrder == HarmoOrder && s.PrimarySourceId == ld.SourceId).Select(a => a.HarmonicOrder3).DefaultIfEmpty().ToList();
                }

                AVG1 = Math.Round(AVG1, 2);
                AVG2 = Math.Round(AVG2, 2);
                AVG3 = Math.Round(AVG3, 2);

                if (AVG1 >= rush.peack1)
                {

                    rush.peack1 = Math.Round(AVG1, 2); rush.StartPeakonetime = kk.ToShortTimeString(); rush.EndPeakonetime = kk.AddHours(1).ToShortTimeString();
                    rush.Standard1 = CalculateStdDev(har1)[1];
                    rush.Variance1 = CalculateStdDev(har1)[0];
                }
                if (AVG2 >= rush.peack2)
                {
                    rush.peack2 = Math.Round(AVG2, 2); rush.StartPeaktwotime = kk.ToShortTimeString(); rush.EndPeaktwotime = kk.AddHours(1).ToShortTimeString();
                    rush.Standard2 = CalculateStdDev(har2)[1];
                    rush.Variance2 = CalculateStdDev(har2)[0];
                }
                if (AVG3 >= rush.peack3)
                {
                    rush.peack3 = Math.Round(AVG3, 2); rush.StartPeakthreetime = kk.ToShortTimeString(); rush.EndPeakthreetime = kk.AddHours(1).ToShortTimeString();
                    rush.Standard3 = CalculateStdDev(har3)[1];
                    rush.Variance3 = CalculateStdDev(har3)[0];
                }
                //    }
                //}
                //rdr.Close();
                //starttime = endtime;
                //endtime = endtime.AddHours(1);

                kk = kk.AddHours(1);


            }
            rush.loadtype = ld.PhaseType;
            if (ld.PhaseType == "1")
            {
                string tst = "l" + ld.Id;
                if (phs.sN1 == tst)
                    rush.phasenumber = 1;
                if (phs.sN2 == tst)
                    rush.phasenumber = 2;
                if (phs.sN3 == tst)
                    rush.phasenumber = 3;

            }
            //}



            return rush;
        }
        public List<ProductionViewModel> ProductionReport(DateTime fromdate, DateTime todate, int facid, int Type)// 1 =>prim , 2 load
        {
            ProductionService ps = new ProductionService(_Context);
            List<ProductionViewModel> res = new List<ProductionViewModel>(); DateTime CurrentDate = fromdate;
            var prod = _Context.Production.Where(s => s.FacId == facid && s.Date.Date >= fromdate.Date && s.Date.Date <= todate.Date).ToList();
            if (prod.Count != 0)
            {
                double Count = (todate - fromdate).TotalDays;
                if (Type == 1)
                {
                    res = ps.MappingToproductionModellist(prod);
                    for (int i = 0; i < res.Count; i++)
                    {
                        GetEnrgyOfoneDayForPrimaries(res[i].Date, facid, ref res, i);
                    }
                }
                else if (Type == 2)
                {
                    for (int i = 0; i <= Count; i++)
                    {
                        GetEnrgyOfoneDayForLoads(CurrentDate.ToString("yyyy-MM-dd"), facid, ref res, i);
                        CurrentDate = CurrentDate.AddDays(1);
                    }
                }
            }


            return res;
        }
        //production
        public void GetEnrgyOfoneDayForLoads(string current, int facid, ref List<ProductionViewModel> res, int idx)
        {

            var loads = _Context.Load.ToList();

            //SqlConnection con = new SqlConnection("Server=WIN-P2A6HV3LSI9\\SQLEXPRESS;Database=power;Integrated Security=SSPI;");
            //SqlConnection con = new SqlConnection("Server=localhost;Database=power;Integrated Security=SSPI;");
            //con.Open();
            //SqlDataReader rdr;
            //SqlCommand cmd = new SqlCommand();
            //cmd.Connection = con;
            //cmd.CommandType = CommandType.Text;
            DateTime date2 = Convert.ToDateTime(current);
            res.Add(new ProductionViewModel
            {
                FacId = facid,
                facname = (_Context.Factory.SingleOrDefault(s => s.Id == facid)).Name,
                Date = current
            });
            for (int i = 0; i < loads.Count; i++)
            {

                Decimal Spow1 = 0.00m;
                Decimal Spow2 = 0.00m;
                Decimal Spow3 = 0.00m;

                string name;
                string type;
                if (loads[i].SourceId % 2 == 0)
                {
                    Spow1 = _Context.SourceReading.Where(r => r.Fac_Id == facid).Where(r => r.TimeStamp.Date == date2).Where(r => r.SecondarySourceId == loads[i].SourceId).DefaultIfEmpty().Sum(r => r.Power1) / 60;
                    Spow2 = _Context.SourceReading.Where(r => r.Fac_Id == facid).Where(r => r.TimeStamp.Date == date2).Where(r => r.SecondarySourceId == loads[i].SourceId).DefaultIfEmpty().Sum(r => r.Power2) / 60;
                    Spow3 = _Context.SourceReading.Where(r => r.Fac_Id == facid).Where(r => r.TimeStamp.Date == date2).Where(r => r.SecondarySourceId == loads[i].SourceId).DefaultIfEmpty().Sum(r => r.Power3) / 60;



                    //cmd.CommandText = "select sum(power1)/60,sum(power2)/60,sum(power3)/60 from   SourceReading where   Fac_Id = " + facid + " and " +
                    //              " CONVERT(date,TimeStamp)= '" + current + "' and " + loads[i].SourceId + " = SecondarySourceId ";
                    name = (_Context.secondarySource.SingleOrDefault(s => s.Id == loads[i].SourceId)).Name;
                    type = (_Context.secondarySource.SingleOrDefault(s => s.Id == loads[i].SourceId)).Type;
                }
                else
                {
                    Spow1 = _Context.SourceReading.Where(r => r.Fac_Id == facid).Where(r => r.TimeStamp.Date == date2).Where(r => r.PrimarySourceId == loads[i].SourceId).Sum(r => r.Power1) / 60;
                    Spow2 = _Context.SourceReading.Where(r => r.Fac_Id == facid).Where(r => r.TimeStamp.Date == date2).Where(r => r.PrimarySourceId == loads[i].SourceId).Sum(r => r.Power2) / 60;
                    Spow3 = _Context.SourceReading.Where(r => r.Fac_Id == facid).Where(r => r.TimeStamp.Date == date2).Where(r => r.PrimarySourceId == loads[i].SourceId).Sum(r => r.Power3) / 60;


                    //cmd.CommandText = "select sum(power1)/60,sum(power2)/60,sum(power3)/60 from  SourceReading where   Fac_Id = " + facid + " and " +
                    //                " CONVERT(date,TimeStamp)= '" + current + "' and " + loads[i].SourceId + " = PrimarySourceId ";
                    name = (_Context.PrimarySource.SingleOrDefault(s => s.Code == Convert.ToString(loads[i].SourceId))).Name;
                    type = (_Context.PrimarySource.SingleOrDefault(s => s.Code == Convert.ToString(loads[i].SourceId))).Type;
                }
                //rdr = cmd.ExecuteReader();
                //if (rdr.Read())
                //{

                //  if (rdr[0].ToString() == "" || rdr[0].ToString() == "" || rdr[0].ToString() == "")
                if (Spow1 == 0 && Spow2 == 0 && Spow3 == 0)

                {
                    //rdr.Close();
                    continue;
                }
                if (!res[idx].Diction.ContainsKey(loads[i].Id))
                {
                    res[idx].Diction[loads[i].Id] = new List<Tuple<string, string, string, string, string>>();
                }
                if (type == "3")
                {
                    // res[idx].Diction[loads[i].Id].Add(new Tuple<string, string, string, string, string>(loads[i].name, current,Math.Round(Convert.ToDecimal(rdr[0].ToString()),2).ToString(), Math.Round(Convert.ToDecimal(rdr[1].ToString()), 2).ToString(), Math.Round(Convert.ToDecimal(rdr[2].ToString()), 2).ToString()));
                    res[idx].Diction[loads[i].Id].Add(new Tuple<string, string, string, string, string>(loads[i].name, current, Math.Round(Spow1, 2).ToString(), Math.Round(Spow2, 2).ToString(), Math.Round(Spow3, 2).ToString()));
                }
                else
                {
                    // res[idx].Diction[loads[i].Id].Add(new Tuple<string, string, string, string, string>(loads[i].name, current, Math.Round(Convert.ToDecimal(rdr[0].ToString()), 2).ToString(), "---", "---"));
                    res[idx].Diction[loads[i].Id].Add(new Tuple<string, string, string, string, string>(loads[i].name, current, Math.Round(Spow1, 2).ToString(), "---", "---"));
                }
            }
            //    rdr.Close();
            //}

            //con.Close();
            return;
        }
        public void GetEnrgyOfoneDayForPrimaries(string current, int facid, ref List<ProductionViewModel> res, int idx)
        {

            //SqlConnection con = new SqlConnection("Server=WIN-P2A6HV3LSI9\\SQLEXPRESS;Database=power;Integrated Security=SSPI;");
            //SqlConnection con = new SqlConnection("Server=localhost;Database=power;Integrated Security=SSPI;");

            //con.Open();
            //SqlDataReader rdr;
            DateTime date2 = Convert.ToDateTime(current);
            //SqlCommand cmd = new SqlCommand("select distinct  ISNULL(PrimarySourceId,0)  from SourceReading " +
            //                               " where CONVERT(date,TimeStamp) = '" + current + "' and Fac_Id = " + facid, con);


            var PrimList = _Context.SourceReading.Where(r => r.TimeStamp.Date == date2).Where(r => r.PrimarySourceId != null).Where(r => r.Fac_Id == facid).Select(r => r.PrimarySourceId).Distinct().ToList();
            //for (int a = 0; a < PrimList.Count(); a++) {
            //    int x = PrimList[a].Value;
            //         }
            //cmd.CommandType = CommandType.Text;
            //rdr = cmd.ExecuteReader();
            //List<int> primaries = new List<int>();  //idhat
            //int primiid = 0;
            //int c = 0;
            //while (rdr.Read())
            //{
            //    int primid = int.Parse(rdr[0].ToString());
            //    if (primid != 0) primaries.Add(int.Parse(rdr[0].ToString()));
            //   //primiid = int.Parse(PrimList.ElementAt(c).ToString());
            //   // if (primiid != 0) { PrimList.Add(int.Parse(PrimList.ElementAt(c).ToString())); }
            //   // c++;

            //    //int primid = int.Parse(ll[0].ToString());
            //    //if (primid != 0) primaries.Add(int.Parse(ll[0].ToString()));

            //}
            //rdr.Close();

            int primid = 0;
            ////for (int i = 0; i < PrimList.Count(); i++)
            ////{
            ////    primid = int.Parse(PrimList[i].Value.ToString());
            ////    if (primid != 0) PrimList.Add(PrimList[i].Value);
            ////}

            //int primid = 0;

            //for (int i = 0; i < primaries.Count; i++)
            for (int i = 0; i < PrimList.Count() /* i < primaries.Count*/; i++)

            {



                primid = int.Parse(PrimList[i].Value.ToString());






                decimal AvgPow1 = 00.00m;
                decimal AvgPow2 = 00.00m;
                decimal AvgPow3 = 00.00m;
                decimal EngPow1 = 00.00m;
                decimal EngPow2 = 00.00m;
                decimal EngPow3 = 00.00m;

                AvgPow1 = _Context.SourceReading.Where(r => r.TimeStamp.Date == date2).Where(r => r.Fac_Id == facid).Where(r => r.PrimarySourceId == primid).DefaultIfEmpty().Average(r => r.Power1);
                AvgPow2 = _Context.SourceReading.Where(r => r.TimeStamp.Date == date2).Where(r => r.Fac_Id == facid).Where(r => r.PrimarySourceId == primid).DefaultIfEmpty().Average(r => r.Power2);
                AvgPow3 = _Context.SourceReading.Where(r => r.TimeStamp.Date == date2).Where(r => r.Fac_Id == facid).Where(r => r.PrimarySourceId == primid).DefaultIfEmpty().Average(r => r.Power3);
                EngPow1 = AvgPow1 * 24;
                EngPow2 = AvgPow2 * 24;
                EngPow3 = AvgPow3 * 24;


                //cmd.CommandText = "select  Avg(power1) *24 , Avg(power2)*24,Avg(power3)*24,Avg(power1),Avg(power2),Avg(power3) from SourceReading " +
                //                      " where CONVERT(date,TimeStamp)= '" + current + "' and Fac_Id = " + facid + " and PrimarySourceId =" + primaries[i];
                //cmd.CommandType = CommandType.Text;
                //rdr = cmd.ExecuteReader();

                //string name = (_Context.PrimarySource.SingleOrDefault(s => s.Id == primaries[i])).Name;
                //string type = (_Context.PrimarySource.SingleOrDefault(s => s.Id == primaries[i])).Type;


                string name = (_Context.PrimarySource.SingleOrDefault(s => s.Code == primid.ToString())).Name;
                string type = (_Context.PrimarySource.SingleOrDefault(s => s.Code == primid.ToString())).Type;

                //if (rdr.Read())
                //{
                if (!res[idx].Diction.ContainsKey(primid))
                {
                    res[idx].Diction[primid] = new List<Tuple<string, string, string, string, string>>();
                    res[idx].AvgPower[primid] = new List<Tuple<string, string, string>>();
                    res[idx].Ratio[primid] = new List<Tuple<string, string, string>>();

                }
                if (type == "3")
                {
                    //res[idx].Diction[primaries[i]].Add(new Tuple<string, string, string, string, string>(name, current, Math.Round(Convert.ToDecimal(rdr[0].ToString()), 2).ToString(), Math.Round(Convert.ToDecimal(rdr[1].ToString()), 2).ToString(), Math.Round(Convert.ToDecimal(rdr[2].ToString()), 2).ToString()));
                    //res[idx].AvgPower[primaries[i]].Add(new Tuple<string, string, string>(Math.Round(Convert.ToDecimal(rdr[3].ToString()), 2).ToString(), Math.Round(Convert.ToDecimal(rdr[4].ToString()), 2).ToString(), Math.Round(Convert.ToDecimal(rdr[5].ToString()), 2).ToString()));
                    //res[idx].Ratio[primaries[i]].Add(new Tuple<string, string, string>((Math.Round(Double.Parse(rdr[3].ToString()) / res[idx].Quantity, 5)).ToString(), (Math.Round(Double.Parse(rdr[4].ToString()) / res[idx].Quantity, 5)).ToString(), (Math.Round(Double.Parse(rdr[5].ToString()) / res[idx].Quantity, 5)).ToString()));

                    res[idx].Diction[primid].Add(new Tuple<string, string, string, string, string>(name, current, Math.Round(EngPow1, 2).ToString(), Math.Round(EngPow2, 2).ToString(), Math.Round(EngPow3, 2).ToString()));
                    res[idx].AvgPower[primid].Add(new Tuple<string, string, string>(Math.Round(AvgPow1, 2).ToString(), Math.Round(AvgPow2, 2).ToString(), Math.Round(AvgPow3, 2).ToString()));
                    res[idx].Ratio[primid].Add(new Tuple<string, string, string>((Math.Round(Double.Parse(AvgPow1.ToString()) / res[idx].Quantity, 5)).ToString(), (Math.Round(Double.Parse(AvgPow2.ToString()) / res[idx].Quantity, 5)).ToString(), (Math.Round(Double.Parse(AvgPow3.ToString()) / res[idx].Quantity, 5)).ToString()));
                }
                else
                {
                    res[idx].Diction[primid].Add(new Tuple<string, string, string, string, string>(name, current, Math.Round(Convert.ToDecimal(EngPow1), 2).ToString(), "---", "---"));
                    res[idx].AvgPower[primid].Add(new Tuple<string, string, string>(Math.Round(AvgPow1, 2).ToString(), "---", "---"));
                    res[idx].Ratio[primid].Add(new Tuple<string, string, string>(Math.Round(Double.Parse(AvgPow1.ToString()) / res[idx].Quantity, 2).ToString(), "---", "---"));

                }
            }
            //    rdr.Close();
            //}
            //rdr.Close();
            //    con.Close();
            return;
        }

        public Tuple<decimal, decimal, decimal> GetEnergyConsumedForOneLoad(Load load, string current, int facid)
        {
            //Tuple<decimal, decimal, decimal> res;

            // SqlConnection con = new SqlConnection("Server=WIN-P2A6HV3LSI9\\SQLEXPRESS;Database=power;Integrated Security=SSPI;");
            //SqlConnection con = new SqlConnection("Server=localhost;Database=power;Integrated Security=SSPI;");
            //con.Open();
            //SqlDataReader rdr;
            //SqlCommand cmd = new SqlCommand();
            //cmd.Connection = con;
            //cmd.CommandType = CommandType.Text;
            var date = Convert.ToDateTime(current);
            var firCond = _Context.SourceReading.Where(r => r.Fac_Id == facid);
            var SecCond = firCond.Where(r => r.TimeStamp.Date == date.Date);


            if (load.SourceId % 2 == 0) // if secondary source
            {
                var power1 = SecCond.Where(r => r.SecondarySourceId == load.SourceId).Sum(r => r.Power1);
                var power2 = SecCond.Where(r => r.SecondarySourceId == load.SourceId).Sum(r => r.Power2);
                var power3 = SecCond.Where(r => r.SecondarySourceId == load.SourceId).Sum(r => r.Power3);

                var res1 = power1 / 60;
                var res2 = power2 / 60;
                var res3 = power3 / 60;


                return new Tuple<decimal, decimal, decimal>(Math.Round(res1, 2), Math.Round(res2), Math.Round(res3));



                //var jj= _Context.SourceReading
                //     .Where(r => r.Fac_Id == facid)
                //     .Where(r => r.TimeStamp == date)
                //     .Where(r => r.SecondarySourceId == load.SourceId)
                //     .Sum(r=>r.Fac_Id)
                //     .ToString();



                //Sum(r=>r.Power1)

            }
            else
            {

                var power1 = SecCond.Where(r => r.PrimarySourceId == load.SourceId).Sum(r => r.Power1);
                var power2 = SecCond.Where(r => r.PrimarySourceId == load.SourceId).Sum(r => r.Power2);
                var power3 = SecCond.Where(r => r.PrimarySourceId == load.SourceId).Sum(r => r.Power3);

                var res1 = power1 / 60;
                var res2 = power2 / 60;
                var res3 = power3 / 60;
                return new Tuple<decimal, decimal, decimal>(Math.Round(res1, 2), Math.Round(res2), Math.Round(res3));


                //  var jj = _Context.SourceReading.Where(r => r.Fac_Id == facid).Where(r => r.TimeStamp.ToString() == current).Where(r => r.PrimarySourceId == load.SourceId).Sum(r=>r.Power1);
            }

            //if (load.SourceId % 2 == 0)
            //{
            //    cmd.CommandText = "select sum(power1)/60,sum(power2)/60,sum(power3)/60 from   SourceReading where   Fac_Id = " + facid + " and " +
            //                  " CONVERT(date,TimeStamp)= '" + current + "' and " + load.SourceId + " = SecondarySourceId ";
            //}
            //else
            //{
            //    cmd.CommandText = "select sum(power1)/60,sum(power2)/60,sum(power3)/60 from  SourceReading where   Fac_Id = " + facid + " and " +
            //                    " CONVERT(date,TimeStamp)= '" + current + "' and " + load.SourceId + " = PrimarySourceId ";
            //}
            //rdr = cmd.ExecuteReader();
            //if (rdr.Read())
            //{

            //return new Tuple<decimal, decimal, decimal>(Math.Round(decimal.Parse(res1), 2), Math.Round(decimal.Parse(res2)), Math.Round(decimal.Parse(res3)));



        }
        public void GetAveragePerDay(int id, DateTime date, ref List<List<List<decimal>>> CurrentAvg, ref List<List<List<decimal>>> HarmAvg, ref List<List<List<decimal>>> VoltageAvg, ref List<List<List<decimal>>> PowerFactorAvg, int sourcid)
        {
            decimal c1 = 0, c2 = 0, c3 = 0, v1 = 0, v2 = 0, v3 = 0, f1 = 0, f2 = 0, f3 = 0, h1 = 0, h2 = 0, h3 = 0;
            decimal cnt = 0;
            List<SourceReading> q;
            if (sourcid % 2 == 0)
                q = _Context.SourceReading.Where(c => c.Fac_Id == id && c.TimeStamp.Date == date.Date && c.SecondarySourceId == sourcid).ToList();  // get all factories source reades belong to date and fac_id 
            else
                q = _Context.SourceReading.Where(c => c.Fac_Id == id && c.TimeStamp.Date == date.Date && c.PrimarySourceId == sourcid).ToList();  // get all factories source reades belong to date and fac_id 


            DashBoardServices ds = new DashBoardServices(_Context);

            DateTime CurrentHour = date.Date;
            DateTime nextHour = CurrentHour.AddHours(1);

            // nextHour = nextHour.AddHours(1);
            for (int i = 0; i < 24; i++)
            {
                CurrentAvg.Add(new List<List<decimal>>());
                VoltageAvg.Add(new List<List<decimal>>());
                PowerFactorAvg.Add(new List<List<decimal>>());
                HarmAvg.Add(new List<List<decimal>>());

                for (int j = 0; j < 3; j++)
                {
                    CurrentAvg[i].Add(new List<decimal>());
                    VoltageAvg[i].Add(new List<decimal>());
                    PowerFactorAvg[i].Add(new List<decimal>());
                    HarmAvg[i].Add(new List<decimal>());
                }
            }
            for (int i = 0; i < 24; i++)
            {
                foreach (var item in q)
                {

                    querySource temp = MappingToQuerySource(item);
                    if (temp.Timestamp >= CurrentHour && temp.Timestamp <= nextHour)
                    {
                        if (temp.Current1 != -1)
                            c1 += temp.Current1;
                        if (temp.Current2 != -1)
                            c2 += temp.Current2;
                        if (temp.Current3 != -1)
                            c3 += temp.Current3;
                        if (temp.Voltage1 != -1)
                            v1 += temp.Voltage1;
                        if (temp.Voltage2 != -1)
                            v2 += temp.Voltage2;
                        if (temp.Voltage3 != -1)
                            v3 += temp.Voltage3;
                        if (temp.PowerFactor1 != -1)
                            f1 += temp.PowerFactor1;
                        if (temp.PowerFactor2 != -1)
                            f2 += temp.PowerFactor2;
                        if (temp.PowerFactor3 != -1)
                            f3 += temp.PowerFactor3;
                        if (temp.HarmonicOrder1 != -1)
                            h1 += temp.HarmonicOrder1;
                        if (temp.HarmonicOrder2 != -1)
                            h2 += temp.HarmonicOrder2;
                        if (temp.HarmonicOrder3 != -1)
                            h3 += temp.HarmonicOrder3;
                        cnt++;
                    }
                }

                if (cnt != 0)
                {
                    CurrentAvg[i][0].Add(c1 / cnt);
                    CurrentAvg[i][1].Add(c2 / cnt);
                    CurrentAvg[i][2].Add(c3 / cnt);

                    VoltageAvg[i][0].Add(v1 / cnt);
                    VoltageAvg[i][1].Add(v2 / cnt);
                    VoltageAvg[i][2].Add(v3 / cnt);

                    PowerFactorAvg[i][0].Add(f1 / cnt);
                    PowerFactorAvg[i][1].Add(f2 / cnt);
                    PowerFactorAvg[i][2].Add(f3 / cnt);

                    HarmAvg[i][0].Add(h1 / cnt);
                    HarmAvg[i][1].Add(h2 / cnt);
                    HarmAvg[i][2].Add(h3 / cnt);

                }
                else
                {
                    decimal x = 0;
                    CurrentAvg[i][0].Add(x);
                    CurrentAvg[i][1].Add(x);
                    CurrentAvg[i][2].Add(x);

                    VoltageAvg[i][0].Add(x);
                    VoltageAvg[i][1].Add(x);
                    VoltageAvg[i][2].Add(x);

                    PowerFactorAvg[i][0].Add(x);
                    PowerFactorAvg[i][1].Add(x);
                    PowerFactorAvg[i][2].Add(x);

                    HarmAvg[i][0].Add(x);
                    HarmAvg[i][1].Add(x);
                    HarmAvg[i][2].Add(x);
                }
                c1 = 0; c2 = 0; c3 = 0; v1 = 0; v2 = 0; v3 = 0; f1 = 0; f2 = 0; f3 = 0; h1 = 0; h2 = 0; h3 = 0;
                CurrentHour = CurrentHour.AddHours(1);
                nextHour = nextHour.AddHours(1);

                cnt = 0;
            }
        }
        public querySource MappingToQuerySource(SourceReading src)
        {
            querySource api = new querySource
            {
                Current1 = src.Current1,
                Current2 = src.Current2,
                Current3 = src.Current3,
                frequency1 = src.frequency1,
                frequency2 = src.frequency2,
                frequency3 = src.frequency3,
                HarmonicOrder = src.HarmonicOrder,
                HarmonicOrder1 = src.HarmonicOrder1,
                HarmonicOrder2 = src.HarmonicOrder2,
                HarmonicOrder3 = src.HarmonicOrder3,
                mCurrent1 = src.mCurrent1,
                mCurrent2 = src.mCurrent2,
                mCurrent3 = src.mCurrent3,
                Power1 = src.Power1,
                Power2 = src.Power2,
                Power3 = src.Power3,
                PowerFactor1 = src.PowerFactor1,
                PowerFactor2 = src.PowerFactor2,
                PowerFactor3 = src.PowerFactor3,
                ReturnCurrent = src.ReturnCurrent,
                Timestamp = src.TimeStamp,
                Voltage1 = src.Voltage1,
                Voltage2 = src.Voltage2,
                Voltage3 = src.Voltage3,
                facid = src.Fac_Id

            };
            return api;

        }
        public List<SourceReading> getReadingPerHour(int phsnumber, int SourceID, DateTime time, int hour, int facid)
        {
            var ress = new List<SourceReading>();
            DateTime currenthour = time.Date;
            currenthour = currenthour.AddHours(hour);
            DateTime nexthour = currenthour;
            nexthour = nexthour.AddHours(1);
            if (SourceID % 2 == 0)//secondry source
            {
                ress = _Context.SourceReading.Where(s => s.SecondarySourceId == SourceID && s.TimeStamp >= currenthour && s.TimeStamp <= nexthour && s.Fac_Id == facid).ToList();
            }
            else // primary source 
            {
                ress = _Context.SourceReading.Where(s => s.PrimarySourceId == SourceID && s.TimeStamp >= currenthour && s.TimeStamp <= nexthour && s.Fac_Id == facid).ToList();
            }
            return ress;
        }
        //type = 0 load , 1 = function
        public PowerPeakViewModel PowerPeakPerDay(DateTime date, int type, int id)
        {

            ReportService rs = new ReportService(_Context);
            PowerPeakViewModel Result = new PowerPeakViewModel();
            var resSrc = new List<SourceReading>();
            var listprim = _Context.SourceReading.Select(s => s.PrimarySourceId).ToList();
            var listsec = _Context.SourceReading.Select(s => s.SecondarySourceId).ToList();
            var listload = _Context.Load.Select(s => s.Id).ToList();
            if (type == 1) //primary source
            {
                if (listprim.Contains(id))
                {
                    var primName = _Context.PrimarySource.Where(s => s.Id == id).Select(s => s.Name).SingleOrDefault();
                    var srcP1 = _Context.SourceReading.Where(s => s.PrimarySourceId == id && s.TimeStamp.Date == date.Date).Max(s => s.Power1);
                    var srcP2 = _Context.SourceReading.Where(s => s.PrimarySourceId == id && s.TimeStamp.Date == date.Date).Max(s => s.Power2);
                    var srcP3 = _Context.SourceReading.Where(s => s.PrimarySourceId == id && s.TimeStamp.Date == date.Date).Max(s => s.Power3);
                    Result.peakP1 = srcP1;
                    Result.peakP2 = srcP2;
                    Result.peakP3 = srcP3;
                    Result.Id = id;
                    Result.name = primName;
                    Result.dateP1 = date.Date;
                    Result.dateP2 = date.Date;
                    Result.dateP3 = date.Date;
                    Result.loadtype = "-1";
                }
            }
            else if (type == 2) //load
            {
                if (listload.Contains(id))
                {
                    var srcId = _Context.Load.Where(s => s.Id == id).Select(s => new { s.SourceId, s.name, s.LoadInfo }).SingleOrDefault();
                    var Ratingpower = _Context.Loadparameter.SingleOrDefault(s => s.Id == srcId.LoadInfo.Id);
                    if (srcId.SourceId % 2 == 0) //sec
                    {
                        if (listprim.Contains(srcId.SourceId))
                        {
                            var srcP1 = _Context.SourceReading.Where(s => s.SecondarySourceId == srcId.SourceId && s.TimeStamp.Date == date.Date).Max(s => s.Power1);
                            var srcP2 = _Context.SourceReading.Where(s => s.SecondarySourceId == srcId.SourceId && s.TimeStamp.Date == date.Date).Max(s => s.Power2);
                            var srcP3 = _Context.SourceReading.Where(s => s.SecondarySourceId == srcId.SourceId && s.TimeStamp.Date == date.Date).Max(s => s.Power3);
                            Result.peakP1 = srcP1;
                            Result.peakP2 = srcP2;
                            Result.peakP3 = srcP3;
                            Result.Id = id;
                            Result.name = srcId.name;
                            Result.dateP1 = date.Date;
                            Result.dateP2 = date.Date;
                            Result.dateP3 = date.Date;
                            Result.RatingPowerValue = Convert.ToDecimal(Ratingpower.Power);
                            Result.PeakToRatePercentage1 = (decimal)(Result.peakP1 / Result.RatingPowerValue);
                            Result.PeakToRatePercentage2 = (decimal)(Result.peakP2 / Result.RatingPowerValue);
                            Result.PeakToRatePercentage3 = (decimal)(Result.peakP3 / Result.RatingPowerValue);
                        }
                    }
                    else //prim
                    {
                        if (listprim.Contains(srcId.SourceId))
                        {
                            var srcP1 = _Context.SourceReading.Where(s => s.PrimarySourceId == srcId.SourceId && s.TimeStamp.Date == date.Date).Max(s => s.Power1);
                            var srcP2 = _Context.SourceReading.Where(s => s.PrimarySourceId == srcId.SourceId && s.TimeStamp.Date == date.Date).Max(s => s.Power2);
                            var srcP3 = _Context.SourceReading.Where(s => s.PrimarySourceId == srcId.SourceId && s.TimeStamp.Date == date.Date).Max(s => s.Power3);
                            Result.peakP1 = srcP1;
                            Result.peakP2 = srcP2;
                            Result.peakP3 = srcP3;
                            Result.Id = id;
                            Result.name = srcId.name;
                            Result.dateP1 = date.Date;
                            Result.dateP2 = date.Date;
                            Result.dateP3 = date.Date;
                            Result.RatingPowerValue = Convert.ToDecimal(Ratingpower.Power);
                            Result.PeakToRatePercentage1 = (decimal)(Result.peakP1 / Result.RatingPowerValue);
                            Result.PeakToRatePercentage2 = (decimal)(Result.peakP2 / Result.RatingPowerValue);
                            Result.PeakToRatePercentage3 = (decimal)(Result.peakP3 / Result.RatingPowerValue);
                        }
                    }
                    Result.loadtype = "1";
                }
            }
            return Result;
        }

        public List<string> GetSourceStatus()
        {
            DateTime Date = DateTime.Now;
            //String time = "2018-12-18 11:51:00";
            //DateTime Date = Convert.ToDateTime(time);
            var PrimariesIDs = _Context.SourceReading.Where(s => s.TimeStamp <= Date && s.TimeStamp >= Date.AddMinutes(-30) && s.PrimarySourceId != null).Select(s => s.PrimarySourceId).Distinct().ToList();
            var SecondariesIDs = _Context.SourceReading.Where(s => s.TimeStamp <= Date && s.TimeStamp >= Date.AddMinutes(-30) && s.SecondarySourceId != null).Select(s => s.SecondarySourceId).Distinct().ToList();
            List<String> result = new List<String>();
            int i = 0;
            for (i = 0; i < PrimariesIDs.Count; i++)
            {

                var prim = _Context.PrimarySource.DefaultIfEmpty().FirstOrDefault(p => p.Code == PrimariesIDs[i].ToString());
                var primCode = prim.Code;
                result.Insert(i, primCode);


            }
            int b = i;
            for (int c = 0; c < SecondariesIDs.Count; c++)
            {

                var sec = _Context.secondarySource.DefaultIfEmpty().FirstOrDefault(p => p.Code == SecondariesIDs[c].ToString());
                var secCode = sec.Code;
                result.Insert(i, secCode);

            }
            return result;

        }


        public List<Instant> GetInstants(int facId, int sort, int val, DateTime date)
        {
            List<Instant> res = new List<Instant>();
            LoadsServices ls = new LoadsServices(_Context);

            if (sort == 1)
            {
                var load = _Context.Load.FirstOrDefault(l => l.Id == val);
                var prim = _Context.PrimarySource.FirstOrDefault(p => p.Code == load.SourceId.ToString());

                var SourceReads = _Context.SourceReading.Where(s => s.PrimarySourceId == Convert.ToInt32(prim.Code) && s.TimeStamp.Date == date.Date).ToList();
                Instant tmp = new Instant();
                //tmp.CSvString = "Load Name ," +load.name + ",";
                foreach (var item in SourceReads)
                {

                    tmp.Cphase1.Add(item.Current1);
                    tmp.Cphase2.Add(item.Current2);
                    tmp.Cphase3.Add(item.Current3);
                    tmp.Vphase1.Add(item.Voltage1);
                    tmp.Vphase2.Add(item.Voltage2);
                    tmp.Vphase3.Add(item.Voltage3);
                    tmp.Pphase1.Add(item.PowerFactor1);
                    tmp.Pphase2.Add(item.PowerFactor2);
                    tmp.Pphase3.Add(item.PowerFactor3);
                    tmp.time.Add(Convert.ToString(item.TimeStamp));
                    tmp.SortVal = 0;
                    //tmp.CSvString +=
                    //    "Current phase1 ," + item.Current1 + "," +
                    //    "Current phase2 ," + item.Current2 + "," +
                    //    "Current phase3 ," + item.Current3 + "," +
                    //    "Voltage phase1 ," + item.Voltage1 + "," +
                    //    "Voltage phase2 ," + item.Voltage2 + "," +
                    //    "Voltage phase3 ," + item.Voltage3 + "," +
                    //    "PowerFactor phase1 ," + item.PowerFactor1 + "," +
                    //    "PowerFactor phase2 ," + item.PowerFactor2 + "," +
                    //    "PowerFactor phase3 ," + item.PowerFactor3 + "," +
                    //    "TimeStamp ," + Convert.ToString(item.TimeStamp) + ",";






                }
                tmp.Cnt = SourceReads.Count;
                tmp.loadName = load.name;
                res.Add(tmp);

            }
            else
            {

                List<LoadDataModel> loads = ls.GetAllLoads();
                foreach (var load in loads)
                {
                    if (load.Fac_Id != facId)
                    {
                        loads.Remove(load);
                    }
                    else
                    {
                        var prim = _Context.PrimarySource.FirstOrDefault(p => p.Code == load.PS_Id.ToString());

                        var SourceReads = _Context.SourceReading.Where(s => s.PrimarySourceId == Convert.ToInt32(prim.Code) && s.TimeStamp.Date == date.Date).ToList();
                        Instant tmp = new Instant();
                        //tmp.CSvString = "Load Name ," + load.name + ",";
                        foreach (var item in SourceReads)
                        {

                            tmp.Cphase1.Add(item.Current1);
                            tmp.Cphase2.Add(item.Current2);
                            tmp.Cphase3.Add(item.Current3);
                            tmp.Vphase1.Add(item.Voltage1);
                            tmp.Vphase2.Add(item.Voltage2);
                            tmp.Vphase3.Add(item.Voltage3);
                            tmp.Pphase1.Add(item.PowerFactor1);
                            tmp.Pphase2.Add(item.PowerFactor2);
                            tmp.Pphase3.Add(item.PowerFactor3);
                            tmp.time.Add(Convert.ToString(item.TimeStamp));
                            //    tmp.CSvString +=
                            //    "Current phase1 ," + item.Current1 + "," +
                            //"Current phase2 ," + item.Current2 + "," +
                            //"Current phase3 ," + item.Current3 + "," +
                            //"Voltage phase1 ," + item.Voltage1 + "," +
                            //"Voltage phase2 ," + item.Voltage2 + "," +
                            //"Voltage phase3 ," + item.Voltage3 + "," +
                            //"PowerFactor phase1 ," + item.PowerFactor1 + "," +
                            //"PowerFactor phase2 ," + item.PowerFactor2 + "," +
                            //"PowerFactor phase3 ," + item.PowerFactor3 + "," +
                            //"TimeStamp ," + Convert.ToString(item.TimeStamp) + ",";


                        }
                        if (val == 1)
                        {
                            tmp.SortVal = 1;
                        }
                        else if (val == 2)
                        {
                            tmp.SortVal = 2;
                        }
                        else
                        {
                            tmp.SortVal = 3;
                        }

                        tmp.Cnt = SourceReads.Count;
                        tmp.loadName = load.name;
                        res.Add(tmp);
                    }
                }



            }

            return res;
        }


        public List<HarmSt> GetHarmSts(int id, int sort, string val, DateTime date, int harm)
        {
            List<HarmSt> res = new List<HarmSt>();

            HarmSt haarm = new HarmSt();
            var load = _Context.Load.FirstOrDefault(l => l.Id == Convert.ToInt32(val));

            var src = _Context.SourceReading.Where(s => s.TimeStamp.Date == date && s.PrimarySourceId == load.SourceId & s.HarmonicOrder == harm).ToList();

            if (src != null && src.Count != 0)
            {
                haarm.Date = date.Date;
                haarm.HarmNum = harm;
                haarm.loadName = load.name;
                haarm.cnt = src.Count;
                foreach (var item in src)
                {
                    haarm.time.Add(item.TimeStamp.Date.ToString());

                    haarm.Hphase1.Add(item.HarmonicOrder1);
                    haarm.Hphase2.Add(item.HarmonicOrder2);
                    haarm.Hphase3.Add(item.HarmonicOrder3);



                }
                res.Add(haarm);
            }


            return res;
        }

        public List<EnergyC> GetEnergies(int id, int sort, string val, DateTime date)
        {
            List<EnergyC> Res = new List<EnergyC>();
            EnergyC result = new EnergyC();
            var th = Convert.ToDateTime("2019-10-15");
            if (date.Date <= th.Date)
            {
                var load = _Context.Load.FirstOrDefault(l => l.Id == Convert.ToInt32(val));
                var prim = _Context.PrimarySource.FirstOrDefault(p => p.Code == load.SourceId.ToString());
                decimal pAvg1 = 00.00m;
                decimal pAvg2 = 00.00m;
                decimal pAvg3 = 00.00m;
                var dt = date;
                var dF = date;

                while (dF != dt.AddDays(1))
                {
                    pAvg1 += _Context.SourceReading.Where(s => s.PrimarySourceId == load.SourceId && s.TimeStamp.Hour == dF.Hour && s.TimeStamp.Day == date.Day).DefaultIfEmpty().Average(s => s.Power1);
                    pAvg2 += _Context.SourceReading.Where(s => s.PrimarySourceId == load.SourceId && s.TimeStamp.Hour == dF.Hour && s.TimeStamp.Day == date.Day).DefaultIfEmpty().Average(s => s.Power2);
                    pAvg3 += _Context.SourceReading.Where(s => s.PrimarySourceId == load.SourceId && s.TimeStamp.Hour == dF.Hour && s.TimeStamp.Day == date.Day).DefaultIfEmpty().Average(s => s.Power3);

                    dF = dF.AddHours(1);
                    result.date = date.ToShortDateString();
                    result.loadName = load.name;
                }

                // result.date = date.ToShortDateString();
                result.Ephase1 = pAvg1;
                result.Ephase2 = pAvg2;
                result.Ephase3 = pAvg3;

                Res.Add(result);
            }
            else
            {

                var x = _Context.powAvg.FirstOrDefault(p => p.date.Date == date.Date && p.loadId == Convert.ToInt32(val));
                result.loadName = _Context.Load.FirstOrDefault(l => l.Id == Convert.ToInt32(val)).name;
                result.date = Convert.ToString(x.date.ToShortDateString());
                result.Ephase1 = x.pAvg1;
                result.Ephase2 = x.pAvg2;
                result.Ephase3 = x.pAvg3;

                Res.Add(result);
            }
            return Res;
        }
        public AveragePowerTodayViewModel AverageLoadPowerToday(int LoadId, DateTime Day)
        {
            DateTime Date = Day.Date;
            decimal p1avg = _Context.SourceReading.Where(r => r.PrimarySourceId == LoadId && r.TimeStamp.Date == Date).Average(r => r.Power1);
            decimal p2avg = _Context.SourceReading.Where(r => r.PrimarySourceId == LoadId && r.TimeStamp.Date == Date).Average(r => r.Power2);
            decimal p3avg = _Context.SourceReading.Where(r => r.PrimarySourceId == LoadId && r.TimeStamp.Date == Date).Average(r => r.Power3);
            AveragePowerTodayViewModel model = new AveragePowerTodayViewModel
            {
                LoadPower1Average = p1avg,
                LoadPower2Average = p2avg,
                LoadPower3Average = p3avg,
                LoadPowerAverage = (p1avg + p2avg + p3avg) / 3
            };
            return model;
        }
    }
}