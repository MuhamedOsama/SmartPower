using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartPower.Models.report
{
    public class PowerAvg
    { 
        public int Id { get; set; }

        public Nullable<int> primarySourceId { get; set; }
        public Nullable<int> secondrySourceId { get; set; }
        public int loadId { get; set; }

        public Decimal P1 { get; set; }
        public Decimal P2 { get; set; }
        public Decimal P3 { get; set; }

        public DateTime readingDate { get; set; }


    }
}
