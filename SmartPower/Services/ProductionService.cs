using SmartPower.DataContext;
using SmartPower.Domin;
using SmartPower.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;


namespace SmartPower.Services
{
    public class ProductionService
    {
        public PowerDbContext _Context;
        public ProductionService(PowerDbContext con)
        {
            _Context = con;
        }
        public async Task<bool> CreatProductionAsync(ProductionViewModel obj)
        {
            var prod = MappingToproductionModel(obj);
            _Context.Production.Add(prod);
            await _Context.SaveChangesAsync();
            return true;
        }
        public Production MappingToproductionModel(ProductionViewModel obj)
        {
            Production pr = new Production
            {
                Id = obj.Id,
                Quantity = obj.Quantity,
                FacId = obj.FacId,
                Date = DateTime.Parse(obj.Date),               
                Type = obj.Type
            };
            return pr;
        }
        public ProductionViewModel MappingToproductionViewModel(Production obj)
        {
            string name = (_Context.Factory.SingleOrDefault(s => s.Id == obj.FacId)).Name;
            ProductionViewModel pr = new ProductionViewModel
            {
                Id = obj.Id,
                Quantity = obj.Quantity,
                FacId = obj.FacId,
                Date = obj.Date.ToString("yyyy-MM-dd"),
                Type = obj.Type,
                facname = name
            };
            return pr;
        }
        public List<ProductionViewModel> MappingToproductionModellist(List<Production> obj)
        {
            List<ProductionViewModel> res = new List<ProductionViewModel>();

          
            foreach (var item in obj)
            {
                double prod;

                if (item == null)
                {
                    prod = 0;
                }
                else
                    prod = item.Quantity;

                string name = (_Context.Factory.SingleOrDefault(s => s.Id == item.FacId)).Name;
                res.Add(new ProductionViewModel
                {
                    Id = item.Id,
                    Quantity = prod,
                    FacId = item.FacId,
                    Date = (item.Date.Date).ToString("yyyy-MM-dd"),
                    facname = name,
                    Type = item.Type
                });
            }
            return res;
        }
        public List<char> GetallDatesOroductionWithFacid(int facid)
        {
            List<string> res = new List<string>();
          //SqlConnection con = new SqlConnection("Server=WIN-P2A6HV3LSI9\\SQLEXPRESS;Database=power;Integrated Security=SSPI;");
            //SqlConnection con = new SqlConnection("Server=localhost;Database=power;Integrated Security=SSPI;");
            //con.Open();
            //SqlDataReader rdr;
            //SqlCommand cmd = new SqlCommand("select CONVERT(date, Date) from production where FacId =" + facid, con);
            //rdr = cmd.ExecuteReader();






           var jj = _Context.Production.Where(r => r.FacId == facid).Select(r => r.Date).ToString().ToList();




            
                
                
                
            //    while (rdr.Read())
            //    res.Add(rdr[0].ToString());

            //con.Close();
            //rdr.Close();
            return jj;
        }


        public List<ProductionViewModel> GetallProduction(int facid)
        {
            List<Production> li = _Context.Production.Where(s => s.FacId == facid).ToList();
            List<ProductionViewModel> res = MappingToproductionModellist(li);
            return res;
        }

    }
}
