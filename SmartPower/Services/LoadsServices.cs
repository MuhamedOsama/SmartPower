using Microsoft.EntityFrameworkCore;
using SmartPower.Controllers.Domin;
using SmartPower.DataContext;
using SmartPower.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartPower.Services
{
    public class LoadsServices
    {
        private readonly PowerDbContext _con;
        public LoadsServices(PowerDbContext _con)
        {
            this._con = _con;   
        }
        public async Task<bool> CreateLoadAsync(LoadDataModel obj)
        {
            //mapping 
            var load = MappingtoLoad(obj);
            //Adding To Db 
            _con.Load.Add(load);
            await _con.SaveChangesAsync();     
            Loadparameter par = new Loadparameter
            {              
                Power = obj.Power,                
                PowerFactor = obj.PowerFactor,
                RatingCurrent = obj.RatingCurrent,
                RatingTemp= obj.RatingTemp,
                RatingVoltage=obj.RatingVoltage,
                Type = obj.Type,                    
            };
            _con.Loadparameter.Add(par);

            var value = _con.Load.Last();
            value.code = value.Id;
            value.LoadInfo = par;
            _con.SaveChanges();

            // lw mtwsl b primary
            if (obj.SourceId %2 != 0)
            {
                PhasesConnection phase = new PhasesConnection();
                phase.SourceId = obj.SourceId;
                phase.SourceType = new PrimarySourceSerivce(_con).GetPrimarySourceFromDBByCode(obj.SourceId).Type;

                if (obj.Type == "1")
                {
                    if (obj.dN1 == 1)
                    {
                        phase.dN1 = 1;
                        phase.sN1 = "l" + value.Id;
                    }
                    else if (obj.dN1 == 2)
                    {
                        phase.dN2 = 1;
                        phase.sN2 = "l" + value.Id;
                    }
                    else
                    {
                        phase.dN3 = 1;
                        phase.sN3 = "l" + value.Id;
                    }

                }
                else
                {

                    if (obj.dN1 == 1)
                    {
                        phase.dN1 = 1;
                        phase.sN1 = "l" + value.Id;
                    }
                    else if (obj.dN1 == 2)
                    {
                        phase.dN2 = 1;
                        phase.sN2 = "l" + value.Id;
                    }
                    else
                    {
                        phase.dN3 = 1;
                        phase.sN3 = "l" + value.Id;
                    }


                    if (obj.dN2 == 1)
                    {
                        phase.dN1 = 2;
                        phase.sN1 = "l" + value.Id;
                    }
                    else if (obj.dN2 == 2)
                    {
                        phase.dN2 = 2;
                        phase.sN2 = "l" + value.Id;
                    }
                    else
                    {
                        phase.dN2 = 2;
                        phase.sN3 = "l" + value.Id;
                    }


                    if (obj.dN3 == 1)
                    {
                        phase.dN1 = 3;
                        phase.sN1 = "l" + value.Id;
                    }
                    else if (obj.dN3 == 2)
                    {
                        phase.dN2 = 3;
                        phase.sN2 = "l" + value.Id;
                    }
                    else
                    {
                        phase.dN3 = 3;
                        phase.sN3 = "l" + value.Id;
                    }

                }

                _con.PhasesConnection.Add(phase);
                await _con.SaveChangesAsync();
            }
            // lw mtwsl bsec
            else
            {
                //connect to secondry source phases 
                PhasesConnection phase = new PhasesConnectionService(_con).getPhaseBySourceId(obj.SourceId);
                if (obj.PhaseType == "1")
                {
                    if (obj.dN1 == 1)
                    {
                        phase.dN1 = 1;
                        phase.sN1 = "l" + value.Id;
                    }
                    else if (obj.dN1 == 2)
                    {
                        phase.dN2 = 1;
                        phase.sN2 = "l" + value.Id;
                    }
                    else
                    {
                        phase.dN3 = 1;
                        phase.sN3 = "l" + value.Id;
                    }
                }
                else
                {
                    if (obj.dN1 == 1)
                    {
                        phase.dN1 = 1;
                        phase.sN1 = "l" + value.Id;
                    }
                    else if (obj.dN1 == 2)
                    {
                        phase.dN2 = 1;
                        phase.sN2 = "l" + value.Id;
                    }
                    else
                    {
                        phase.dN3 = 1;
                        phase.sN3 = "l" + value.Id;
                    }


                    if (obj.dN2 == 1)
                    {
                        phase.dN1 = 2;
                        phase.sN1 = "l" + value.Id;
                    }
                    else if (obj.dN2 == 2)
                    {
                        phase.dN2 = 2;
                        phase.sN2 = "l" + value.Id;
                    }
                    else
                    {
                        phase.dN2 = 2;
                        phase.sN3 = "l" + value.Id;
                    }


                    if (obj.dN3 == 1)
                    {
                        phase.dN1 = 3;
                        phase.sN1 = "l" + value.Id;
                    }
                    else if (obj.dN3 == 2)
                    {
                        phase.dN2 = 3;
                        phase.sN2 = "l" + value.Id;
                    }
                    else
                    {
                        phase.dN3 = 3;
                        phase.sN3 = "l" + value.Id;
                    }

                }
                _con.PhasesConnection.Update(phase);
            }




            await _con.SaveChangesAsync();           
            return true;
        }
        public Load MappingtoLoad(LoadDataModel obj)
        {
            string ldFunction;
            if (obj.Function == "-1")
            {
                ldFunction = obj.FunctionOther;

                bool checkFunc = CheckFunction(ldFunction);
                if (checkFunc == false)
                {
                    Function func = new Function()
                    {
                        FunctionName = ldFunction
                    };
                    _con.Functions.Add(func);
                    _con.SaveChanges();
                }

            }
            else
            {
                ldFunction = obj.Function;
            }


            Load result = new Load()
            {
                name = obj.name,
                code = obj.code,
                Id = obj.Id,
                Connection = obj.Connection,
                Function = ldFunction,
                PhaseType = obj.PhaseType,
                SourceId = obj.SourceId,
                Type = obj.Type, 
                LoadInfo = _con.Loadparameter.SingleOrDefault(s=>s.Id == obj.loadpramid),
            };
            
            return result;
        }
        public bool EditLoadAsyn(LoadDataModel obj)
        {
          

            string ldFunction;
            if (obj.Function == "-1")
            {
                ldFunction = obj.FunctionOther;

                bool checkFunc = CheckFunction(ldFunction);
                if (checkFunc == false)
                {
                    Function func = new Function()
                    {
                        FunctionName = ldFunction
                    };
                    _con.Functions.Add(func);
                    _con.SaveChanges();
                }

            }
            else
            {
                ldFunction = obj.Function;
            }



            Load ld = new Load
            {
                Connection = obj.Connection,
                PhaseType = obj.PhaseType,
                Function = ldFunction,
                SourceId = obj.SourceId,
                Id = obj.Id , 
                Type = obj.Type,
                code = obj.code,
                name = obj.name
                 
            };
            Loadparameter ldpram = new Loadparameter
            {
                Id = obj.loadpramid,
                Power = obj.Power,
                PowerFactor = obj.PowerFactor,
                RatingCurrent = obj.RatingCurrent,
                RatingTemp = obj.RatingTemp,
                RatingVoltage = obj.RatingVoltage,
                Type = obj.Type
            };
            ld.LoadInfo = ldpram; 
            _con.Load.Update(ld);
            _con.Loadparameter.Update(ldpram);
             _con.SaveChanges();
            return true;  
        }  
        public LoadDataModel GetLoadById(int id)
        {
            var load = _con.Load.Include(l => l.LoadInfo).SingleOrDefault(l=>l.Id == id);
            LoadDataModel ld = MappingLoadDM(load);
            return ld;  
        }
        // return  loadDM With Full data
        public LoadDataModel MappingLoadDM(Load obj)
        {
            string SourceType;
            string Sourcename;
            string FactoryName;
            if (obj.SourceId % 2 == 0)
            {
                int fid = _con.secondarySource.SingleOrDefault(p => p.Code == obj.SourceId.ToString()).Fac_Id;
                Sourcename = _con.secondarySource.SingleOrDefault(p => p.Code == obj.SourceId.ToString()).Name;
                FactoryName = _con.Factory.SingleOrDefault(p => p.Id == fid).Name;
                SourceType = "Secondary Source";
            }
            else
            {
                Sourcename = _con.PrimarySource.SingleOrDefault(p => p.Code == obj.SourceId.ToString()).Name;
                SourceType = "Primary Source";
                int fid = _con.PrimarySource.SingleOrDefault(p => p.Code == obj.SourceId.ToString()).FactoryId;
                FactoryName = _con.Factory.SingleOrDefault(p => p.Id == fid).Name;
            }
            LoadDataModel ld = new LoadDataModel
            {
                code = obj.code,
                Id = obj.Id,
                Connection = obj.Connection,
                Function = obj.Function,
                PhaseType = obj.PhaseType,
                SourceId = obj.SourceId,
                PrimOrSec = SourceType,
                sourceName = Sourcename,
                PowerFactor = obj.LoadInfo.PowerFactor,
                RatingCurrent = obj.LoadInfo.RatingCurrent,
                RatingVoltage = obj.LoadInfo.RatingVoltage,
                RatingTemp = obj.LoadInfo.RatingTemp,
                Type = obj.Type,
                fac_name = FactoryName,
                Power = obj.LoadInfo.Power,
                loadpramid = obj.LoadInfo.Id,
                name = obj.name
            };
            return ld; 
        } 
        public List<LoadDataModel> GetAllLoads()
        {
            var Loads = _con.Load.Include(l => l.LoadInfo).ToList();
            List<LoadDataModel> res = new List<LoadDataModel>(); 
            foreach(var obj  in Loads)
            {
                string SourceType;
                string Sourcename;
                string FactoryName;
                int fid;
                int SID=0;
                if (obj.SourceId % 2 == 0)
                {
                    //var code = Convert.ToString(obj.SourceId);
                     fid = _con.secondarySource.SingleOrDefault(p => p.Code ==obj.SourceId.ToString()).Fac_Id; 
                    Sourcename = _con.secondarySource.SingleOrDefault(p => p.Code == obj.SourceId.ToString()).Name;
                    FactoryName  = _con.Factory.SingleOrDefault(p => p.Id == fid).Name;
                    SourceType = "Secondary Source"; 
                }
                else
                {
                    //var code = Convert.ToString(obj.SourceId);
                    var val = _con.PrimarySource.SingleOrDefault(p => p.Code == obj.SourceId.ToString());
                    if (val == null) continue; 
                    Sourcename = _con.PrimarySource.SingleOrDefault(p => p.Code == obj.SourceId.ToString()).Name;
                    SourceType = "Primary Source";
                    SID =Convert.ToInt16(_con.PrimarySource.SingleOrDefault(p => p.Code == obj.SourceId.ToString()).Code);
                     fid = _con.PrimarySource.SingleOrDefault(p => p.Code == obj.SourceId.ToString()).FactoryId;
                    FactoryName = _con.Factory.SingleOrDefault(p => p.Id == fid).Name;
                }

                res.Add(new LoadDataModel
                {
                    code = obj.code,
                    Id = obj.Id,
                    Connection = obj.Connection,
                    Function = obj.Function,
                    PhaseType = obj.PhaseType,
                    SourceId = obj.SourceId,
                    PrimOrSec = SourceType,
                    sourceName = Sourcename,
                    PowerFactor = obj.LoadInfo.PowerFactor,
                    RatingCurrent = obj.LoadInfo.RatingCurrent,
                    RatingVoltage = obj.LoadInfo.RatingVoltage,
                    Power = obj.LoadInfo.Power,
                    RatingTemp = obj.LoadInfo.RatingTemp,
                    Type = obj.Type,
                    fac_name = FactoryName,
                    name = obj.name,
                    Fac_Id = fid,
                    PS_Id= SID

                });  
            }
            return res ;  
        }
        
        public Load GetLoadByName(string name , int id)
        {
            var result = _con.Load.SingleOrDefault(l => l.name == name && l.SourceId == id);
            return result;
        }
        public async Task<bool>  DeleteLoad(int Id)
        {
            var x = _con.Load.Include (l=> l.LoadInfo).SingleOrDefault(s => s.Id == Id); 
            var w = _con.Loadparameter.SingleOrDefault(s => s.Id == x.LoadInfo.Id);

             _con.Load.Remove(x);
             _con.Loadparameter.Remove(w); 
            await  _con.SaveChangesAsync();

      


            return true;  


        }

        public List<LoadDataModel> MappingToLsDM(List<Load> ListObjs)
        {
            List<LoadDataModel> Results = new List<LoadDataModel>();
            foreach (var obj in ListObjs)
            {
                string SourceType;
                string Sourcename;
                string FactoryName;
                if (obj.SourceId % 2 == 0)
                {
                    int fid = _con.secondarySource.SingleOrDefault(p => p.Code == obj.SourceId.ToString()).Fac_Id;
                    Sourcename = _con.secondarySource.SingleOrDefault(p => p.Code == obj.SourceId.ToString()).Name;
                    FactoryName = _con.Factory.SingleOrDefault(p => p.Id == fid).Name;
                    SourceType = "Secondary Source";
                }
                else
                {
                    Sourcename = _con.PrimarySource.SingleOrDefault(p => p.Code == obj.SourceId.ToString()).Name;
                    SourceType = "Primary Source";
                    int fid = _con.PrimarySource.SingleOrDefault(p => p.Code == obj.SourceId.ToString()).FactoryId;
                    FactoryName = _con.Factory.SingleOrDefault(p => p.Id == fid).Name;
                }
                Results.Add(new LoadDataModel()
                {
                    code = obj.code,
                    Id = obj.Id,
                    Connection = obj.Connection,
                    Function = obj.Function,
                    PhaseType = obj.PhaseType,
                    SourceId = obj.SourceId,
                    PrimOrSec = SourceType,
                    sourceName = Sourcename,
                    PowerFactor = obj.LoadInfo.PowerFactor,
                    RatingCurrent = obj.LoadInfo.RatingCurrent,
                    RatingVoltage = obj.LoadInfo.RatingVoltage,
                    RatingTemp = obj.LoadInfo.RatingTemp,
                    Type = obj.Type,
                    fac_name = FactoryName,
                    Power = obj.LoadInfo.Power,
                    loadpramid = obj.LoadInfo.Id,
                     name = obj.name
                });
            }
            return Results;
        }

        

        public List<string> GetAllFunctions()
        {
            List<string> res = _con.Functions.Select(f=>f.FunctionName).ToList();
            return res;
        }



        public bool CheckFunction(string name)
        {
            var res = _con.Functions.SingleOrDefault(f => f.FunctionName == name);
            if (res != null)
            {
                return true;
            }
            else return false;
        }

    }


  

}
