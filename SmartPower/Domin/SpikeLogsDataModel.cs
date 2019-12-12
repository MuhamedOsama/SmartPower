using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartPower.Controllers.Domin
{
    public class SpikeLogsDataModel
    {
        public int Id { get; set; }
        public decimal Value { get; set; }
        public DateTime Time { get; set; }
        public string PhaseName { get; set; }
        public string SourceName { get; set; }
        public string SourceCode { get; set; }
        public int secondarySourceId { get; set; }
    }
}
