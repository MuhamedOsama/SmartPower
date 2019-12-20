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
    public class SecoundrySourceService
    {
        private readonly PowerDbContext _Context;

        public SecoundrySourceService(PowerDbContext _context)
        {
            _Context = _context;
        }

        //=================================================
        //Getters
        //get all
        public List<SecoundrySouresDataModelSim> GetAllSecoundrySources()
        {
            var Sources = _Context.secondarySource.ToList();
            //mapping
            var SourceDM = new List<SecoundrySouresDataModelSim>();
            if (Sources.Count != 0)
            {
                 SourceDM = MappingToPSDMSimple(Sources);
            }
            return SourceDM;
        }
        //Get All For Factory
        public List<SecoundrySouresDataModelSim> GetSecSourceByPimSourceId(int P_id)
        {
            var Query = _Context.secondarySource.Where(p => p.PrimarySourceId == P_id).ToList();
            //Mapping 
            var Results = MappingToPSDMSimple(Query);
            return Results;
        }

        //Get One PrimarySource By it's Code
        public SecoundrySouresDataModelSim GetPrimarySourceSimpleBYCode(int? Id)
        {
            var Query = _Context.secondarySource.SingleOrDefault(p => p.Code == Id.ToString());
            //Mapping
            var Results = MappingToPrimarySourceDataModel(Query);
            return Results;
        }


        //Get One PrimarySource By it's Id
        public SecoundrySouresDataModelSim GetPrimarySourceSimple(int? Id)
        {
            var Query = _Context.secondarySource.SingleOrDefault(p => p.Id == Id);
            //Mapping
            var Results = MappingToPrimarySourceDataModel(Query);
            return Results;
        }
        
       public secondarySource GetSecondaryByName(string name, int id)
        {
            var Query = _Context.secondarySource.SingleOrDefault(p => p.Name == name && p.PrimarySourceId == id);
            return Query;
        }

        public secondarySource GetSecoundrySourceFromDBByCode(int? Id)
        {
            var obj = _Context.secondarySource.SingleOrDefault(p => p.Code == Id.ToString());
            return obj;
        }

        public secondarySource GetSecoundrySourceFromDB(int? Id)
        {
            var obj = _Context.secondarySource.SingleOrDefault(p => p.Id == Id);
            return obj;
        }
        //================================================================
        //Setters 
        //Create Secondary 
        public async Task<bool> CreateSecoundrySourceAsync(SecoundrySouresDataModelSim obj)
        {
            //mapping 
            var secondarySource = MappingToPrimarySource(obj);
            //Adding To Db 
            _Context.secondarySource.Add(secondarySource);
            await _Context.SaveChangesAsync();
            var value = _Context.secondarySource.Last();
            //value.Code = "" + value.Id;
            _Context.SaveChanges();


            // create secondary source phase
            PhasesConnectionService phs = new PhasesConnectionService(_Context);
            phs.Create(value.Id, value.Type);
        
            // PEAK POWER TABLE
            powerPeak peak = new powerPeak()
            {
                secondrySourceId = Convert.ToInt16(value.Code),
                peakP1 = 0,
                peakP2 = 0,
                peakP3 = 0,

            };
            _Context.powerPeak.Add(peak);
            _Context.SaveChanges();


            //connect to primary source phases 
            PhasesConnection phase = new PhasesConnection();
            phase.SourceId = obj.PS_Id;
            phase.SourceType = new PrimarySourceSerivce(_Context).GetPrimarySourceFromDBByCode(obj.PS_Id).Type;

            if (obj.Type == "1")
            {
                if (obj.dN1 == 1)
                {
                    phase.dN1 = 1;
                    phase.sN1 = "s" + value.Id;
                }
                else if (obj.dN1 == 2)
                {
                    phase.dN2 = 1;
                    phase.sN2 = "s" + value.Id;
                }
                else
                {
                    phase.dN3 = 1;
                    phase.sN3 = "s" + value.Id;
                }

            }
            else
            {

                if (obj.dN1 == 1)
                {
                    phase.dN1 = 1;
                    phase.sN1 = "s" + value.Id;
                }
                else if (obj.dN1 == 2)
                {
                    phase.dN2 = 1;
                    phase.sN2 = "s" + value.Id;
                }
                else
                {
                    phase.dN3 = 1;
                    phase.sN3 = "s" + value.Id;
                }


                if (obj.dN2 == 1)
                {
                    phase.dN1 = 2;
                    phase.sN1 = "s" + value.Id;
                }
                else if (obj.dN2 == 2)
                {
                    phase.dN2 = 2;
                    phase.sN2 = "s" + value.Id;
                }
                else
                {
                    phase.dN2 = 2;
                    phase.sN3 = "s" + value.Id;
                }


                if (obj.dN3 == 1)
                {
                    phase.dN1 = 3;
                    phase.sN1 = "s" + value.Id;
                }
                else if (obj.dN3 == 2)
                {
                    phase.dN2 = 3;
                    phase.sN2 = "s" + value.Id;
                }
                else
                {
                    phase.dN3 = 3;
                    phase.sN3 = "s" + value.Id;
                }

            }

            _Context.PhasesConnection.Add(phase);
            await _Context.SaveChangesAsync();  
            return true;
        }
        //Delete PrimarySource 
        public async Task<bool> Delete(int? Id)
        {
            var Obj = GetSecoundrySourceFromDBByCode(Id);
            if (Obj != null)
            {
                var sourcavg = _Context.SourceAvgs.Where(s => s.SecondarySourceId == Id).ToList();
                foreach (var i in sourcavg)
                    _Context.SourceAvgs.Remove(i);

                var x = _Context.secondarySource.Include(s => s.SourceLogs).Where(s => s.Code == Id.ToString()).ToList(); 
                foreach(var i in x)
                {
                    foreach (var k in i.SourceLogs)
                        _Context.SourceReading.Remove(k);
                 
                    _Context.secondarySource.Remove(i);
                }

                var allloads = _Context.Load.Where(s => s.SourceId == Id).ToList();
                foreach (var i in allloads)
                    _Context.Load.Remove(i); 

             
                var x1 = _Context.PhasesConnection.Where(p => p.SourceId == Id).ToList();
                foreach (var i in x1)
                    _Context.PhasesConnection.Remove(i);

                var y = _Context.powerPeak.Where(p => p.secondrySourceId == Id).ToList();
                foreach (var i in y)
                    _Context.powerPeak.Remove(i);
                       
                var avgs = _Context.PowerAvg.Where(a => a.secondrySourceId == Id).ToList();
                if (avgs.Count!= 0)
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

        public async Task DeleteAllSecondaryAsync(int facid)
        {
            var lst = _Context.secondarySource.Where(s => s.Fac_Id == facid);
            bool ch;
            foreach (var x in lst)
                ch = await Delete(x.Id);
        }

        //=============================================================
        //mapping profile
        //mapping List<SS> to List<SSDM>
        public List<SecoundrySouresDataModelSim> MappingToPSDMSimple(List<secondarySource> ListObjs)
        {
            List<SecoundrySouresDataModelSim> Results = new List<SecoundrySouresDataModelSim>();
            foreach (var Item in ListObjs)
            {
                string fname = ((Factory)_Context.Factory.SingleOrDefault(q => q.Id == Item.Fac_Id)).Name;
                Results.Add(new SecoundrySouresDataModelSim()
                {
                    Id = Item.Id,
                    Name = Item.Name,
                    Code = Item.Code,
                    PirmarySourceName = GetPrimaryName(Item.PrimarySourceId),
                    PS_Id = Item.PrimarySourceId,
                    FacName = fname ,
                    DesignValue = Item.DesignValue,  
                    MaxCurrent =  Item.MaxCurrent, 
                    Topology =  Item.Topology,
                    Type = Item.Type,
                    FacId = Item.Fac_Id
            });
           }
            return Results;
        }
        //Mapping Single object To SSDM
        public SecoundrySouresDataModelSim MappingToPrimarySourceDataModel(secondarySource obj)
        {
            SecoundrySouresDataModelSim ReturnObj = new SecoundrySouresDataModelSim()
            {
                Id = obj.Id,
                Code = obj.Code,
                Name = obj.Name,
                PS_Id = obj.PrimarySourceId,
                PirmarySourceName = GetPrimaryName(obj.PrimarySourceId)
            };
            return ReturnObj;
        }
        //Mapping Single To Secoundry 
        public secondarySource MappingToPrimarySource(SecoundrySouresDataModelSim obj)
        {
            secondarySource result = new secondarySource()
            {
                // Id = obj.Id,
                Name = obj.Name,
                Code = obj.Code,
                PrimarySourceId = obj.PS_Id,
                DesignValue = obj.DesignValue,
                MaxCurrent = obj.MaxCurrent,
                Topology = obj.Topology,
                Type = obj.Type ,  
                Fac_Id =  obj.FacId
            };
            return result;
        }
        //mapping to Model + Id {{For Create}}
        public secondarySource MappingToSecoundrySourceWithId(SecoundrySouresDataModelSim obj)
        {
            secondarySource result = new secondarySource()
            {
                Id = obj.Id,
                Name = obj.Name,
                Code = obj.Code,
                PrimarySourceId = obj.PS_Id
            };
            return result;
        }
        public SecoundrySouresDataModelSim MappingToSecoundrySourceDataModel(secondarySource obj)
        {
            SecoundrySouresDataModelSim res = new SecoundrySouresDataModelSim
            {
                Topology = obj.Topology,
                MaxCurrent = obj.MaxCurrent,
                DesignValue = obj.DesignValue,
                Code = obj.Code,
                PS_Id = obj.PrimarySourceId,
                FacId = obj.Fac_Id,
                Type = obj.Type,
                Id = obj.Id,
                 Name =  obj.Name
            };
            return res;  
            
        }
        public List<secondarySource> GetAllSecondarySourcesbyfacId(int factoryid)  //type of distination 
        {

            List<secondarySource> x = _Context.secondarySource.Where(s => s.Fac_Id == factoryid).ToList();
            return x;
        }
        public async Task<bool> EditSecondarySource(SecoundrySouresDataModelSim obj)
        {

            secondarySource result = new secondarySource()
            {
                Id = obj.Id,
                Name = obj.Name,
                DesignValue = obj.DesignValue,
                MaxCurrent = obj.MaxCurrent,
                Topology = obj.Topology,
                Fac_Id = obj.FacId,
                Code = obj.Code,
                Type = obj.Type, 
                PrimarySourceId =  obj.PS_Id, 
               
                
            };
            _Context.secondarySource.Update(result);
            await _Context.SaveChangesAsync();
            return true;

        }

        public List<SecoundrySouresDataModelSim> GetSpecificSecondary(int facid , int primary)
        {
            List<SecoundrySouresDataModelSim> allsec = MappingToPSDMSimple(_Context.secondarySource.Where(s => s.Fac_Id == facid).ToList());
            List<SecoundrySouresDataModelSim> result = new List<SecoundrySouresDataModelSim>(); 
                    if ( primary != -1)
                                  {

                            foreach (var item in allsec)
                            {
                                if (item.PS_Id == primary) result.Add(item);
                            }
                            return result;
                         }                         
            return allsec; 
        }


        //==============================================================================
        //External Functions 
        public string GetPrimaryName(int P_Id)
        {
            string name = " ";
            
            var Query = _Context.PrimarySource.SingleOrDefault(p => p.Code == P_Id.ToString());
            if(Query != null) { 
            name = Query.Name;
            }
            return name;
        }
        public int GetPrimarySourceId(int Id)
        {
            int P_Id;
            var Query = _Context.secondarySource.SingleOrDefault(s => s.Code == Id.ToString());
            P_Id = Query.PrimarySourceId;
            return P_Id;
        }
        public string GetSecoundrySourceName(int? id)
        {
            string Result = "  ";
            if (id == null)
            {
                return Result;
            }
            else
            {
                Result = _Context.secondarySource.SingleOrDefault(p => p.Code == id.ToString()).Name;
                return Result;
            }
        }

        public SecondrySourceWithFullLoads GetSecondaryLoads(int Id)
        {
            SecondrySourceWithFullLoads Result = new SecondrySourceWithFullLoads();
            var Query = _Context.secondarySource.Include(f => f.Loads).SingleOrDefault(f => f.Code == Id.ToString());
            if (Query != null)
            {
                Result =  MappingToFactoryFullDataAsync(Query);
            }
            return Result;
        }

        public SecondrySourceWithFullLoads MappingToFactoryFullDataAsync(secondarySource obj)
        {
            SecondrySourceWithFullLoads MappedObj = new SecondrySourceWithFullLoads()
            {
               Id=obj.Id ,
               Code = obj.Code , 
               Name = obj.Name,
               PrimarySourceId = obj.PrimarySourceId, 
               PrimarySource = obj.PrimarySource                   
            };

            foreach (var item in obj.Loads)
            {
                MappedObj.Loads.Add(new Load()
                {
                    Id = item.Id,
                    Connection = item.Connection , 
                    Function =  item.Function ,  
                    code = item.code,  
                    
                });
            }
            return MappedObj;
        }

        /********************************************************************************/ 
                    /* Create From  home */ 
                      
        public async  Task<bool>  CreateSecondry_Home(SecoundrySouresDataModelSim obj)
        {
            secondarySource result = new secondarySource()
            {
           
                Name = obj.Name,
                Code = obj.Code,
                DesignValue = obj.DesignValue,
                MaxCurrent = obj.MaxCurrent,
                Topology = obj.Topology,
                Type = obj.Type,
                
            };
            _Context.secondarySource.Add(result);
            await _Context.SaveChangesAsync();
            var value = _Context.secondarySource.Last();
           // value.Code = "" + value.Id;
            _Context.SaveChanges();
            return true;
        }

    }
}
