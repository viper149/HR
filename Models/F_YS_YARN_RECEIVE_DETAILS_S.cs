using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using DenimERP.Models.BaseModels;

namespace DenimERP.Models
{
    public partial class F_YS_YARN_RECEIVE_DETAILS_S : BaseEntity
    {
        public F_YS_YARN_RECEIVE_DETAILS_S()
        {
            F_YARN_QC_APPROVE_S = new HashSet<F_YARN_QC_APPROVE_S>();
            F_YARN_TRANSACTION_S = new HashSet<F_YARN_TRANSACTION_S>();
            F_YS_YARN_RECEIVE_REPORT_S = new HashSet<F_YS_YARN_RECEIVE_REPORT_S>();
            F_YS_YARN_ISSUE_DETAILS_S = new HashSet<F_YS_YARN_ISSUE_DETAILS_S>();
        }

        public int TRNSID { get; set; }
        [Display(Name = "Trans. Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? TRNSDATE { get; set; }
        [Display(Name = "Yarn Receive No.")]
        public int? YRCVID { get; set; }
        [Display(Name = "Product No")]
        public int? PRODID { get; set; }
        [Display(Name = "Challan Quantity")]
        public double? INV_QTY { get; set; }
        [Display(Name = "Lot No.")]
        public int? LOT { get; set; }
        [Display(Name = "Bag Quantity")]
        public double? BAG_QTY { get; set; }
        [Display(Name = "Receive Quantity")]
        public double? RCV_QTY { get; set; }
        [Display(Name = "Reject Quantity")]
        public double? REJ_QTY { get; set; }
        [Display(Name = "Location")]
        public int? LOCATIONID { get; set; }
        [Display(Name = "Ledger")]
        public int? LEDGERID { get; set; }
        [Display(Name = "Page No.")]
        public int? PAGENO { get; set; }
        [Display(Name = "Raw(%)")]
        public int? RAW { get; set; }
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

        public F_YS_LEDGER LEDGER { get; set; }
        public F_YS_LOCATION LOCATION { get; set; }
        public BAS_YARN_LOTINFO LOTNavigation { get; set; }
        public BAS_YARN_COUNTINFO PROD { get; set; }
        public F_YS_RAW_PER RAWNavigation { get; set; }
        public F_YS_YARN_RECEIVE_MASTER_S YRCV { get; set; }

        public ICollection<F_YARN_QC_APPROVE_S> F_YARN_QC_APPROVE_S { get; set; }
        public ICollection<F_YARN_TRANSACTION_S> F_YARN_TRANSACTION_S { get; set; }
        public ICollection<F_YS_YARN_RECEIVE_REPORT_S> F_YS_YARN_RECEIVE_REPORT_S { get; set; }
        public ICollection<F_YS_YARN_ISSUE_DETAILS_S> F_YS_YARN_ISSUE_DETAILS_S { get; set; }
    }
}
