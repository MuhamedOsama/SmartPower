using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SmartPower.Controllers.Domin
{
    public class SecoundrySouresDataModelSim
    {
        public int Id { get; set; }
       
        public string Code { get; set; }
        public string Name { get; set; }
        public string PirmarySourceName { get; set; }
        public int PS_Id { get; set; }
        public int DesignValue { get; set; }
        public string Type { get; set; }
        public int MaxCurrent { get; set; }
        public string Topology { get; set; }
        public int FacId { get; set; }
        [Display(Name = "Factory Name")]
        public string FacName { get; set; }


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
