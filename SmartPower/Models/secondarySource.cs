using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace SmartPower.Models
{
    public class secondarySource
    {
        public secondarySource()
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
        public int Fac_Id { get; set; }
        public int DesignValue { get; set; }
        public string Type { get; set; }
        public int MaxCurrent { get; set; }
        public string Topology { get; set; }
        public ICollection<SourceReading> SourceLogs { get; set; }
        public ICollection<Load> Loads { get; set; }
    }
}