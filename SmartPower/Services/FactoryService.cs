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
    public class FactoryService
    {
        private readonly PowerDbContext _context;



        public FactoryService(PowerDbContext context)
        {
            _context = context;
        }
        //Create New Factory 
        public async Task<bool> CreateFactory(FactoryDataModel obj)
        {
            string business;
            if (obj.businessType == "-1")
            {
                business = obj.businessTypeOthers;
                bool CheckBusiness = CheckBusinessTypeByName(obj.businessTypeOthers);
                if (CheckBusiness == false)
                {
                    BusinessTypeFac busfac = new BusinessTypeFac
                    {
                        busnisstype = business
                    };
                    _context.businessTypeFac.Add(busfac);
                  await  _context.SaveChangesAsync();
                }
            }
            else business = obj.businessType; 

            Factory DbObj = new Factory()
            {
                Name = obj.Name,
                Scale = obj.Scale,
                businessType = business
            };


            _context.Factory.Add(DbObj);
            await _context.SaveChangesAsync();
            return true;
        }
        //GetAllFactories 
        public List<FactoryDataModel> GetAllFactoriesSimple()
        {
            List<FactoryDataModel> Factories = new List<FactoryDataModel>();
            var Results = _context.Factory.ToList();
            if (Results.Count != null)
            {
                foreach (var item in Results)
                {
                    Factories.Add(new FactoryDataModel()
                    {

                        Name = item.Name,
                        Scale = item.Scale,
                        businessType = item.businessType,
                        FacId = item.Id
                    });
                }
            }
            return Factories;
        }
        //Delete Factory
        public async Task<bool> DeleteFactoryAsync(int id)
        {
            bool Result = false;
            var factory = _context.Factory.Single(f => f.Id == id);
           
            if (factory != null)
            {
                PrimarySourceSerivce ps = new PrimarySourceSerivce(_context);
                await ps.DeleteAllPrimaryAsync(id);
                SecoundrySourceService ss = new SecoundrySourceService(_context);
                await ss.DeleteAllSecondaryAsync(id);
                _context.Factory.Remove(factory);
                await _context.SaveChangesAsync();
                Result = true;
                return Result;
            }
            return Result;
        }
        //Edit Factory
        public async Task<bool> EditFactoryAsync(FactoryDataModel obj)
        {
            Factory DataObj = new Factory()
            {
                Name = obj.Name,
                businessType = obj.businessType,
                Scale = obj.Scale,
                Id = obj.FacId
            };
            _context.Factory.Update(DataObj);
            await _context.SaveChangesAsync();
            return true;
        }
        //Get Factory 
        public FactoryDataModel GetFactoryById(int Id)
        {

            var factory = _context.Factory.Find(Id);
            var Result = MapFactoryToFactoryDM(factory);

            return Result;
        }
        public Factory GetFactoryByName(string name)
        {

            var factory = _context.Factory.SingleOrDefault(f => f.Name == name);

            return factory;
        }
        public string GetFactoryNameById(int Id)
        {

            var factory = _context.Factory.Find(Id).Name;

            return factory;
        }
        //GetFactoryWithFullData
        public async Task<FactoryDataModelWithFullData> GetFactoryWithDataByIdAsync(int Id)
        {
            FactoryDataModelWithFullData Result = new FactoryDataModelWithFullData();
            var Query = _context.Factory.Include(f => f.PrimarySources).SingleOrDefault(f => f.Id == Id);
            if (Query != null)
            {
                Result = await MappingToFactoryFullDataAsync(Query);
            }
            return Result;
        }
        //Get Readings By Id for Source 
        public async Task<List<SourceReading>> GetReadingsByIdAsync(int Id)
        {
            var Readings = await _context.SourceReading.Where(r=>r.PrimarySourceId == Id).ToListAsync();
            return Readings;
        }

        //===============================================
        //Mapping Profile
        //Mapping Factory to Factory Data Model 
        public FactoryDataModel MapFactoryToFactoryDM(Factory obj)
        {
            FactoryDataModel Results = new FactoryDataModel()
            {

                Name = obj.Name,
                Scale = obj.Scale,
                businessType = obj.businessType,
                FacId = obj.Id
            };
            return Results;
        }
        //Mapping From FactoryDataModel To Factory 
        public Factory MappingToFactory(FactoryDataModel obj)
        {
            Factory mappingobj = new Factory()
            {
                Id = obj.FacId,
                Name = obj.Name,
                Scale = obj.Scale,
                businessType = obj.businessType,
    
            };
            return mappingobj;
        }
        //mapping Factory To Factory Full Data
        public async Task<FactoryDataModelWithFullData> MappingToFactoryFullDataAsync(Factory obj)
        {
            FactoryDataModelWithFullData MappedObj = new FactoryDataModelWithFullData()
            {
                FacId = obj.Id,
                Name = obj.Name,
                Scale = obj.Scale,
                businessType = obj.businessType,
  
            };
            foreach (var item in obj.PrimarySources)
            {
                MappedObj.PrimarySources.Add(new PrimarySourceDataModel()
                {
                    Id = item.Id,
                    Code = item.Code,
                    Name = item.Name,
                    SourceLogs = await GetReadingsByIdAsync(item.Id),
                     DesignValue = item.DesignValue ,  
                      Type =  item.Type,  
                       Topology = item.Topology,
                       MaxCurrent =  item.MaxCurrent

                });
            }
            return MappedObj;
        }


        public List<string> GetallBusinessType()
        {
            List<string> res = _context.businessTypeFac.Select(s=>s.busnisstype).ToList();
            return res; 
        }

        public bool CheckBusinessTypeByName(string name)
        {
            var check = _context.businessTypeFac.SingleOrDefault(s => s.busnisstype.ToLower() == name.ToLower());
            if (check != null)
                 return true;
            else return false;
        }
    }
}
