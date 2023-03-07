using System.ComponentModel.DataAnnotations;

namespace DenimERP.ViewModels.Com.InvoiceExport
{
    public class ComExInvoiceMasterPiListViewModel
    {
        [Display(Name = "PI No.")]
        public string PINO { get; set; }
        [Display(Name = "Style Name")]
        public string STYLENAME { get; set; }
        [Display(Name = "Unit Price")]
        public double? UNITPRICE { get; set; }
        [Display(Name = "Qty.")]
        public decimal? QTY { get; set; }
        [Display(Name = "Total")]
        public double? TOTAL { get; set; }
    }
}
