using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using DenimERP.Models.BaseModels;

namespace DenimERP.Models
{
    public partial class F_YS_YARN_ISSUE_DETAILS_S : BaseEntity
    {
        public F_YS_YARN_ISSUE_DETAILS_S()
        {
            F_YARN_TRANSACTION_S = new HashSet<F_YARN_TRANSACTION_S>();
        }

        public int TRANSID { get; set; }
        [Display(Name = "Trans. Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? TRNSDATE { get; set; }
        [Display(Name = "Count Name")]
        public int? COUNTID { get; set; }
        [Display(Name = "Lot No.")]
        public int? LOTID { get; set; }
        [Display(Name = "Issue No.")]
        public int? YISSUEID { get; set; }
        [Display(Name = "Req. Details No.")]
        public int? REQ_DET_ID { get; set; }
        [Display(Name = "Unit")]
        public int? UNIT { get; set; }
        [Display(Name = "Issue Quantity")]
        public double? ISSUE_QTY { get; set; }
        [Display(Name = "Challan No - Rcv Qty")]
        public int? RCVDID { get; set; }
        [Display(Name = "Bags")]
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }
        public string OPT3 { get; set; }
        public string REMARKS { get; set; }
        [Display(Name = "Location")]
        public int? LOCATIONID { get; set; }
        [Display(Name = "Main Count")]
        public int? MAIN_COUNTID { get; set; }

        [NotMapped]
        public string DateFormat
        {
            get
            {
                var dateTimeFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern.ToUpper();
                return !string.IsNullOrEmpty(dateTimeFormat) ? dateTimeFormat : "DD-MMM-YY";
            }
        }

        public BAS_YARN_COUNTINFO COUNT { get; set; }
        public BAS_YARN_COUNTINFO RefBasYarnCountinfo { get; set; }
        public F_YS_LOCATION LOCATION { get; set; }
        public BAS_YARN_LOTINFO LOT { get; set; }
        public F_YARN_REQ_DETAILS_S TRANS { get; set; }
        public F_BAS_UNITS UNITNavigation { get; set; }
        public F_YS_YARN_ISSUE_MASTER_S YISSUE { get; set; }
        public F_YS_YARN_RECEIVE_DETAILS_S RCVD { get; set; }

        public ICollection<F_YARN_TRANSACTION_S> F_YARN_TRANSACTION_S { get; set; }
    }
}
