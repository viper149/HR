using System.ComponentModel.DataAnnotations;

namespace DenimERP.ViewModels
{
    public class ComExPIMasterAndDetailsViewModel
    {
        [Display(Name = "Style")]
        public int? STYLEID { get; set; }
        public string STYLENAME { get; set; }
        public string UNIT { get; set; }
        [Display(Name = "Cost Ref.")]
        public int? COSTID { get; set; }
        public int? QTY { get; set; }
        public double? UNITPRICE { get; set; }
        public double? TOTAL { get; set; }
        public string REMARKS { get; set; }
        public int OverHead { get; set; }
        public bool SO_STATUS { get; set; }
    }
}
