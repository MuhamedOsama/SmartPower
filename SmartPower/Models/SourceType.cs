using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SmartPower.Models
{
    public class SourceType
    {
        [Key]
        public int TypeId { get; set; }
        public string TypeName { get; set; }
    }
}
