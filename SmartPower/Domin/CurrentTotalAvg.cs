using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace SmartPower.Controllers.Domin
{
    public class CurrentTotalAvg
    {
        public CurrentTotalAvg()
        {
            DataList = new Collection<CurrentAvgDataModel>();
        }
        public ICollection<CurrentAvgDataModel> DataList { get; set; }
        public decimal TotalAvgOne { get; set; }
        public decimal TotalAvgTwo { get; set; }
        public decimal TotalAvgThree { get; set; }
        public decimal PeakOne { get; set; }
        public decimal PeakTwo { get; set; }
        public decimal PeakThree { get; set; }
        public decimal VarOne { get; set; }
        public decimal varTwo { get; set; }
        public decimal VarThree { get; set; }

    }
}
