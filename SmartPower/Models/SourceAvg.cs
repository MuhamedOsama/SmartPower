using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartPower.Models
{
    public class SourceAvg
    {
        public int Id { get; set; }
        public decimal Current1Avg { get; set; }
        public decimal Current2Avg { get; set; }
        public decimal Current3Avg { get; set; }
        public DateTime Time { get; set; }
        public PrimarySource PrimarySource { get; set; }
        public Nullable<int> PrimarySourceId { get; set; }
        public secondarySource SecondarySource { get; set; }
        public Nullable<int> SecondarySourceId { get; set; }
    }
}
