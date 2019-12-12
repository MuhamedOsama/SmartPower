using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartPower
{
    public class ApiData
    {
     
        public string name { get; set; }
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
        public decimal HarmonicOrder { get; set; }
        public decimal HarmonicOrder1 { get; set; }
        public decimal HarmonicOrder2 { get; set; }
        public decimal HarmonicOrder3 { get; set; }
        public decimal ReturnCurrent { get; set; }
        public DateTime TimeStamp { get; set; }  
    }
}
