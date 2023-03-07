using System.ComponentModel.DataAnnotations;

namespace DenimERP.ViewModels
{
    public class PiChartDataViewModel
    {
        [Display(Name = "PI Quantity")]
        public int ? PIQty { get; set; }
        [Display(Name = "PI Value")]
        public double ? PIValue { get; set; }
        [Display(Name = "PI Quantity")]
        public int ? PIQtyY { get; set; }
        [Display(Name = "PI Value")]
        public double ? PIValueY { get; set; }
    }
}
