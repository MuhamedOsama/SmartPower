using SmartPower.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace SmartPower.Controllers.Domin
{
    public class LoadDataModel
    {         
       
        public int Id { get; set; }
        public int loadpramid { get; set; }
        public string name { get; set; }
        public int code { get; set; }
        public int secondarySourceId { get; set; }
        public int Fac_Id { get; set; } 
        public int PS_Id { get; set; }
        public int SourceId { get; set; }
        public string sourceName { get; set; }
        public string fac_name { get; set; }
        public string PrimOrSec { get; set; }

        public string Function { get; set; }
        public string FunctionOther { get; set; }
        public string Connection { get; set; }
        public string PhaseType { get; set; }
        public decimal PowerFactor { get; set; }
        public decimal Power { get; set; }
        public decimal RatingCurrent { get; set; }
        public decimal RatingVoltage { get; set; }
        public decimal RatingTemp { get; set; }
        public string Type { get; set; }

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
