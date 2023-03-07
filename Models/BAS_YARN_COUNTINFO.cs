using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DenimERP.Models.BaseModels;
using Microsoft.AspNetCore.Mvc;

namespace DenimERP.Models
{
    public partial class BAS_YARN_COUNTINFO : BaseEntity
    {
        public BAS_YARN_COUNTINFO()
        {
            RND_FABRIC_COUNTINFO = new HashSet<RND_FABRIC_COUNTINFO>();
            RND_YARNCONSUMPTION = new HashSet<RND_YARNCONSUMPTION>();
            RND_SAMPLE_INFO_DETAILS = new HashSet<RND_SAMPLE_INFO_DETAILS>();
            COS_PRECOSTING_DETAILS = new HashSet<COS_PRECOSTING_DETAILS>();
            RND_SAMPLE_INFO_WEAVING_DETAILS = new HashSet<RND_SAMPLE_INFO_WEAVING_DETAILS>();
            F_YS_INDENT_DETAILS = new HashSet<F_YS_INDENT_DETAILS>();
            F_YS_YARN_RECEIVE_DETAILS = new HashSet<F_YS_YARN_RECEIVE_DETAILS>();
            F_YARN_TRANSACTION = new HashSet<F_YARN_TRANSACTION>();
            F_PR_WARPING_PROCESS_ROPE_BALL_DETAILS = new HashSet<F_PR_WARPING_PROCESS_ROPE_BALL_DETAILS>();
            F_PR_WARPING_PROCESS_ROPE_YARN_CONSUM_DETAILS = new HashSet<F_PR_WARPING_PROCESS_ROPE_YARN_CONSUM_DETAILS>();
            BAS_YARN_COUNT_LOT_INFO = new HashSet<BAS_YARN_COUNT_LOT_INFO>();
            RndAnalysisSheetDetailses = new HashSet<RND_ANALYSIS_SHEET_DETAILS>();
            FYsYarnIssueDetailses = new HashSet<F_YS_YARN_ISSUE_DETAILS>();
            RefFYsYarnIssueDetailses = new HashSet<F_YS_YARN_ISSUE_DETAILS>();
            F_YARN_TRANSACTION_S = new HashSet<F_YARN_TRANSACTION_S>();
            F_YS_YARN_ISSUE_DETAILS_S = new HashSet<F_YS_YARN_ISSUE_DETAILS_S>();
            F_YS_YARN_ISSUE_DETAILS_S_MAIN = new HashSet<F_YS_YARN_ISSUE_DETAILS_S>();
            F_YS_YARN_RECEIVE_DETAILS_S = new HashSet<F_YS_YARN_RECEIVE_DETAILS_S>();
            F_PR_RECONE_YARN_DETAILS = new HashSet<F_PR_RECONE_YARN_DETAILS>(); 
            COS_POSTCOSTING_YARNDETAILS = new HashSet<COS_POSTCOSTING_YARNDETAILS>();
            F_YARN_REQ_DETAILS = new HashSet<F_YARN_REQ_DETAILS>();
            F_YS_GP_DETAILS = new HashSet<F_YS_GP_DETAILS>();
            F_PR_WEAVING_OS = new HashSet<F_PR_WEAVING_OS>();
            BAS_PRODUCTINFO = new HashSet<BAS_PRODUCTINFO>();
        }

        public int COUNTID { get; set; }
        [NotMapped]
        public string EncryptedId { get; set; }
        [Required(ErrorMessage = "Count Name Can Not Be Empty.")]
        [Remote(action: "IsBasYarnCountinfoInUse", controller: "BasYarnCountinfo")]
        [Display(Name = "Count Name")]
        public virtual string COUNTNAME { get; set; }
        [Display(Name = "Count Name(R&D)")]
        public string RND_COUNTNAME { get; set; }
        [Display(Name = "Yarn Category")]
        public int? YARN_CAT_ID { get; set; }
        [Display(Name = "Description")]
        public string DESCRIPTION { get; set; }
        [Display(Name = "Part No")]
        public int? PART_ID { get; set; }
        [Display(Name = "Unit")]
        public string UNIT { get; set; }
        [Display(Name = "Color")]
        public int COLOR { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }

        public BAS_YARN_CATEGORY YARN_CAT_ { get; set; }
        public BAS_COLOR BAS_COLOR { get; set; }
        public BAS_YARN_PARTNO PART_ { get; set; }

        public ICollection<RND_FABRIC_COUNTINFO> RND_FABRIC_COUNTINFO { get; set; }
        public ICollection<RND_YARNCONSUMPTION> RND_YARNCONSUMPTION { get; set; }
        public ICollection<RND_SAMPLE_INFO_DETAILS> RND_SAMPLE_INFO_DETAILS { get; set; }
        public ICollection<RND_SAMPLE_INFO_WEAVING_DETAILS> RND_SAMPLE_INFO_WEAVING_DETAILS { get; set; }
        public ICollection<COS_PRECOSTING_DETAILS> COS_PRECOSTING_DETAILS { get; set; }
        public ICollection<F_YS_INDENT_DETAILS> F_YS_INDENT_DETAILS { get; set; }
        public ICollection<F_YS_YARN_RECEIVE_DETAILS> F_YS_YARN_RECEIVE_DETAILS { get; set; }
        public ICollection<F_YARN_TRANSACTION> F_YARN_TRANSACTION { get; set; }
        public ICollection<F_PR_WARPING_PROCESS_ROPE_BALL_DETAILS> F_PR_WARPING_PROCESS_ROPE_BALL_DETAILS { get; set; }
        public ICollection<F_PR_WARPING_PROCESS_ROPE_YARN_CONSUM_DETAILS> F_PR_WARPING_PROCESS_ROPE_YARN_CONSUM_DETAILS { get; set; }
        public ICollection<BAS_YARN_COUNT_LOT_INFO> BAS_YARN_COUNT_LOT_INFO { get; set; }
        public ICollection<RND_ANALYSIS_SHEET_DETAILS> RndAnalysisSheetDetailses { get; set; }
        public ICollection<F_YS_YARN_ISSUE_DETAILS> FYsYarnIssueDetailses { get; set; }
        public ICollection<F_YS_YARN_ISSUE_DETAILS> RefFYsYarnIssueDetailses { get; set; }
        public ICollection<F_YS_YARN_ISSUE_DETAILS_S> F_YS_YARN_ISSUE_DETAILS_S_MAIN { get; set; }
        public ICollection<F_YARN_TRANSACTION_S> F_YARN_TRANSACTION_S { get; set; }
        public ICollection<F_YS_YARN_ISSUE_DETAILS_S> F_YS_YARN_ISSUE_DETAILS_S { get; set; }
        public ICollection<F_YS_YARN_RECEIVE_DETAILS_S> F_YS_YARN_RECEIVE_DETAILS_S { get; set; }
        public ICollection<F_PR_RECONE_YARN_DETAILS> F_PR_RECONE_YARN_DETAILS { get; set; }
        public ICollection<COS_POSTCOSTING_YARNDETAILS> COS_POSTCOSTING_YARNDETAILS { get; set; }
        public ICollection<F_YARN_REQ_DETAILS> F_YARN_REQ_DETAILS { get; set; }
        public ICollection<F_YS_GP_DETAILS> F_YS_GP_DETAILS { get; set; }
        public ICollection<F_PR_WEAVING_OS> F_PR_WEAVING_OS { get; set; }
        public ICollection<BAS_PRODUCTINFO> BAS_PRODUCTINFO { get; set; }
    }
}
