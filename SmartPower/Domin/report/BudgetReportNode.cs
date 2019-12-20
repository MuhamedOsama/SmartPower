using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartPower.Domin.report
{
    public class BudgetReportNode
    {
        public dynamic text { get; set; }
        public string image { get; set; }
        public List<dynamic> children { get; set; } = new List<dynamic>();
    }
}
