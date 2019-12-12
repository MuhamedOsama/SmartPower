using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartPower.Controllers.Domin
{
    public class CurrentAvgDataModel
    {
        public int Id { get; set; }
        public decimal CurrentOne { get; set; }
        public decimal CurrentTwo { get; set; }
        public decimal CurrentThree { get; set; }
        public int? PS_Id { get; set; }
        public int? SS_Id { get; set; }
        public string PS_Name { get; set; }
        public string SS_Name { get; set; }

    }
}
