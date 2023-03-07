using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DenimERP.Models.BaseModels;

namespace DenimERP.Models

{
    public partial class F_YS_YARN_ISSUE_DETAILS : BaseEntity
    {
        public F_YS_YARN_ISSUE_DETAILS()
        {
            F_YARN_TRANSACTION = new HashSet<F_YARN_TRANSACTION>();
        }

        public int TRANSID { get; set; }
        [Display(Name = "Trans. Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? TRNSDATE { get; set; }
        [Display(Name = "Issue No.")]
        public int? YISSUEID { get; set; }
        [Display(Name = "Lot No.")]
        public int? LOTID { get; set; }
        [Display(Name = "Location")]
        public int? LOCATIONID { get; set; }
        [Display(Name = "Main Count")]
        public int? REQ_DET_ID { get; set; }
        [Display(Name = "Count Name")]
        public int? COUNTID { get; set; }
        [Display(Name = "Unit")]
        public int? UNIT { get; set; }
        [Display(Name = "Main Count")]
        public int? MAIN_COUNTID { get; set; }
        [Display(Name = "Issue Quantity")]
        public double? ISSUE_QTY { get; set; }
        [Display(Name = "Indent No - Qty")]
        public int? RCVDID { get; set; }
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }
        public string OPT3 { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }
        public int? INDENT_TYPE { get; set; }
        public int? POID { get; set; }
        public int? SDRFID { get; set; }
        [Display(Name = "Bag Quantity")]
        public int? BAG { get; set; }
        [DisplayName("Order No")]
        public int? SO_NO { get; set; }
        [NotMapped]
        public  double ? TotalIssuedCount { get; set; }

        public F_BAS_UNITS FBasUnits { get; set; }
        public F_YS_YARN_ISSUE_MASTER YISSUE { get; set; }
        public F_YARN_REQ_DETAILS TRANS { get; set; }
        public BAS_YARN_COUNTINFO BasYarnCountinfo { get; set; }
        public BAS_YARN_COUNTINFO RefBasYarnCountinfo { get; set; }
        public BAS_YARN_LOTINFO LOT { get; set; }
        public F_YS_LOCATION FYsLocation { get; set; }
        public F_YS_YARN_RECEIVE_DETAILS RCVD { get; set; }
        public RND_PRODUCTION_ORDER PO_EXTRA { get; set; }

        public ICollection<F_YARN_TRANSACTION> F_YARN_TRANSACTION { get; set; }
    }
}