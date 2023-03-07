using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DenimERP.Models.BaseModels;

namespace DenimERP.Models
{
    public partial class PL_PRODUCTION_SETDISTRIBUTION : BaseEntity
    {
        public PL_PRODUCTION_SETDISTRIBUTION()
        {
            F_PR_WARPING_PROCESS_ROPE_DETAILS = new HashSet<F_PR_WARPING_PROCESS_ROPE_DETAILS>();
            F_PR_SIZING_PROCESS_ROPE_MASTER = new HashSet<F_PR_SIZING_PROCESS_ROPE_MASTER>();
            F_PR_WEAVING_BEAM_RECEIVING = new HashSet<F_PR_WEAVING_BEAM_RECEIVING>();
            F_PR_WEAVING_PROCESS_MASTER_S = new HashSet<F_PR_WEAVING_PROCESS_MASTER_S>();
            F_PR_WEAVING_PROCESS_MASTER_B = new HashSet<F_PR_WEAVING_PROCESS_MASTER_B>();
            F_PR_INSPECTION_PROCESS_MASTER = new HashSet<F_PR_INSPECTION_PROCESS_MASTER>();
            F_PR_FINISHING_BEAM_RECEIVE = new HashSet<F_PR_FINISHING_BEAM_RECEIVE>();
            F_PR_WARPING_PROCESS_DW_MASTER = new HashSet<F_PR_WARPING_PROCESS_DW_MASTER>();
            F_PR_SLASHER_DYEING_MASTER = new HashSet<F_PR_SLASHER_DYEING_MASTER>();
            F_YARN_REQ_DETAILS = new HashSet<F_YARN_REQ_DETAILS>();
            RND_FABTEST_BULK = new HashSet<RND_FABTEST_BULK>();
            RND_SAMPLE_INFO_WEAVING = new HashSet<RND_SAMPLE_INFO_WEAVING>();
            F_FS_FABRIC_CLEARENCE_2ND_BEAM = new HashSet<F_FS_FABRIC_CLEARENCE_2ND_BEAM>();
            F_PR_WARPING_PROCESS_ECRU_MASTER = new HashSet<F_PR_WARPING_PROCESS_ECRU_MASTER>();
            F_PR_WARPING_PROCESS_SW_MASTER = new HashSet<F_PR_WARPING_PROCESS_SW_MASTER>();
            F_FS_CLEARANCE_MASTER_SAMPLE_ROLL = new HashSet<F_FS_CLEARANCE_MASTER_SAMPLE_ROLL>();
            F_QA_FIRST_MTR_ANALYSIS_M = new HashSet<F_QA_FIRST_MTR_ANALYSIS_M>();
            F_YARN_REQ_DETAILS_S = new HashSet<F_YARN_REQ_DETAILS_S>();
            RND_FABTEST_SAMPLE = new HashSet<RND_FABTEST_SAMPLE>();
            RND_FABTEST_GREY = new HashSet<RND_FABTEST_GREY>();
            F_PR_FINISHING_PROCESS_MASTER = new HashSet<F_PR_FINISHING_PROCESS_MASTER>();
            F_SAMPLE_FABRIC_RCV_D = new HashSet<F_SAMPLE_FABRIC_RCV_D>();
            RND_BOM = new HashSet<RND_BOM>();
            F_PR_WEAVING_OS = new HashSet<F_PR_WEAVING_OS>();
        }
        public int SETID { get; set; }
        [Display(Name = "Trans. date")]
        public DateTime? TRNSDATE { get; set; }
        public int? SUBGROUPID { get; set; }
        [Display(Name = "Program/Set No.")]
        public int? PROG_ID { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }
        public string OPT3 { get; set; }

        public PL_BULK_PROG_SETUP_D PROG_ { get; set; }
        public PL_PRODUCTION_PLAN_DETAILS SUBGROUP { get; set; }

        public ICollection<F_PR_WARPING_PROCESS_ROPE_DETAILS> F_PR_WARPING_PROCESS_ROPE_DETAILS { get; set; }
        public ICollection<F_PR_SIZING_PROCESS_ROPE_MASTER> F_PR_SIZING_PROCESS_ROPE_MASTER { get; set; }
        public ICollection<F_PR_WEAVING_BEAM_RECEIVING> F_PR_WEAVING_BEAM_RECEIVING { get; set; }
        public ICollection<F_PR_WEAVING_PROCESS_MASTER_S> F_PR_WEAVING_PROCESS_MASTER_S { get; set; }
        public ICollection<F_PR_WEAVING_PROCESS_MASTER_B> F_PR_WEAVING_PROCESS_MASTER_B { get; set; }
        public ICollection<F_PR_INSPECTION_PROCESS_MASTER> F_PR_INSPECTION_PROCESS_MASTER { get; set; }
        public ICollection<F_PR_FINISHING_BEAM_RECEIVE> F_PR_FINISHING_BEAM_RECEIVE { get; set; }
        public ICollection<F_PR_WARPING_PROCESS_DW_MASTER> F_PR_WARPING_PROCESS_DW_MASTER { get; set; }
        public ICollection<F_PR_SLASHER_DYEING_MASTER> F_PR_SLASHER_DYEING_MASTER { get; set; }
        public ICollection<F_YARN_REQ_DETAILS> F_YARN_REQ_DETAILS { get; set; }
        public ICollection<RND_FABTEST_BULK> RND_FABTEST_BULK { get; set; }
        public ICollection<RND_SAMPLE_INFO_WEAVING> RND_SAMPLE_INFO_WEAVING { get; set; }
        public ICollection<F_FS_FABRIC_CLEARENCE_2ND_BEAM> F_FS_FABRIC_CLEARENCE_2ND_BEAM { get; set; }
        public ICollection<F_PR_WARPING_PROCESS_ECRU_MASTER> F_PR_WARPING_PROCESS_ECRU_MASTER { get; set; }
        public ICollection<F_PR_WARPING_PROCESS_SW_MASTER> F_PR_WARPING_PROCESS_SW_MASTER { get; set; }
        public ICollection<F_FS_CLEARANCE_MASTER_SAMPLE_ROLL> F_FS_CLEARANCE_MASTER_SAMPLE_ROLL { get; set; }
        public ICollection<F_QA_FIRST_MTR_ANALYSIS_M> F_QA_FIRST_MTR_ANALYSIS_M { get; set; }
        public ICollection<F_YARN_REQ_DETAILS_S> F_YARN_REQ_DETAILS_S { get; set; }
        public ICollection<RND_FABTEST_SAMPLE> RND_FABTEST_SAMPLE { get; set; }
        public ICollection<RND_FABTEST_GREY> RND_FABTEST_GREY { get; set; }
        public ICollection<F_PR_FINISHING_PROCESS_MASTER> F_PR_FINISHING_PROCESS_MASTER { get; set; }
        public ICollection<F_SAMPLE_FABRIC_RCV_D> F_SAMPLE_FABRIC_RCV_D { get; set; }
        public ICollection<RND_BOM> RND_BOM { get; set; }
        public ICollection<F_PR_WEAVING_OS> F_PR_WEAVING_OS { get; set; }
    }
}
