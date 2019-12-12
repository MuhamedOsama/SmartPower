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
    public class PhasesConnectionService
    {

        private readonly PowerDbContext _context;


        public PhasesConnectionService(PowerDbContext _context)
        {
            this._context = _context;  
        }     
        public void Create(int id,string type)
        {
            
         
            if(type  == "1")
            {
                _context.PhasesConnection.Add(new PhasesConnection
                {
                    SourceType = type,
                    SourceId = id,
                    dN1 = -1,
                    dN2 = -2,
                    dN3 = -2
                });

            } else
            {
                _context.PhasesConnection.Add(new PhasesConnection
                {
                    SourceType = type,
                    SourceId = id,
                    dN1 = -1,
                    dN2 = -1,
                    dN3 = -1
                });
            }
            _context.SaveChanges();

        }
    
        public PhasesConnection getPhaseBySourceId(int id)
        {
            List<PhasesConnection> phase = null;
            if (id%2 != 0)
            {
                var primORsec = _context.PrimarySource.SingleOrDefault(p => p.Id == id);
             phase = _context.PhasesConnection.Where(p => p.SourceId == primORsec.Id).ToList();
            }
            else
            {
                var primORsec = _context.secondarySource.SingleOrDefault(p => p.Code == id.ToString());
               phase = _context.PhasesConnection.Where(p => p.SourceId == primORsec.Id).ToList();
            }
            
            return phase.FirstOrDefault();
        }
        public  List<PrimarySource> GetValidPrimarySource(string type,int factoryid)  //type of distination 
        {
            var validd = _context.PrimarySource.Where(P=> P.FactoryId == factoryid).ToList();
            return validd;  
        }
        // not used any more
        //public List<PrimarySource> GetValidPrimarySource(string type, int factoryid)  //type of distination 
        //{
        //    List<PhasesConnection> li = _context.PhasesConnection.ToList();
        //    List<PhasesConnection> valid = new List<PhasesConnection>();
        //    foreach (PhasesConnection pc in li)
        //    {
        //        if (type == "1" && pc.SourceId % 2 != 0)
        //        {
        //            if (pc.dN1 == -1 || pc.dN2 == -1 || pc.dN3 == -1) valid.Add(pc);
        //        }
        //        else if (type == "3" && pc.SourceId % 2 != 0 && pc.SourceType == "3")
        //        {
        //            if (pc.dN1 == -1 && pc.dN2 == -1 && pc.dN3 == -1) valid.Add(pc);
        //        }
        //    }
        //    List<PrimarySource> validd = new List<PrimarySource>();
        //    PrimarySourceSerivce ps = new PrimarySourceSerivce(_context);

        //    foreach (var primary in valid)
        //    {
        //        var par = ps.GetPrimarySourceFromDB(primary.SourceId);

        //        if (par.FactoryId == factoryid)
        //        {
        //            validd.Add(par);
        //        }
        //    }
        //    return validd;
        //}

        public List<secondarySource> GetValidsecondrySource(string type, int factoryid)  //type of distination 
        {

            List<PhasesConnection> li = _context.PhasesConnection.ToList();
            List<PhasesConnection> valid = new List<PhasesConnection>();
            foreach (PhasesConnection pc in li)
            {
                if (type == "1" && pc.SourceId % 2 == 0)
                {
                    if (pc.dN1 == -1 || pc.dN2 == -1 || pc.dN3 == -1) valid.Add(pc);
                }
                else if (type == "3" && pc.SourceId % 2 == 0 && pc.SourceType == "3")
                {
                    if (pc.dN1 == -1 && pc.dN2 == -1 && pc.dN3 == -1) valid.Add(pc);
                }
            }
            List<secondarySource> validd = new List<secondarySource>();
            SecoundrySourceService ps = new SecoundrySourceService(_context);

            foreach (var secondary in valid)
            {
                var par = ps.GetSecoundrySourceFromDBByCode(secondary.SourceId);

                if (par.Fac_Id == factoryid)
                {
                    validd.Add(par);
                }
            }
            return validd;
        }
        public validNodes GetValidNodes(int id) // id of 
        {

            if (id % 2 != 0)
            {
                validNodes valid = new validNodes
                {
                    dN1 = -1,
                    dN2 = -1,
                    dN3 = -1,
                };
                return valid;

            }
            else
            {
                PhasesConnection phase = getPhaseBySourceId(id);

                validNodes valid = new validNodes
                {
                    dN1 = phase.dN1,
                    dN2 = phase.dN2,
                    dN3 = phase.dN3,
                };

                return valid;

            }


        }
    }
}
