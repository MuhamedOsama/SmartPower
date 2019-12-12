using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartPower.Domin
{
    public class AvgPowerViewModel
    {
        public int primarySourceId { get; set; }
        public int secondrySourceId { get; set; }
        public int loadId { get; set; }
        public int peakP1 { get; set; }
        public DateTime dateP1 { get; set; }
        public int peakP2 { get; set; }
        public DateTime dateP2 { get; set; }
        public int peakP3 { get; set; }
        public DateTime dateP3 { get; set; }
    }
}
