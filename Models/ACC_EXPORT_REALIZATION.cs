using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace DenimERP.Models
{
    public partial class ACC_EXPORT_REALIZATION
    {
        public int TRNSID { get; set; }
        [NotMapped]
        public string EncryptedId { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [Display(Name = "Date")]
        public DateTime? TRNSDATE { get; set; }
        [Display(Name = "Invoice No")]
        public int INVID { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [Display(Name = "Realization Date")]
        public DateTime? REZDATE { get; set; }
        [Required(ErrorMessage = "Please Enter PRC Amount($)")]
        [Display(Name = "PRC Amount($)")]
        public double? PRC_USD { get; set; }
        [Display(Name = "PRC Amount(€)")]
        public double? PRC_EURO { get; set; }
        [Display(Name = "ERQ($)")]
        public double? ERQ { get; set; }
        [Display(Name = "ORQ($)")]
        public double? ORQ { get; set; }
        [Display(Name = "CD($)")]
        public double? CD { get; set; }
        [Display(Name = "OD")]
        public double? OD { get; set; }
        [Display(Name = "ATT TK")]
        public double? AITTK { get; set; }
        [Display(Name = "Rate($)")]
        public double? RATE { get; set; }
        [Display(Name = "ATT($)")]
        public double? AIT { get; set; }
        [Display(Name = "B. Interest($)")]
        public double? BINTEREST { get; set; }
        [Display(Name = "B. Charge")]
        public double? BCHARGE { get; set; }
        [Display(Name = "B. Comm($)")]
        public double? BCOMM { get; set; }
        [Display(Name = "Disc. Vou. No.")]
        public string DISVOU { get; set; }
        [Display(Name = "Real. Vou. No.")]
        public string REALVOU { get; set; }
        [Display(Name = "Notes")]
        public string NOTES { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }
        public string ISDELETE { get; set; }
        [Display(Name = "Author")]
        public string USRID { get; set; }
        [Display(Name = "Audit By")]
        public string AUDITBY { get; set; }
        [Display(Name = "Audit Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? AUDITDATE { get; set; }

        [NotMapped]
        public string DateFormat
        {
            get
            {
                var dateTimeFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern.ToUpper();
                return !string.IsNullOrEmpty(dateTimeFormat) ? dateTimeFormat : "DD-MMM-YY";
            }
        }

        public COM_EX_INVOICEMASTER INVOICE { get; set; }
    }
}
