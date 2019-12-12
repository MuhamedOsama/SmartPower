using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartPower.Domin.Report
{
    public class RushHourViewModel
    {

        public RushHourViewModel()
        {
            peack1 = peack2 = peack3 = -1;
        }
        public decimal peack1 { get; set; }
        public decimal peack2 { get; set; }
        public decimal peack3 { get; set; }
        public string  StartPeakonetime { get; set; }
        public string StartPeaktwotime { get; set; }
        public string StartPeakthreetime { get; set; }
        public string EndPeakonetime { get; set; }
        public string EndPeaktwotime { get; set; }
        public string EndPeakthreetime { get; set; }
        public int phasenumber { get; set; }
        public string loadtype { get; set; }
        public string name { get; set; }
        public double Standard1 { get; set; }
        public double Standard2 { get; set; }
        public double Standard3 { get; set; }
        public double Variance1 { get; set; }
        public double Variance2 { get; set; }
        public double Variance3 { get; set; }
    }
}
