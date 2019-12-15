using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartPower.Domin.report
{
    public class AveragePowerTodayViewModel
    {
        public decimal? LoadPower1Average { get; set; }
        public decimal? LoadPower2Average { get; set; }
        public decimal? LoadPower3Average { get; set; }
        public decimal? LoadPowerAverage { get; set; }
        public decimal? FunctionPower1Average { get; set; }
        public decimal? FunctionPower2Average { get; set; }
        public decimal? FunctionPower3Average { get; set; }
        public decimal? FunctionPowerAverage { get; set; }

    }
}
