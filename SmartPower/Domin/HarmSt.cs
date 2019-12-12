using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartPower.Domin
{
    public class HarmSt
    {
        public HarmSt()
        {
            Hphase1 = new List<decimal>();
            Hphase2 = new List<decimal>();

            Hphase3 = new List<decimal>();
            time = new List<string>();


        }

        public int cnt { get; set; }
        public string loadName { get; set; }
        public DateTime Date { get; set; }
        public int  HarmNum { get; set; }

        public List<decimal> Hphase1 { get; set; }
        public List<decimal> Hphase2     { get; set; }
       public List<decimal> Hphase3 { get; set; }
        public List<string> time { get; set; }
    }
}
