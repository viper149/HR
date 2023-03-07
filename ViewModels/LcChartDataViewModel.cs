using System.ComponentModel.DataAnnotations;

namespace DenimERP.ViewModels
{
    public class LcChartDataViewModel
    {
        [Display(Name = "L/C Quantity")]
        public int ? LCQty { get; set; }
        [Display(Name = "Yesterday L/C Quantity")]
        public double LCQtyY { get; set; }
        [Display(Name = "L/C Value")]
        public double? LCValue { get; set; }
        [Display(Name = "Yesterday L/C Value")]
        public  double ? LCValueY { get; set; }
    }
}
