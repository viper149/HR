using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using DenimERP.Models.BaseModels;

namespace DenimERP.Models
{
    public partial class F_GEN_S_INDENTDETAILS : BaseEntity
    {
        public int TRNSID { get; set; }
        [Display(Name = "Trans. Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? TRNSDATE { get; set; }
        [Display(Name = "Indent No.")]
        public int? GINDID { get; set; }
        [Display(Name = "Indent Sl No.")]
        public int? INDSLID { get; set; }
        [Display(Name = "Product Name")]
        public int? PRODUCTID { get; set; }
        public int? UNIT { get; set; }
        [Display(Name = "Requisition Quantity")]
        [Range(1.0, double.MaxValue, ErrorMessage = "The field {0} must be greater than {1}")]
        public double? QTY { get; set; }
        [Display(Name = "Validity")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? VALIDITY { get; set; }
        [Display(Name = "Indent Quantity")]
        [Range(1.0, double.MaxValue, ErrorMessage = "The field {0} must be greater than {1}")]
        public string FULL_QTY { get; set; }
        [Display(Name = "Additional Quantity")]
        public string ADD_QTY { get; set; }
        [Display(Name = "Balance")]
        public string BAL_QTY { get; set; }
        [Display(Name = "Indent Book & Page No.")]
        public string LOCATION { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }
        public string OPT3 { get; set; }

        [NotMapped]
        public string DateFormat
        {
            get
            {
                var dateTimeFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern.ToUpper();
                return !string.IsNullOrEmpty(dateTimeFormat) ? dateTimeFormat : "DD-MMM-YY";
            }
        }
        [NotMapped] 
        public string Expire { get; set; }

        public F_GEN_S_INDENTMASTER GIND { get; set; }
        public F_GS_PRODUCT_INFORMATION PRODUCT { get; set; }
        public F_GEN_S_PURCHASE_REQUISITION_MASTER INDSL { get; set; }
    }
}
