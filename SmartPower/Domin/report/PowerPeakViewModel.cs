using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartPower.Domin
{
    public class PowerPeakViewModel
    {

        public int Id { get; set; }
        public string name { get; set; }
        public decimal peakP1 { get; set; }
        public DateTime dateP1 { get; set; }
        public decimal peakP2 { get; set; }
        public DateTime dateP2 { get; set; }
        public decimal peakP3 { get; set; }
        public DateTime dateP3 { get; set; }
        public int phasenumber { get; set; }
        public string loadtype {get ; set;}
        public decimal RatingPowerValue { get; set; }
        public decimal PeakToRatePercentage1 { get; set; }
        public decimal PeakToRatePercentage2 { get; set; }
        public decimal PeakToRatePercentage3 { get; set; }

    }
}
