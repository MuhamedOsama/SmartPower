using System;
using System.ComponentModel.DataAnnotations;

namespace SmartPower.Models
{
    public class LoadReading
    {
        [Key]
        public int Id { get; set; }
        public int LoadId { get; set; }
        public decimal Vibration { get; set; }
        public decimal Speed { get; set; }
        public decimal Temperature { get; set; }
        public DateTime TimeStamp { get; set; }

    }
}