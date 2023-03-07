using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using DenimERP.Models.BaseModels;

namespace DenimERP.Models
{
    public partial class F_YARN_REQ_DETAILS_S : BaseEntity
    {
        public int TRNSID { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [Display(Name = "Trans. Date")]
        public DateTime? TRNSDATE { get; set; }
        [Display(Name = "Yarn Req. No")]
        public int? YSRID { get; set; }
        [Display(Name = "Order No.")]
        public int? ORDERNO { get; set; }
        [Display(Name = "Count Name")]
        public int? COUNTID { get; set; }
        [Display(Name = "Unit")]
        public int? UNIT { get; set; }
        [Display(Name = "Required Quantity")]
        [Range(0.0, double.MaxValue, ErrorMessage = "The field {0} must be greater than {1}.")]
        public double? REQ_QTY { get; set; }
        [Display(Name = "Set / Prog. No.")]
        public int? SETID { get; set; }
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }
        public string OPT3 { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }
        [Display(Name = "Lot No.")]
        public int? LOTID { get; set; }

        [NotMapped]
        public string DateFormat
        {
            get
            {
                var dateTimeFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern.ToUpper();
                return !string.IsNullOrEmpty(dateTimeFormat) ? dateTimeFormat : "DD-MMM-YY";
            }
        }

        public RND_FABRIC_COUNTINFO COUNT { get; set; }
        public BAS_YARN_LOTINFO LOT { get; set; }
        public RND_PRODUCTION_ORDER ORDERNONavigation { get; set; }
        public PL_PRODUCTION_SETDISTRIBUTION SET { get; set; }
        public F_BAS_UNITS UNITNavigation { get; set; }
        public F_YARN_REQ_MASTER_S YSR { get; set; }
        public F_YS_YARN_ISSUE_DETAILS_S F_YS_YARN_ISSUE_DETAILS_S { get; set; }
    }
}
