using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartPower.Domin
{
    public class EnergyC
    {
        public string loadName { get; set; }
        public string date { get; set; }
        public decimal Ephase1 { get; set; }
        public decimal Ephase2 { get; set; }

        public decimal Ephase3 { get; set; }

    }
}
