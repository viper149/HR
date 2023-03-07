using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DenimERP.Models.BaseModels;

namespace DenimERP.Models
{
    public partial class RND_PRODUCTION_ORDER : BaseEntity
    {
        public RND_PRODUCTION_ORDER()
        {
            //PL_ORDERWISE_LOTINFO = new HashSet<PL_ORDERWISE_LOTINFO>();
            PL_PRODUCTION_SO_DETAILS = new HashSet<PL_PRODUCTION_SO_DETAILS>();
            PL_BULK_PROG_SETUP_M = new HashSet<PL_BULK_PROG_SETUP_M>();
            F_YARN_REQ_DETAILS = new HashSet<F_YARN_REQ_DETAILS>();
            //F_YS_YARN_RECEIVE_MASTER = new HashSet<F_YS_YARN_RECEIVE_MASTER>();
            RND_PURCHASE_REQUISITION_MASTER = new HashSet<RND_PURCHASE_REQUISITION_MASTER>();
            F_FS_FABRIC_CLEARENCE_2ND_BEAM = new HashSet<F_FS_FABRIC_CLEARENCE_2ND_BEAM>();
            F_YARN_REQ_DETAILS_S = new HashSet<F_YARN_REQ_DETAILS_S>();
            F_FS_FABRIC_CLEARANCE_MASTER = new HashSet<F_FS_FABRIC_CLEARANCE_MASTER>();
            F_PR_WEAVING_PRODUCTION = new HashSet<F_PR_WEAVING_PRODUCTION>();
            F_YS_YARN_ISSUE_DETAILS = new HashSet<F_YS_YARN_ISSUE_DETAILS>();
            COS_POSTCOSTING_MASTER = new HashSet<COS_POSTCOSTING_MASTER>();
            F_PR_WEAVING_OS = new HashSet<F_PR_WEAVING_OS>();

            F_YS_YARN_RECEIVE_MASTER2 = new HashSet<F_YS_YARN_RECEIVE_MASTER2>();
        }

        public int POID { get; set; }
        [NotMapped]
        public string EncryptedId { get; set; }
        [Display(Name = "Order No.")]
        public int? ORDERNO { get; set; }
        [Display(Name = "RS No")]
        public int? RSNO { get; set; }
        [Display(Name = "Order Type")]
        public int? OTYPEID { get; set; }
        [Display(Name = "Order PI Received Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? PIRCVDATE { get; set; }
        [Display(Name = "Order Repeat")]
        public int? ORPTID { get; set; }
        [Display(Name = "Order Qty(Y)")]
        public double? ORDER_QTY_YDS { get; set; }
        [Display(Name = "Order Qty(M)")]
        public double? ORDER_QTY_MTR { get; set; }
        [Display(Name = "Warp Length(M)")]
        public double? WARP_LENGTH_MTR { get; set; }
        [Display(Name = "Grey Length(M)")]
        public double? GREY_LENGTH_MTR { get; set; }
        [Display(Name = "Dyeing Type")]
        public string DYENG_TYPE { get; set; }
        [Display(Name = "Loom Type")]
        public string LOOM_TYPE { get; set; }
        [Display(Name = "No of Ball")]
        public string NO_OF_BALL { get; set; }
        [Display(Name = "Total Ends")]
        public double? TOTAL_ENOS { get; set; }
        [Display(Name = "Master Roll")]
        public int? MID { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }
        public string OPT3 { get; set; }
        public string OPT4 { get; set; }
        [NotMapped]
        public string OPT5 { get; set; }
        [NotMapped]
        public string OPT6 { get; set; }
        [NotMapped]
        public string OPT7 { get; set; }

        public RND_MSTR_ROLL M { get; set; }
        public RND_ORDER_REPEAT ORPT { get; set; }
        public RND_ORDER_TYPE OTYPE { get; set; }
        public COM_EX_PI_DETAILS SO { get; set; }
        public RND_SAMPLE_INFO_DYEING RS { get; set; }

        //public ICollection<PL_ORDERWISE_LOTINFO> PL_ORDERWISE_LOTINFO { get; set; }
        public ICollection<PL_PRODUCTION_SO_DETAILS> PL_PRODUCTION_SO_DETAILS { get; set; }
        public ICollection<PL_BULK_PROG_SETUP_M> PL_BULK_PROG_SETUP_M { get; set; }
        public ICollection<F_YARN_REQ_DETAILS> F_YARN_REQ_DETAILS { get; set; }
        //public ICollection<F_YS_YARN_RECEIVE_MASTER> F_YS_YARN_RECEIVE_MASTER { get; set; }
        public ICollection<RND_PURCHASE_REQUISITION_MASTER> RND_PURCHASE_REQUISITION_MASTER { get; set; }
        public ICollection<F_FS_FABRIC_CLEARENCE_2ND_BEAM> F_FS_FABRIC_CLEARENCE_2ND_BEAM { get; set; }
        public ICollection<F_YARN_REQ_DETAILS_S> F_YARN_REQ_DETAILS_S { get; set; }
        public ICollection<F_FS_FABRIC_CLEARANCE_MASTER> F_FS_FABRIC_CLEARANCE_MASTER { get; set; }
        public ICollection<F_PR_WEAVING_PRODUCTION> F_PR_WEAVING_PRODUCTION { get; set; }
        public ICollection<F_YS_YARN_ISSUE_DETAILS> F_YS_YARN_ISSUE_DETAILS { get; set; }
        public ICollection<COS_POSTCOSTING_MASTER> COS_POSTCOSTING_MASTER { get; set; }
        public ICollection<F_PR_WEAVING_OS> F_PR_WEAVING_OS { get; set; }
        public ICollection<F_YS_YARN_RECEIVE_MASTER2> F_YS_YARN_RECEIVE_MASTER2 { get; set; }
    }
}
