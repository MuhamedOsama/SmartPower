using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SmartPower.Controllers.Domin
{
    public class FactoryDataModel
    {

        public int FacId { get; set; }

        [Required]
        public string Name { get; set; }
        [Required]
        public string Scale { get; set; }

       [Display(Name ="Business Type")]
        public  string businessType { get; set; }
        public  string businessTypeOthers { get; set; }


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
