using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace SmartPower.Models
{
    public class PrimarySource
    {
        public PrimarySource()
        {
            this.SourceLogs = new Collection<SourceReading>();
            this.secondarySources = new Collection<secondarySource>();
        }
        [Key]
        public int Id { get; set; } 
        public string Code { get; set; }
        public string Name { get; set; }
        public Factory Factory { get; set; }
        public int FactoryId { get; set; }
        public int DesignValue { get; set;  }
        public string Type { get; set; }
        public int MaxCurrent { get; set;  }
        public string Topology { get; set; }
        public int SourceTypeId { get; set; } 
        public SourceType SourceType { get; set; }
        public ICollection<SourceReading> SourceLogs { get; set; } 
        public ICollection<secondarySource> secondarySources { get; set; }
       // public ICollection<Load> loads { get; set; }

    }
}