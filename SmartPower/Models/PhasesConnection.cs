using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;


namespace SmartPower.Models
{
    public class PhasesConnection
    {
        public PhasesConnection() { }
        public int ID { get; set; }

        public int SourceId { get; set; }
        public string SourceType { get; set; }
        public string DestinationType { get; set; }

        // ID + type (l1 || s1)
        public string sN1 { get; set; }  
        public string sN2 { get; set; }  
        public string sN3 { get; set; }  

        // node number || Phase number
        public int dN1 { get; set; } 
        public int dN2 { get; set; }
        public int dN3 { get; set; }
    
    }
}
