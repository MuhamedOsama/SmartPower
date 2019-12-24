using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartPower.Models.SerializationModels
{
    public class SumLoadEnergy
    {
        public int id { get; set; }
        public string fromdate { get; set; }
        public string todate { get; set; }
    }
}
