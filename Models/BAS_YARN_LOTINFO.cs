using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DenimERP.Models
{
    public partial class BAS_YARN_LOTINFO
    {
        public BAS_YARN_LOTINFO()
        {
            RND_FABRIC_COUNTINFO = new HashSet<RND_FABRIC_COUNTINFO>();
            RND_SAMPLE_INFO_DETAILS = new HashSet<RND_SAMPLE_INFO_DETAILS>();
            RND_SAMPLE_INFO_WEAVING_DETAILS = new HashSet<RND_SAMPLE_INFO_WEAVING_DETAILS>();
            PL_ORDERWISE_LOTINFO = new HashSet<PL_ORDERWISE_LOTINFO>();
            RND_MSTR_ROLL = new HashSet<RND_MSTR_ROLL>();
            PL_BULK_PROG_YARN_D = new HashSet<PL_BULK_PROG_YARN_D>();
            PL_PRODUCTION_PLAN_DETAILS = new HashSet<PL_PRODUCTION_PLAN_DETAILS>();
            F_PR_WEAVING_WEFT_YARN_CONSUM_DETAILS = new HashSet<F_PR_WEAVING_WEFT_YARN_CONSUM_DETAILS>();
            BAS_YARN_COUNT_LOT_INFO = new HashSet<BAS_YARN_COUNT_LOT_INFO>();
            LOOM_SETTING_CHANNEL_INFO = new HashSet<LOOM_SETTING_CHANNEL_INFO>();
            FYsYarnReceiveDetailses = new HashSet<F_YS_YARN_RECEIVE_DETAILS>();
            F_YS_INDENT_DETAILS = new HashSet<F_YS_INDENT_DETAILS>();
            F_YARN_TRANSACTION = new HashSet<F_YARN_TRANSACTION>();
            F_YS_YARN_ISSUE_DETAILS = new HashSet<F_YS_YARN_ISSUE_DETAILS>();
            YARNLOTCOM_IMP_INVDETAILS = new HashSet<COM_IMP_INVDETAILS>();
            COM_IMP_WORK_ORDER_DETAILS = new HashSet<COM_IMP_WORK_ORDER_DETAILS>();
            F_YARN_REQ_DETAILS = new HashSet<F_YARN_REQ_DETAILS>();
            F_QA_FIRST_MTR_ANALYSIS_D = new HashSet<F_QA_FIRST_MTR_ANALYSIS_D>();
            F_YARN_REQ_DETAILS_S = new HashSet<F_YARN_REQ_DETAILS_S>();
            F_YARN_TRANSACTION_S = new HashSet<F_YARN_TRANSACTION_S>();
            F_YS_YARN_ISSUE_DETAILS_S = new HashSet<F_YS_YARN_ISSUE_DETAILS_S>();
            F_YS_YARN_RECEIVE_DETAILS_S = new HashSet<F_YS_YARN_RECEIVE_DETAILS_S>();
            COS_POSTCOSTING_YARNDETAILS = new HashSet<COS_POSTCOSTING_YARNDETAILS>();
            F_YS_GP_DETAILS = new HashSet<F_YS_GP_DETAILS>();
            F_PR_WEAVING_OS = new HashSet<F_PR_WEAVING_OS>();
            F_YS_YARN_RECEIVE_DETAILS2 = new HashSet<F_YS_YARN_RECEIVE_DETAILS2>();
        }

        public int LOTID { get; set; }
        [NotMapped]
        public string EncryptedId { get; set; }
        [Required(ErrorMessage = "Please add a {0}")]
        [Display(Name = "Lot No.")]
        public string LOTNO { get; set; }
        [Required(ErrorMessage = "Please add a {0}")]
        [Display(Name = "Brand")]
        public string BRAND { get; set; }
        [Display(Name = "Slab Code")]
        public string SLABCODE { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }

        public ICollection<F_YARN_REQ_DETAILS> F_YARN_REQ_DETAILS { get; set; }
        public ICollection<RND_FABRIC_COUNTINFO> RND_FABRIC_COUNTINFO { get; set; }
        public ICollection<RND_SAMPLE_INFO_DETAILS> RND_SAMPLE_INFO_DETAILS { get; set; }
        public ICollection<RND_SAMPLE_INFO_WEAVING_DETAILS> RND_SAMPLE_INFO_WEAVING_DETAILS { get; set; }
        public ICollection<PL_ORDERWISE_LOTINFO> PL_ORDERWISE_LOTINFO { get; set; }
        public ICollection<RND_MSTR_ROLL> RND_MSTR_ROLL { get; set; }
        public ICollection<PL_BULK_PROG_YARN_D> PL_BULK_PROG_YARN_D { get; set; }
        public ICollection<PL_PRODUCTION_PLAN_DETAILS> PL_PRODUCTION_PLAN_DETAILS { get; set; }
        public ICollection<F_PR_WEAVING_WEFT_YARN_CONSUM_DETAILS> F_PR_WEAVING_WEFT_YARN_CONSUM_DETAILS { get; set; }
        public ICollection<BAS_YARN_COUNT_LOT_INFO> BAS_YARN_COUNT_LOT_INFO { get; set; }
        public ICollection<LOOM_SETTING_CHANNEL_INFO> LOOM_SETTING_CHANNEL_INFO { get; set; }
        public ICollection<F_YS_YARN_RECEIVE_DETAILS> FYsYarnReceiveDetailses { get; set; }
        public ICollection<F_YS_INDENT_DETAILS> F_YS_INDENT_DETAILS { get; set; }
        public ICollection<F_YARN_TRANSACTION> F_YARN_TRANSACTION { get; set; }
        public ICollection<F_YS_YARN_ISSUE_DETAILS> F_YS_YARN_ISSUE_DETAILS { get; set; }
        public ICollection<COM_IMP_INVDETAILS> YARNLOTCOM_IMP_INVDETAILS { get; set; }
        public ICollection<COM_IMP_WORK_ORDER_DETAILS> COM_IMP_WORK_ORDER_DETAILS { get; set; }
        public ICollection<F_QA_FIRST_MTR_ANALYSIS_D> F_QA_FIRST_MTR_ANALYSIS_D { get; set; }
        public ICollection<F_YARN_REQ_DETAILS_S> F_YARN_REQ_DETAILS_S { get; set; }
        public ICollection<F_YARN_TRANSACTION_S> F_YARN_TRANSACTION_S { get; set; }
        public ICollection<F_YS_YARN_ISSUE_DETAILS_S> F_YS_YARN_ISSUE_DETAILS_S { get; set; }
        public ICollection<F_YS_YARN_RECEIVE_DETAILS_S> F_YS_YARN_RECEIVE_DETAILS_S { get; set; }
        public ICollection<COS_POSTCOSTING_YARNDETAILS> COS_POSTCOSTING_YARNDETAILS { get; set; }
        public ICollection<F_YS_GP_DETAILS> F_YS_GP_DETAILS { get; set; }
        public ICollection<F_PR_WEAVING_OS> F_PR_WEAVING_OS { get; set; }
        public ICollection<F_YS_YARN_RECEIVE_DETAILS2> F_YS_YARN_RECEIVE_DETAILS2 { get; set; }
    }
}
