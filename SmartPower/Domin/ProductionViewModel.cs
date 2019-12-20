using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;



namespace SmartPower.Domin
{
    public class ProductionViewModel
    {
        public ProductionViewModel()
        {
            Diction = new Dictionary<int, List<Tuple<string, string, string, string, string>>>();
            AvgPower = new Dictionary<int, List<Tuple<string, string, string>>>();
            Ratio = new Dictionary<int, List<Tuple<string, string, string>>>();
            
         
        }
        public int Id { get; set; }
        public int FacId { get; set; }
        public string facname { get; set; }
        public double Quantity { get; set; }
        public string Date { get; set; }     
        public string Type { get; set; }
        // primary id          name      date ,  Energy @1  ,  Energy @2 , Energy @3
        public Dictionary<int, List<Tuple<string, string, string, string, string>>> Diction; // Energy
                                            // @1    @2      @3
        public Dictionary<int, List<Tuple< string, string, string>>> AvgPower;
        public Dictionary<int, List<Tuple< string, string, string>>> Ratio;


    }
}
