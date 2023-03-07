using System.ComponentModel.DataAnnotations;
using DenimERP.Models.BaseModels;

namespace DenimERP.Models
{
    public partial class F_GS_GATEPASS_RETURN_RCV_DETAILS : BaseEntity
    {
        public int TRNSID { get; set; }
        [Display(Name = "Gate Pass Issue No.")]
        public int? RCVID { get; set; }
        [Display(Name = "Product")]
        [Required(ErrorMessage = "{0} must be selected")]
        public int? PRODID { get; set; }
        [Display(Name = "Received Quantity")]
        public double? RCV_QTY { get; set; }
        [Display(Name = "No. of Bags")]
        public int? RCV_BAG { get; set; }
        [Display(Name = "Gate Entry No.")]
        public string GATE_ENTRYNO { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }

        public F_GS_PRODUCT_INFORMATION PROD { get; set; }
        public F_GS_GATEPASS_RETURN_RCV_MASTER RCV { get; set; }
    }
}
