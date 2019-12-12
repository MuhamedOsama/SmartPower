using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace SmartPower.Models
{
    public class Load
    {
        public Load()
        {
            this.LoadLogs = new Collection<LoadReading>();
        }
        [Key]
        public int Id { get; set; }

       
        public string name {get; set;}
    

        public int code { get; set; }

        public int SourceId { get; set; }

        public string Type { get; set; }

        public string Function { get; set; }

        public string Connection { get; set; }

        public string PhaseType { get; set; }

        public Loadparameter LoadInfo { get; set; }
        public ICollection<LoadReading> LoadLogs { get; set; }

        
    }
}