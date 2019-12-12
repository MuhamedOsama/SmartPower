using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartPower.Models
{
    public class CurrentSpikeLogs
    {
        public int Id { get; set; }
        public decimal Value { get; set; } 
        public DateTime Time { get; set; }
        public string PhaseName { get; set; }
        //public int SourceId { get; set; }
        public string SourceCode { get; set; }
    }
}
