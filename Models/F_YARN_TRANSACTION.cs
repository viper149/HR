using System;
using System.ComponentModel.DataAnnotations;
using DenimERP.Models.BaseModels;

namespace DenimERP.Models

{
    public partial class F_YARN_TRANSACTION : BaseEntity
    {
        public int YTRNID { get; set; }
        [Display(Name = "Trans. Date")]
        public DateTime? YTRNDATE { get; set; }
        [Display(Name = "Count Name")]
        public int? COUNTID { get; set; }
        [Display(Name = "Receive No.")]
        public int? YRCVID { get; set; }
        [Display(Name = "Receive Type")]
        public int? RCVTID { get; set; }
        [Display(Name = "Issue No.")]
        public int? YISSUEID { get; set; }
        [Display(Name = "Issue Type")]
        public int? ISSUEID { get; set; }
        [Display(Name = "Lot No")]
        public int? LOTID { get; set; }
        [Display(Name = "Opening Balance")]
        public double? OP_BALANCE { get; set; }
        [Display(Name = "Receive Quantity")]
        public double? RCV_QTY { get; set; }
        [Display(Name = "Issue Quantity")]
        public double? ISSUE_QTY { get; set; }
        [Display(Name = "Balance")]
        public double? BALANCE { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }
        public string OPT3 { get; set; }
        public int? INDENT_TYPE { get; set;}
        public int? BAG { get; set;}
        public BAS_YARN_COUNTINFO COUNT { get; set; }
        public F_BAS_ISSUE_TYPE ISSUE { get; set; }
        public F_BAS_RECEIVE_TYPE RCVT { get; set; }
        public F_YS_YARN_ISSUE_DETAILS YISSUE { get; set; }
        public F_YS_YARN_RECEIVE_DETAILS YRCV { get; set; }
        public BAS_YARN_LOTINFO LOT { get; set; }
    }
}
