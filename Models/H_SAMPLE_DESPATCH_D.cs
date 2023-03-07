using System.ComponentModel.DataAnnotations;
using DenimERP.Models.BaseModels;

namespace DenimERP.Models
{
    public partial class H_SAMPLE_DESPATCH_D : BaseEntity
    {
        public int SDDID { get; set; }
        public int? SDID { get; set; }
        public int RCVDID { get; set; }
        [Display(Name = "Quantity")]
        [Required(ErrorMessage = "The field {0} can not be empty.")]
        public double? QTY { get; set; }
        [Display(Name = "C. S. Price")]
        public string CS_PRICE { get; set; }
        [Display(Name = "Negotiation Price")]
        public string NEGO_PRICE { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }
        [Display(Name = "Barcode")]
        public string BARCODE { get; set; }

        public H_SAMPLE_RECEIVING_D RCVD { get; set; }
        public H_SAMPLE_DESPATCH_M SD { get; set; }
    }
}