using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SmartPower.Models
{
    public class Production
    {

        [Key]
        public int Id { get; set; }
        public int FacId { get; set; }
        public double Quantity { get; set; }
        public DateTime Date { get; set; }
        public string Type { get; set; }

    }
}
