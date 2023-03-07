using System;

namespace DenimERP.ViewModels
{
    public class WarpingChartDataViewModel
    {
        public double? BallWarping { get; set; }
        public double? DirectWarping { get; set; }
        public double? EcruWarping { get; set; }
        public double? SectionalWarping { get; set; }
        public double ? TotalWarping { get; set; }
        public double? TodaysWarping { get; set; }
        public double? MonthlyWarping { get; set; }
        public double? Recone { get; set; }
        public int ? WarpingPendingSets { get; set; }
        public int ? MonthlyWarpingPendingSets { get; set; }
        public int ? MonthlyWarpingCompleteSets { get; set; }
        public double ? PendingPercent { get; set; }
        public double? MonthlyPendingPercent { get; set; }
        public double ? WarpingCompleteSets { get; set; }
        public double ? CompletePercent { get; set; }
        public double ? MonthlyCompletePercent { get; set; }

        public double ? ConsumedYarn { get; set; }
        public double ? BudgetYrn { get; set; }
        //Rope Warping
        public double ? TotalRopeWarping { get; set; }
        public double ? TodaysRopeWarping { get; set;}
        public double ? MonthlyRopeWarping { get; set; }
        public double ? ComparisonMonthlyRopeWarping { get; set;}
        //Direct Warping
        public double? TotalDirectWarping { get; set; }
        public double? TodaysDirectWarping { get; set; }
        public double? MonthlyDirectWarping { get; set; }
        public double? ComparisonMonthlyDirectWarping { get; set; }
        //Ecru Warping
        public double? TotalEcruWarping { get; set; }
        public double? TodaysEcruWarping { get; set; }
        public double? MonthlyEcruWarping { get; set; }
        public double? ComparisonMonthlyEcruWarping { get; set; }
        //Sectional Warping
        public double ? TotalSectionalWarping { get; set; }
        public DateTime Date { get; set; }
        public DateTime date1 { get; set; }
    }
}
