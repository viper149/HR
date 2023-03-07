using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace DenimERP.Models
{
    public partial class PROC_BILL_DETAILS
    {
        public int BDID { get; set; }
        [Display(Name = "Bill Details Date")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? BDDATE { get; set; }
        [Display(Name = "")]
        public int? TRANSID { get; set; }
        [Display(Name = "")]
        public int? BILLID { get; set; }
        [Display(Name = "Product Code")]
        public int? PRODID { get; set; }
        [Display(Name = "Product Name")]
        public string PRODUCT_NAME { get; set; }
        [Display(Name = "Unit")]
        public string UNIT { get; set; }
        [Display(Name = "Indent No.")]
        public int? INDID { get; set; }
        [Display(Name = "QTY")]
        public double? QTY { get; set; }
        [Display(Name = "Rate")]
        public double? RATE { get; set; }
        [Display(Name = "Amount")]
        public double? AMOUNT { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }
        public string OPT3 { get; set; }
        public string OPT4 { get; set; }
        public string OPT5 { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime? CREATED_AT { get; set; }
        public string UPDATED_BY { get; set; }
        public DateTime? UPDATED_AT { get; set; }

        [NotMapped]
        public string DateFormat
        {
            get
            {
                var dateTimeFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern.ToUpper();
                return !string.IsNullOrEmpty(dateTimeFormat) ? dateTimeFormat : "DD-MMM-YY";
            }
        }

        public PROC_BILL_MASTER BILL { get; set; }
        //public F_GS_INDENT_MASTER IND { get; set; }
        public F_GS_PRODUCT_INFORMATION PROD { get; set; }
        //public F_GS_PURCHASE_REQUISITION_DETAILS TRANS { get; set; }
    }
}
