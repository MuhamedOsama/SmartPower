using SmartPower.DataContext;
using SmartPower.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartPower.Services
{
    public class ReportConstantServices
    {
        private readonly PowerDbContext _Contect;

        public ReportConstantServices(PowerDbContext _contect)
        {
            _Contect = _contect;
        }
        public async Task<bool> CreateAsync(ReportConstant obj)
        {
            _Contect.ReportConstants.Add(obj);
            await _Contect.SaveChangesAsync();
            return true;
        }
        public bool Delete(int id)
        {
            var Query = _Contect.ReportConstants.SingleOrDefault(r => r.Id == id);
            _Contect.ReportConstants.Remove(Query);
            _Contect.SaveChangesAsync();
            return true;
        }
        public bool Edit( ReportConstant obj)
        {
            _Contect.ReportConstants.Update(obj);
            _Contect.SaveChanges();
            return true;
        }
        public ReportConstant GetReportConstant(int Id)
        {
            var Query = _Contect.ReportConstants.SingleOrDefault(r => r.Id == Id);
            return Query;
        }
        public List<ReportConstant> GetAllReportConst()
        {
            return _Contect.ReportConstants.ToList();
        }
        
    }
}
