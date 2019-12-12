using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartPower.Domin
{
    public class Instant
    {
        public Instant()
        {
            Cphase1 = new List<decimal>();
            Cphase2 = new List<decimal>();
            Cphase3 = new List<decimal>();
            Vphase1 = new List<decimal>();
            Vphase2 = new List<decimal>();
            Vphase3 = new List<decimal>();
            Pphase1 = new List<decimal>();
            Pphase2 = new List<decimal>();
            Pphase3 = new List<decimal>();
            time = new List<string>();



        }
        public List<decimal> Cphase1 { get; set; }
        public List<decimal> Cphase2 { get; set; }
        public List<decimal> Cphase3 { get; set; }
        public List<decimal> Vphase1 { get; set; }
        public List<decimal> Vphase2 { get; set; }
        public List<decimal> Vphase3 { get; set; }
        public List<decimal> Pphase1 { get; set; }
        public List<decimal> Pphase2 { get; set; }
        public List<decimal> Pphase3 { get; set; }
        public List<string> time { get; set; }
        public int Cnt { get; set; }
        public string loadName { get; set; }

        public int SortVal { get; set; }
        //public string CSvString { get; set; }


    }
}
