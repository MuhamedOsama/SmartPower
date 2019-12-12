using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartPower.Models
{
    public class querySource
    {



        public querySource()
        {
            CurrentAvg = new List<List<List<decimal>>>();
            VoltageAvg = new List<List<List<decimal>>>();
            PowerFactorAvg = new List<List<List<decimal>>>();
            HarmAvg = new List<List<List<decimal>>>(); 
        }

        public   int id { get; set; }
        public int sourceid { get; set; }
        public string name {  get; set; }
        public decimal Voltage1 { get; set; }
        public decimal Voltage2 { get; set; }
        public decimal Voltage3 { get; set; }
        public decimal Current1 { get; set; }
        public decimal Current2 { get; set; }
        public decimal Current3 { get; set; }
        public decimal mCurrent1 { get; set; }
        public decimal mCurrent2 { get; set; }
        public decimal mCurrent3 { get; set; }
        public decimal PowerFactor1 { get; set; }
        public decimal PowerFactor2 { get; set; }
        public decimal PowerFactor3 { get; set; }
        public decimal frequency1 { get; set; }
        public decimal frequency2 { get; set; }
        public decimal frequency3 { get; set; }
        public decimal Power1 { get; set; }
        public decimal Power2 { get; set; }
        public decimal Power3 { get; set; }
        public DateTime Timestamp { get; set; }
        public decimal HarmonicOrder { get; set; }
        public decimal HarmonicOrder1 { get; set; }
        public decimal HarmonicOrder2 { get; set; }
        public decimal HarmonicOrder3 { get; set; }
        public int facid { get; set; }

        public decimal ReturnCurrent { get; set; }
        public string code { get; set; }


        public List<List<List<decimal>>> CurrentAvg; 
       public List<List<List<decimal>>> VoltageAvg;
       public List<List<List<decimal>>> PowerFactorAvg; 
       public List<List<List<decimal>>> HarmAvg;
        public List<decimal> InsCurrent1 { get; set; }
        public List<decimal> InsCurrent2 { get; set; }

        public List<decimal> InsCurrent3 { get; set; }
        public DateTime InsTime { get; set; }




    }
}
