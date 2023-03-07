using System;
using System.ComponentModel.DataAnnotations;

namespace DenimERP.ViewModels.Com.InvoiceExport
{
    public class ComExInvoiceMasterInvoiceListViewModel
    {
        [Display(Name = "Invoice No")]
        public string INVNO { get; set; }
        [Display(Name = "P.Doc No.")]
        public string DOCNO { get; set; }
        [Display(Name = "OD Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? ODRCVDATE { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [Display(Name = "Invoice Date")]
        public DateTime? INVDATE { get; set; }
        [Display(Name = "Doc Neg. Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? NEGODATE { get; set; }
        [Display(Name = "Qty.")]
        public decimal? QTY { get; set; }
        [Display(Name = "Amount")]
        public double? AMOUNT { get; set; }
    }
}
