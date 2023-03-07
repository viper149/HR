using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using DenimERP.Models.BaseModels;

namespace DenimERP.Models
{
    public partial class F_GEN_S_RECEIVE_DETAILS : BaseEntity
    {
        public F_GEN_S_RECEIVE_DETAILS()
        {
            F_GEN_S_ISSUE_DETAILS = new HashSet<F_GEN_S_ISSUE_DETAILS>();
        }

        public int TRNSID { get; set; }
        [Display(Name = "Trans. Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? TRNSDATE { get; set; }
        [Display(Name = "Receive No.")]
        public int? GRCVID { get; set; }
        [Display(Name = "Product Name")]
        public int? PRODUCTID { get; set; }
        [Display(Name = "Adjusted With")]
        public int? ADJUSTED_WITH { get; set; }
        [Display(Name = "Unit")]
        public int? UNIT { get; set; }
        [Display(Name = "Indent No")]
        public int? GINDID { get; set; }
        [Display(Name = "Invoice Quantity")]
        public double? INVQTY { get; set; }
        [Display(Name = "Rate")]
        public double? RATE { get; set; }
        [Display(Name = "Currency")]
        public string CURRENCY { get; set; }
        [Display(Name = "Amount")]
        public double? AMOUNT { get; set; }
        [Display(Name = "Batch No")]
        public string BATCHNO { get; set; }
        [Display(Name = "Manufacture Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? MNGDATE { get; set; }
        [Display(Name = "Expire Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? EXDATE { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }
        [Display(Name = "Fresh Quantity")]
        [Range(1, double.MaxValue, ErrorMessage = "The field {0} must between {1} ~ {2}")]
        public double? FRESH_QTY { get; set; }
        [Display(Name = "Rejection Quantity")]
        [Range(1, double.MaxValue, ErrorMessage = "The field {0} must between {1} ~ {2}")]
        public double? REJ_QTY { get; set; }
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }

        [NotMapped]
        public string DateFormat
        {
            get
            {
                var dateTimeFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern.ToUpper();
                return !string.IsNullOrEmpty(dateTimeFormat) ? dateTimeFormat : "DD-MMM-YY";
            }
        }

        public F_GEN_S_INDENTMASTER GIND { get; set; }
        public F_GEN_S_RECEIVE_MASTER GRCV { get; set; }
        public F_GS_PRODUCT_INFORMATION PRODUCT { get; set; }

        public ICollection<F_GEN_S_ISSUE_DETAILS> F_GEN_S_ISSUE_DETAILS { get; set; }
    }
}
