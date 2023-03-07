using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using DenimERP.Models.BaseModels;

namespace DenimERP.Models
{
    public partial class F_GS_GATEPASS_INFORMATION_D : BaseEntity
    {
        public int TRNSID { get; set; }
        public int? GPID { get; set; }
        [Display(Name = "Product")]
        public int? PRODID { get; set; }
        [Display(Name = "Product Quantity")]
        public double? ISSUE_QTY { get; set; }
        [Display(Name = "Bag")]
        [Range(0, int.MaxValue, ErrorMessage = "The field {0} must be greater than {1}.")]
        public int? ISSUE_BAG { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? ETR { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }

        [NotMapped]
        [Display(Name = "Unit")]
        public string UNIT_NM { get; set; }

        [NotMapped]
        public string DateFormat
        {
            get
            {
                var dateTimeFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern.ToUpper();
                return !string.IsNullOrEmpty(dateTimeFormat) ? dateTimeFormat : "DD-MMM-YY";
            }
        }

        public F_GS_GATEPASS_INFORMATION_M GP { get; set; }
        public F_GS_PRODUCT_INFORMATION PROD { get; set; }
    }
}
