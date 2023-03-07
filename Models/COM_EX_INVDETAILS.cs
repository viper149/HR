using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DenimERP.Models
{
    public partial class COM_EX_INVDETAILS
    {
        public int TRNSID { get; set; }
        [Display(Name = "Invoice No")]
        public string INVNO { get; set; }
        public int? INVID { get; set; }
        [Display(Name = "Style")]
        public int? STYLEID { get; set; }
        [Display(Name = "Roll")]
        public decimal? ROLL { get; set; }
        [Display(Name = "Qty.")]
        public decimal? QTY { get; set; }
        [Display(Name = "Rate")]
        public double? RATE { get; set; }
        [Display(Name = "Amount")]
        public double? AMOUNT { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }
        [Display(Name = "Author")]
        public string USRID { get; set; }
        public int? PIIDD_TRNSID { get; set; }
        public bool IS_OLD { get; set; }
        [Display(Name = "Is Final")]
        public bool IS_FINAL { get; set; }

        [NotMapped]
        public double PREV_QTY { get; set; }

        public COM_EX_FABSTYLE ComExFabstyle { get; set; }
        public COM_EX_INVOICEMASTER InvoiceMaster { get; set; }
        public COM_EX_PI_DETAILS PiDetails { get; set; }
    }
}
