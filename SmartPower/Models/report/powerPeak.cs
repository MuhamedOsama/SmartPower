using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartPower.Models.report
{
    public class powerPeak
    { 
        public int Id { get; set; }
        public int primarySourceId { get; set; }
        public int secondrySourceId { get; set; }
        public int loadId { get; set; }
        public decimal peakP1 { get; set; }
        public DateTime dateP1 { get; set; }
        public decimal peakP2 { get; set; }
        public DateTime dateP2 { get; set; }
        public decimal peakP3 { get; set; }
        public DateTime dateP3 { get; set; }

    }
}
