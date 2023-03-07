using System;
using DenimERP.Models.BaseModels;

namespace DenimERP.Models
{
    public partial class F_YARN_TRANSACTION_S : BaseEntity
    {
        public int YTRNID { get; set; }
        public DateTime? YTRNDATE { get; set; }
        public int? COUNTID { get; set; }
        public int? YRCVID { get; set; }
        public int? RCVTID { get; set; }
        public int? YISSUEID { get; set; }
        public int? ISSUEID { get; set; }
        public double? OP_BALANCE { get; set; }
        public double? RCV_QTY { get; set; }
        public double? ISSUE_QTY { get; set; }
        public double? BALANCE { get; set; }
        public string REMARKS { get; set; }
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }
        public string OPT3 { get; set; }
        public int? LOTID { get; set; }

        public BAS_YARN_COUNTINFO COUNT { get; set; }
        public F_BAS_ISSUE_TYPE ISSUE { get; set; }
        public BAS_YARN_LOTINFO LOT { get; set; }
        public F_BAS_RECEIVE_TYPE RCVT { get; set; }
        public F_YS_YARN_ISSUE_DETAILS_S YISSUE { get; set; }
        public F_YS_YARN_RECEIVE_DETAILS_S YRCV { get; set; }
    }
}
