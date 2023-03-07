using System.ComponentModel.DataAnnotations;

namespace DenimERP.ViewModels
{
    public class InspectionChartDataViewModel
    {
        [Display(Name = "Inspection Length (MTR)")]
        public double InspectionLengthMtr { get; set; }
        [Display(Name = "Inspection Length (YDS)")]
        public double InspectionLengthYds { get; set; }
        [Display(Name = "Total Inspection Length (YDS)")]
        public double TotalInspectionYds { get; set; }
    }
}
