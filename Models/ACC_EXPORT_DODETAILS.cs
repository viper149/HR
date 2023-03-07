using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace DenimERP.Models
{
    public partial class ACC_EXPORT_DODETAILS
    {
        public int TRNSID { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [Display(Name = "Trans. Date")]
        public DateTime TRNSDATE { get; set; }
        [Display(Name = "DO No.")]
        public int? DONO { get; set; }
        [Display(Name = "PI No")]
        public int? PIID { get; set; }
        [NotMapped]
        [Display(Name = "PI No")]
        public string PINO { get; set; }
        [Display(Name = "Style")]
        public int? STYLEID { get; set; }
        [Display(Name = "Style")]
        public int? SOID { get; set; }
        [NotMapped]
        [Display(Name = "Style")]
        public string STYLENAME { get; set; }
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
        [Display(Name = "Oracle Do Delivery")]
        public double? ORACLE_DO_DELIVERY { get; set; }

        [NotMapped]
        public string DateFormat
        {
            get
            {
                var dateTimeFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern.ToUpper();
                return !string.IsNullOrEmpty(dateTimeFormat) ? dateTimeFormat : "DD-MMM-YY";
            }
        }

        public COM_EX_PIMASTER PI { get; set; }
        public ACC_EXPORT_DOMASTER DO { get; set; }
        public COM_EX_FABSTYLE STYLE { get; set; }
        public COM_EX_PI_DETAILS SO { get; set; }
    }
}
