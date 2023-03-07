using System.ComponentModel.DataAnnotations;

namespace DenimERP.Models
{
    public partial class H_SAMPLE_FABRIC_DISPATCH_DETAILS
    {
        public int SFDDID { get; set; }
        public int SFDID { get; set; }
        [Display(Name = "Barcode")]
        public string BARCODE { get; set; }
        public int? RCVDID { get; set; }
        [Display(Name = "Qty.")]
        public double? DEL_QTY { get; set; }
        [Display(Name = "C. S. Price")]
        public double? CSPRICE { get; set; }
        [Display(Name = "Nego Price")]
        public double? NEGO_PRICE { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }

        public H_SAMPLE_FABRIC_DISPATCH_MASTER SFD { get; set; }
        public H_SAMPLE_FABRIC_RECEIVING_D RCVD { get; set; }
    }
}
