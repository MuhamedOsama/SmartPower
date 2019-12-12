using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartPower.Domin.report
{
    public class LoadTimeDownRatio
    {
        public int loadId {get;set;}
        public string LoadName {get;set;}


        public string totalKnownTime{get;set;}
        public string totalUnkonwnTime{get;set;}
        public string DownTimePerHour{get;set;}
        public string DownTimePercentageToKnownTime{get;set;}

    }
}
