using System;
using System.ComponentModel.DataAnnotations;

namespace DenimERP.Models
{
    public partial class F_YS_YARN_RECEIVE_DETAILS2
    {
        public int TRNSID { get; set; }
        public DateTime? TRNSDATE { get; set; }
        public int? YRCVID { get; set; }
        [Display(Name = "Count Name")]
        public int? COUNTID { get; set; }
        [Display(Name = "Lot No")]
        public int? LOT { get; set; }
        [Display(Name = "Bag Qty")]
        public double? BAG_QTY { get; set; }
        [Display(Name = "Receive Kg")]
        public double? RCV_QTY { get; set; }
        [Display(Name = "Location")]
        public int? LOCATIONID { get; set; }
        [Display(Name = "Ledger")]
        public int? LEDGERID { get; set; }
        [Display(Name = "Raw")]
        public int? RAW { get; set; }
        [Display(Name = "Page No")]
        public int? PAGENO { get; set; }
        public string REAMRKS { get; set; }
        public int? OPT1 { get; set; }
        public string OPT2 { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime? CREATED_AT { get; set; }
        public string UPDATED_BY { get; set; }
        public DateTime? UPDATED_AT { get; set; }

        public BAS_YARN_COUNTINFO COUNT { get; set; }
        public F_YS_LEDGER LEDGER { get; set; }
        public F_YS_LOCATION LOCATION { get; set; }
        public BAS_YARN_LOTINFO LOTNavigation { get; set; }
        public F_YS_RAW_PER RAWNavigation { get; set; }
        public F_YS_YARN_RECEIVE_MASTER2 YRCV { get; set; }
    }
}
