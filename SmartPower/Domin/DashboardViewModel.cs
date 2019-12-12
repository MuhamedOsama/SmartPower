using SmartPower.Domin.report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartPower.Domin
{
    public class DashboardViewModel
    {
        public DashboardViewModel()
        {
            primaries = new Dictionary<int, ApiData>();
            Loads = new Dictionary<int, ApiData>();
            powerpeak = new List<PowerPeakViewModel>();
            loadTimeratio = new List<LoadTimeDownRatio>();
            EnergyConsumed = new Dictionary<int, Tuple<decimal, decimal, decimal>>();
            CountOfActiveLoads = 0;
            CoutOfLifeLoads = 0;
            bol = false; 
        }
        public string Fac_Name { get; set;  }
        public Dictionary<int, ApiData> primaries;
        public Dictionary<int, ApiData> Loads;
        public int CountOfActiveLoads;
        public int CoutOfLifeLoads;  
        public int fac_id; 
        public DateTime datetime;
        public bool bol;
        public List<PowerPeakViewModel> powerpeak;
        public List<LoadTimeDownRatio> loadTimeratio;
        //loadid ph1 ph2    ph3
        public Dictionary<int, Tuple<decimal, decimal, decimal>> EnergyConsumed;

    }
}
