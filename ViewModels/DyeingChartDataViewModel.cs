using System;

namespace DenimERP.ViewModels
{
    public class DyeingChartDataViewModel
    {
        public double? RopeDyeing { get; set; }
        public double? SlasherDyeing { get; set; }
        public double? Today { get; set; }
        public double? Yesterday { get; set; }
        public double ? DyeingPendingSets { get; set; }
        public double ? DyeingCompleteSets { get; set; }
        public double ? PendingPercent { get; set; }
       public double ? CompletePercent { get; set; }
       public double? MonthlyDyeingCompletePercent { get; set; }
       public double ? MonthlyDyeingPendingPercent { get; set; }
       public double ? MonthlyDyeingCompleteSets { get; set; }
       public double ? MonthlyDyeingPendingSets { get; set; }
       public double ? ConsumedChemical { get; set; }
       public DateTime date { get; set; }
       public double ? TotalDyeing { get; set; }
       public double ? TotalRopeDyeing { get; set; }
       public double ? TotalSlasherDyeing { get; set; }
       public double ? TodayRopeDyeing { get; set; }
       public double ? TodaySlasherDyeing { get; set; }
       public double ? MonthlyRopeDyeing { get; set; }
       public double ? MonthlySlasherDyeing { get; set; }
       public double ? MonthlyDyeing { get; set; }
       public double ? TodayDyeing { get; set; }
       public double ? ComparisonMonthlyDyeing { get; set; }
       public double ? ComparisonMonthlyRopeDyeing { get; set; }
       public double ? ComparisonMonthlySlasherDyeing { get; set; }
    }
}
