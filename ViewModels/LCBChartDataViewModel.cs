using System;

namespace DenimERP.ViewModels
{
    public class LCBChartDataViewModel
    {
        public double? LCBData { get; set; }
        public double? ReconData { get; set; }
        public double ? LcbCompleteLength { get; set; }
        public DateTime date { get; set; }
        public double ? MonthlyCompletePercent { get; set; }
        public double ? MonthlyPendingPercent { get; set; }
        public int ? MonthlyLCBCompleteSets { get; set; }
        public int ? MonthlyLCBPendingSets { get; set; }
        public double ? ComparisonMonthlyLCB { get; set; }
        public double ? TotalProduction { get; set; }
        public double ? MonthlyProduction { get; set; }
        public double ? DailyProduction { get; set; }
        public double ? PendingSets { get; set; }
        public double ? CompleteSets { get; set; }


    }
}
