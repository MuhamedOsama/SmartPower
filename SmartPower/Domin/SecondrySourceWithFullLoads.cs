using SmartPower.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace SmartPower.Controllers.Domin
{
    public class SecondrySourceWithFullLoads
    {

        public SecondrySourceWithFullLoads()
        {
            this.SourceLogs = new Collection<SourceReading>();
            this.Loads = new Collection<Load>();
        }
        [Key]
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public PrimarySource PrimarySource { get; set; }
        public int PrimarySourceId { get; set; }

        public ICollection<SourceReading> SourceLogs { get; set; }
        public ICollection<Load> Loads { get; set; }

    }
}
