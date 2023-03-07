using System.ComponentModel.DataAnnotations;

namespace DenimERP.ViewModels
{
    public class FinishingChartDataViewModel
    {

        [Display(Name = "Finishing Length")]
        public double ? TotalFinishing { get; set; }
        public double ? TodaysFinishing { get; set; }
        public double ? TodaysPending { get; set; }
        public double ? MonthlyFinishing { get; set; }
        
    }
}
