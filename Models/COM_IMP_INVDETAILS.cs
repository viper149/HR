using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DenimERP.Models
{
    public partial class COM_IMP_INVDETAILS
    {
        public int TRNSID { get; set; }
        [NotMapped]
        public string EncryptedId { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        //[Required(ErrorMessage = "Please provide a specific Transport Date.")]
        [Display(Name = "Trns. Date")]
        public DateTime TRNSDATE { get; set; }
        public int? INVID { get; set; }
        [Display(Name = "Invoice No")]
        public string INVNO { get; set; }
        [Display(Name = "Product Name")]
        public int? PRODID { get; set; }
        [Display(Name = "Chemical Product ID")]
        public int? CHEMPRODID { get; set; }
        [Display(Name = "Yarn Lot ID")]
        public int? YARNLOTID { get; set; }
        [Display(Name = "Unit")]
        public int? UNIT { get; set; }
        [Display(Name = "Qty")]
        //[Range(typeof(int), "1", "2147483647", ErrorMessage = "The field {0} must be a decimal/number between {1} and {2}.")]
        public double? QTY { get; set; }
        [Display(Name = "Rate")]
        public double? RATE { get; set; }
        [Display(Name = "Amount")]
        public double? AMOUNT { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }
        [Display(Name = "Author")]
        public string USRID { get; set; }

        public COM_IMP_INVOICEINFO ComImpInvoiceinfo { get; set; }
        public BAS_PRODUCTINFO BasProductinfo { get; set; }
        public F_BAS_UNITS F_BAS_UNITS { get; set; }
        public F_CHEM_STORE_PRODUCTINFO F_CHEM_STORE_PRODUCTINFOS { get; set; }
        public BAS_YARN_LOTINFO BAS_YARN_LOTINFOS { get; set; }
    }
}
