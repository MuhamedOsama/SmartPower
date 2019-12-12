using SmartPower.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmartPower.Domin;
using SmartPower.Models; 

namespace SmartPower.Services
{
    public class DashBoardServices
    {
        private readonly PowerDbContext _Context;
        public DashBoardServices(PowerDbContext cont)
        {
            _Context = cont;  
        }

        public void GetdateOfSourcesOfPrimaries(int fac_id ,  DateTime date ,  ref DashboardViewModel res)
        {
            var primaries = _Context.PrimarySource.Where(s=>s.FactoryId  == fac_id).ToList();
            foreach (var prim  in primaries )
            {
                var lstread = _Context.SourceReading.LastOrDefault(s => s.PrimarySourceId == prim.Id && s.TimeStamp.Date ==  date.Date);
                if (lstread != null)
                {
                    res.primaries[prim.Id] = MappingtoApiDate(lstread, prim.Name);
                }
                else
                {
                    res.primaries[prim.Id] = new ApiData();
                    res.primaries[prim.Id].name = prim.Name; 
                }
            }
            var Allload = _Context.Load.ToList();
            List<Load> loads = new List<Load>();
            foreach (var ld in Allload)
            {
                var code = ld.SourceId.ToString();
                if (ld.SourceId % 2 != 0)
                {
                    
                    int fid = _Context.PrimarySource.SingleOrDefault(s => s.Code == code).FactoryId;
                    if (fid == fac_id)
                        loads.Add(ld);

                }
                else if (ld.SourceId % 2 == 0)
                {

                    int fid = _Context.secondarySource.SingleOrDefault(s => s.Code == code).Fac_Id;
                    if (fid == fac_id)
                        loads.Add(ld);
                }
            }
            foreach (var ld in loads)
            {
                SourceReading lstread = new SourceReading();
                if (ld.SourceId % 2 != 0)
                    lstread = _Context.SourceReading.LastOrDefault(s => s.PrimarySourceId == ld.SourceId && s.TimeStamp.Date == date.Date);
                else if (ld.SourceId % 2 == 0)
                    lstread = _Context.SourceReading.LastOrDefault(s => s.SecondarySourceId == ld.SourceId && s.TimeStamp.Date == date.Date);
                if (lstread != null)
                {
                    res.CountOfActiveLoads++;
                    if (ld.PhaseType == "1")
                    {
                        if (lstread.Current1 != 0) res.CoutOfLifeLoads++;
                    }
                    if (ld.PhaseType == "3")
                    {
                        if (lstread.Current1 != 0 && lstread.Current2 != 0 && lstread.Current3 != 0) res.CoutOfLifeLoads++;
                    }
                    res.Loads[ld.Id] = MappingtoApiDate(lstread, ld.name);
                }
                else
                {
                    res.CountOfActiveLoads++;
                    res.Loads[ld.Id] = new ApiData();
                    res.Loads[ld.Id].name = ld.name;
                }

            }

        }
        public void GetdateOfSourcesOfLoads(int fac_id, DateTime date, ref DashboardViewModel res)
        {
            var Allload = _Context.Load.ToList();
            List<Load> loads = new List<Load>();
            
            foreach (var ld in Allload)
            {
                var code = ld.SourceId.ToString();
                if (ld.SourceId % 2 != 0)
                {

                    int fid = _Context.PrimarySource.SingleOrDefault(s => s.Code == code).FactoryId;
                    if (fid == fac_id)
                        loads.Add(ld);

                }
                else if (ld.SourceId % 2 == 0)
                {

                    int fid = _Context.secondarySource.SingleOrDefault(s => s.Code == code).Fac_Id;
                    if (fid == fac_id)
                        loads.Add(ld);
                }
            }
            foreach (var ld in loads)
            {
                SourceReading lstread = new SourceReading();
                if (ld.SourceId % 2 != 0)
                    lstread = _Context.SourceReading.LastOrDefault(s => s.PrimarySourceId == ld.SourceId && s.TimeStamp.Date == date.Date);
                else if (ld.SourceId % 2 == 0)
                    lstread = _Context.SourceReading.LastOrDefault(s => s.SecondarySourceId == ld.SourceId && s.TimeStamp.Date == date.Date);
                if (lstread != null)
                {
                    res.CountOfActiveLoads++;
                    if (ld.PhaseType == "1")
                    {
                        if (lstread.Current1 != 0) res.CoutOfLifeLoads++;
                    }
                    if (ld.PhaseType == "3")
                    {
                        if (lstread.Current1 != 0 && lstread.Current2 != 0 && lstread.Current3 != 0) res.CoutOfLifeLoads++;
                    }
                    res.Loads[ld.Id] = MappingtoApiDate(lstread, ld.name);
                    ReportService rs = new ReportService(_Context);
                    res.powerpeak.Add(rs.PowerPeakBySourceID(ld.Id,2));
                    res.loadTimeratio.Add(rs.LoadTimeDownRatio(ld.Id, date, date));
                    res.EnergyConsumed[ld.Id] = rs.GetEnergyConsumedForOneLoad(ld, date.ToString("yyyy-MM-dd"), fac_id); 
                    // var timeratio = rs.LoadTimeDownRatio(ld.Id, date, date);
                    //var powerpeak = rs.PowerPeakBySourceID(ld.Id, 2); 
        
                }
                else
                {
                    res.CountOfActiveLoads++;
                    res.Loads[ld.Id] = new ApiData();
                    res.Loads[ld.Id].name = ld.name;
                }

            }
        }


        public ApiData MappingtoApiDate(SourceReading src,string Name )
        {
            ApiData api = new ApiData
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
                TimeStamp = src.TimeStamp,
                Voltage1 = src.Voltage1,
                Voltage2 = src.Voltage2,
                Voltage3 = src.Voltage3,
                name = Name
            };
            return api;  

        }

    

    }
}
