using Microsoft.EntityFrameworkCore;
using SmartPower.Controllers.Domin;
using SmartPower.DataContext;
using SmartPower.Models;
using SmartPower.Models.report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartPower.Services
{
    public class PrimarySourceSerivce
    {
        private readonly PowerDbContext _Context;

        public PrimarySourceSerivce(PowerDbContext _context)
        {
            _Context = _context;
        }
        //=================================================
        //Getters
        //get all
        public List<PrimarySourceDataModel> GetAllPrimarySources()
        {
            var Sources = _Context.PrimarySource.ToList();
            //mapping
            var SourceDM = new List<PrimarySourceDataModel>();
            if (Sources.Count != 0)
            { SourceDM = MappingToPSDMSimple(Sources); }
            return SourceDM;
        }




        public List<PrimarySource> GetprimarySourcebyfacid(int factoryid)  //type of distination 
        {

            List<PrimarySource> x = _Context.PrimarySource.Where(s => s.FactoryId == factoryid).ToList();
            return x;
        }

        public List<PrimarySourceDataModel> GetAllPrimarySourcesbyfacId(int Id)
        {
            var prim = _Context.PrimarySource.Where(s => s.FactoryId == Id).ToList();
            return MappingToPSDMSimple(prim); 
        }
        //Get All For Factory
        public List<PrimarySourceDataModel> GetPrimarySourcesByFactoryId(int FacId)
        {
            var Query = _Context.PrimarySource.Where(p => p.FactoryId == FacId).ToList();
            //Mapping 
            var Results = MappingToPSDMSimple(Query);
            return Results;
        }

        //Get One PrimarySource By it's Id
        public PrimarySourceDataModel GetPrimarySourceSimple(int? Id)
        {
           
            var Query = _Context.PrimarySource.SingleOrDefault(p => p.Code == Id.ToString());
            //Mapping
            var Results = MappingToPrimarySourceDataModel(Query);
            return Results;
        }
        public PrimarySourceDataModel GetPrimarySourceSimpleByCode(int? Id)
        {
            var code = Convert.ToString(Id);
            var Query = _Context.PrimarySource.SingleOrDefault(p => p.Code == code);
            //Mapping
            var Results = MappingToPrimarySourceDataModel(Query);
            return Results;
        }

        public PrimarySource GetPrimaryByName(string name , int id )
        {
            var Query = _Context.PrimarySource.SingleOrDefault(p => p.Name == name && p.FactoryId == id);
            return Query;
        }

        public PrimarySource GetPrimarySourceFromDBByCode(int? Id)
        {

            var obj = _Context.PrimarySource.SingleOrDefault(p => p.Code == Id.ToString());
            return obj;
        }

        public PrimarySource GetPrimarySourceFromDB(int? Id)
        {

            var obj = _Context.PrimarySource.SingleOrDefault(p => p.Code == Id.ToString());
            return obj;
        }
        //Get PrimarySource with All Data 
        public PrimarySourceDataModel GetPrimarySourceWithAllData(int Id)
        {
            var Query =  _Context.PrimarySource.Include(p => p.secondarySources).SingleOrDefault(p=>p.Code == Id.ToString());
            //mapping 
            var Result = MappingToPrimarySourceDataModelFullData(Query);
            return Result;
        }
        //================================================================
        //Setters 
        //Create PrimarySource 
        public async Task<bool> CreatePrimarySourceAsync(PrimarySourceDataModel obj)
        {
            //mapping 
            var PriamrySource = MappingToPrimarySource(obj);
            //Adding To Db 
             _Context.PrimarySource.Add(PriamrySource);
            await _Context.SaveChangesAsync();
            var value = _Context.PrimarySource.Last();
            //value.Code = "" + value.Id;
            _Context.SaveChanges();



            
            // PEAK POWER TABLE
            powerPeak peak = new powerPeak()
            {
                primarySourceId = Convert.ToInt16(value.Code),
                peakP1 = 0 , 
                peakP2 = 0 , 
                peakP3 = 0,
                
            };
            _Context.powerPeak.Add(peak);
            _Context.SaveChanges();
            return true;
        }
        //Delete PrimarySource 
        public async Task<bool> Delete(int? Id)
        {
            var  Obj = GetPrimarySourceFromDB(Id);
            if(Obj != null)
            {
                
                var sourcavg = _Context.SourceAvgs.Where(s => s.PrimarySourceId == Id).ToList();
                foreach (var i in sourcavg)
                    _Context.SourceAvgs.Remove(i);

                var del = _Context.PrimarySource.Include(s => s.secondarySources).Include(s=>s.SourceLogs).Where(s => s.Code == Id.ToString());

                foreach (var i in del)
                {
                    foreach (var k in i.SourceLogs)
                        _Context.SourceReading.Remove(k);
                    foreach (var k in i.secondarySources)
                        _Context.secondarySource.Remove(k);

                    _Context.PrimarySource.Remove(i);

                }

                var allloads = _Context.Load.Where(s => s.SourceId == Id).ToList();
                foreach (var i in allloads)
                    _Context.Load.Remove(i);

                var x = _Context.PhasesConnection.Where(p => p.SourceId == Id).ToList() ;
                foreach (var i in x) _Context.PhasesConnection.Remove(i);
                var y = _Context.powerPeak.Where(p => p.primarySourceId == Id).ToList();
                foreach (var i in y)  _Context.powerPeak.Remove(i);
                var avgs = _Context.PowerAvg.Where(a => a.primarySourceId == Id).ToList();
                if (avgs.Count != 0)
                {
                    foreach (var avg in avgs)
                    {
                        _Context.PowerAvg.Remove(avg);
                    }
                }

                await _Context.SaveChangesAsync();


                return true;
            }
            return false;
        }

        //=============================================================
        //mapping profile
        //mapping List<PS> to List<PSDM>
        public List<PrimarySourceDataModel> MappingToPSDMSimple(List<PrimarySource> ListObjs)
        {
            List<PrimarySourceDataModel> Results = new List<PrimarySourceDataModel>();
            foreach (var Item in ListObjs)
            {
                Results.Add(new PrimarySourceDataModel()
                {
                    Id = Item.Id,
                    Name = Item.Name,
                    Code = Item.Code,
                    FacName = GetFactoryName(Item.FactoryId),
                    FacId = Item.FactoryId,
                    DesignValue = Item.DesignValue,
                    MaxCurrent = Item.MaxCurrent,
                    Topology = Item.Topology,
                    Type = Item.Type

                });
            }
            
            return Results;
        }
        //Mapping Single object To PSDM
        public PrimarySourceDataModel MappingToPrimarySourceDataModel(PrimarySource obj)
        {
            PrimarySourceDataModel ReturnObj = new PrimarySourceDataModel()
            {
                Id = obj.Id,
                Code = obj.Code,
                Name = obj.Name,
                FacId = obj.FactoryId,
                FacName = GetFactoryName(obj.FactoryId),
                DesignValue = obj.DesignValue,
                MaxCurrent = obj.MaxCurrent,
                Topology = obj.Topology,
                Type = obj.Type
            };
            return ReturnObj;
        }
        //Mapping Single To PrimarySource 
        public PrimarySource MappingToPrimarySource(PrimarySourceDataModel obj)
        {
            PrimarySource result = new PrimarySource()
            {
               // Id = obj.Id,
                Name = obj.Name,
                Code = obj.Code,
                FactoryId = obj.FacId,
                DesignValue = obj.DesignValue,  
                MaxCurrent = obj.MaxCurrent, 
                Topology =  obj.Topology,  
                Type =  obj.Type 
            };
            return result;
        }
        public PrimarySource MappingToPrimarySourceWithId(PrimarySourceDataModel obj)
        {
            PrimarySource result = new PrimarySource()
            {
                Id = obj.Id,
                Name = obj.Name,
                Code = obj.Code,
                FactoryId = obj.FacId,
                DesignValue = obj.DesignValue,
                MaxCurrent = obj.MaxCurrent,
                Topology = obj.Topology,
                Type = obj.Type
            };
            return result;
        }
        //Full Data Mapping
        public PrimarySourceDataModel MappingToPrimarySourceDataModelFullData(PrimarySource obj)
        {
            PrimarySourceDataModel ReturnObj = new PrimarySourceDataModel()
            {
                Id = obj.Id,
                Code = obj.Code,
                Name = obj.Name,
                FacId = obj.FactoryId,
                FacName = GetFactoryName(obj.FactoryId),
                DesignValue = obj.DesignValue,
                MaxCurrent = obj.MaxCurrent,
                Topology = obj.Topology,
                Type = obj.Type
            };
            foreach(var item in obj.secondarySources)
            {
                ReturnObj.secondarySources.Add(new SecoundrySouresDataModelSim() {
                    Id = item.Id,
                    Code = item.Code,
                    Name = item.Name,
                    PS_Id = obj.Id,
                    PirmarySourceName = obj.Name
                });
            }
            return ReturnObj;
        }

        //==============================================================================
        //External Functions 
        //Get Factory Name
        public string GetFactoryName (int Id)
        {
            string name = " ";
            var Query = _Context.Factory.SingleOrDefault(f => f.Id == Id);
            if(Query != null)
            {
                name = Query.Name;
            }
            return name;
        }
        public int GetFactoryId(int? Id)
        {
            int FacId = 0;
            var Query = _Context.PrimarySource.SingleOrDefault(p => p.Code == Id.ToString());
            FacId = Query.FactoryId;
            return FacId;
        }
        public string GetPrimarySourceName(int? id)
        {
            string Result = "  ";
            if(id == null)
            {
                return Result;
            }
            else
            {
                Result = _Context.PrimarySource.SingleOrDefault(p => p.Code == id.ToString()).Name;
                return Result;
            }
        }

        public async Task<bool> EditPrimarySource(PrimarySourceDataModel obj)
        {
  
            PrimarySource result = new PrimarySource()
            {
                Id = obj.Id,
                Name = obj.Name,         
                DesignValue = obj.DesignValue,
                MaxCurrent = obj.MaxCurrent,
                Topology = obj.Topology,
                FactoryId =  obj.FacId,
                Code = obj.Code, 
                Type = obj.Type
            };
            _Context.PrimarySource.Update(result);
            await _Context.SaveChangesAsync();
            return true; 

        }
        public async Task DeleteAllPrimaryAsync(int facid)
        {
            var lst = _Context.PrimarySource.Where(s=>s.FactoryId == facid);
            bool ch;
            foreach (var x in lst)
                ch = await Delete(x.Id);               
        }

    }
}
