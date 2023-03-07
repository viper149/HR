using System.ComponentModel.DataAnnotations;

namespace DenimERP.ViewModels
{
    public class WeavingChartDataViewModel
    {
        [Display(Name = "Rapier")]
        public double WeavingProductionRapier { get; set; }
        [Display(Name = "Airjet")]
        public double WeavingProductionAirjet { get; set; }
    }
}
