using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartPower.Models
{
    public class PowAvg
    {


        public int Id { get; set; }
        public int loadId { get; set; }
        public decimal pAvg1 { get; set; }
        public decimal pAvg2 { get; set; }
        public decimal pAvg3 { get; set; }
        public System.DateTime date
        {
            get; set;
        }
    }
}
