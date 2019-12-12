using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SmartPower.Controllers.Domin
{
    public class FactoryDataModelWithFullData
    {
        public FactoryDataModelWithFullData()
        {
            PrimarySources = new Collection<PrimarySourceDataModel>();
        }

        public int FacId { get; set; }

        [Required]
        public string Name { get; set; }
        [Required]
        public string Scale { get; set; }

        [Display(Name = "Business Type")]
        public string businessType { get; set; }
        public ICollection<PrimarySourceDataModel> PrimarySources { get; set; }
    }
}
