using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartPower.Models
{
    public class Wire
    {
        public int Id { get; set; }
        public string Vendor { get; set; }
        public decimal  Lenght { get; set; }
        public Load Load { get; set; }
        public int LoadId { get; set; }
        //public int secondarySourceId { get; set; }
        public secondarySource SecondarySource { get; set; }

    }
}
