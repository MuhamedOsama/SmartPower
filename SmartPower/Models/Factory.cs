using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SmartPower.Models
{
    public class Factory
    {
        public Factory()
        {
            this.PrimarySources = new Collection<PrimarySource>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(255)]
        public string Name { get; set; }
        [Required]
        [StringLength(255)]
        public string Scale { get; set; }
     
        [StringLength(255)]
        public string businessType { get; set; }
        public ICollection<PrimarySource> PrimarySources { get; set; }
    }
}
