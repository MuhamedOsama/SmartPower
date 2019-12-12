using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartPower.Controllers.Domin
{
    public class SourceBalnceRetio
    {
        public int SourceId { get; set; }
        public string SourceName { get; set; }
        public string Source_tybe { get; set; }
        public decimal I_Retio { get; set; }
        public decimal V_Retio { get; set; }
        public decimal pf_Retio { get; set; }
    }
}
