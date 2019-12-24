using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SmartPower.Models
{
    public class SourceRelation
    {
        [Key]
        public int Id { get; set; }
        public int ChildId { get; set; }
        public int ParentId { get; set; }
        
    }
}
