using System;
using System.Collections.Generic;
using DenimERP.Models;

namespace DenimERP.ViewModels
{
    public class FPrWeavingProductionViewModel
    {
        public FPrWeavingProductionViewModel()
        {
            FPrWeavingProductionList = new List<F_PR_WEAVING_PRODUCTION>();
        }

        public F_PR_WEAVING_PRODUCTION FPrWeavingProduction { get; set; }
        public List<F_PR_WEAVING_PRODUCTION> FPrWeavingProductionList { get; set; }


        public List<LOOM_TYPE> LoomTypeList { get; set; }
        public List<RND_FABRICINFO> StyleNameList { get; set; }
        public List<RND_PRODUCTION_ORDER> SOList { get; set; }
        public List<F_HRD_EMPLOYEE> EmployeeList { get; set; }

        public double ? TotalWeavingProduction { get; set; }
        public double ? TotalAirjetProduction { get; set; }
        public double ? TotalRapierProduction { get; set; }

        public double ? MonthlyTotalWeavingProduction { get; set; }
        public double? MonthlyTotalAirjetProduction { get; set; }
        public double? MonthlyTotalRapierProduction { get; set; }

        public double? DailyTotalWeavingProduction { get; set; }
        public double? DailyTotalAirjetProduction { get; set; }
        public double? DailyTotalRapierProduction { get; set; }

        public double ? ComparisonMonthlyTotalAirjetProduction { get; set; }
        public double ? ComparisonMonthlyTotalRapierProduction { get; set; }

        public DateTime date { get; set; }

        public double ? MonthlyCompleteSets { get; set;}
        public double ? MonthlyPendingSets { get; set;}

        public double ? MonthlyCompletePercent { get; set; }
        public double ? MonthlyPendingPercent { get; set; }
    }

}
