using SmartPower.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace SmartPower.Controllers.Domin
{
    public class PrimarySourceDataModel
    {
        public PrimarySourceDataModel()
        {
            this.SourceLogs = new Collection<SourceReading>();
            secondarySources = new Collection<SecoundrySouresDataModelSim>();
        }

        public int Id { get; set; }
        public string Code { get; set; }
        public string  Name { get; set; }
        public int FacId { get; set; }
        [Display(Name ="Factory Name")]
        public string  FacName { get; set; }
        public int DesignValue { get; set; }
        public string Type { get; set; }
        public int MaxCurrent { get; set; }
        public string Topology { get; set; }
    

        public ICollection<SecoundrySouresDataModelSim> secondarySources { get; set; }
        public ICollection<SourceReading> SourceLogs { get; set; }
    }
}