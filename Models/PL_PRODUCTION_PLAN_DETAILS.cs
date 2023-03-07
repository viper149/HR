using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DenimERP.Models.BaseModels;

namespace DenimERP.Models
{
    public partial class PL_PRODUCTION_PLAN_DETAILS : BaseEntity
    {
        public PL_PRODUCTION_PLAN_DETAILS()
        {
            F_DYEING_PROCESS_ROPE_DETAILS = new HashSet<F_DYEING_PROCESS_ROPE_DETAILS>();
            PL_PRODUCTION_SETDISTRIBUTION = new HashSet<PL_PRODUCTION_SETDISTRIBUTION>();
            PlProductionSetDistributionList = new List<PL_PRODUCTION_SETDISTRIBUTION>();
            F_PR_WARPING_PROCESS_ROPE_MASTER = new HashSet<F_PR_WARPING_PROCESS_ROPE_MASTER>();
            F_LCB_PRODUCTION_ROPE_MASTER = new HashSet<F_LCB_PRODUCTION_ROPE_MASTER>();
        }

        public int SUBGROUPID { get; set; }
        [Display(Name = "Sub-Group No.")]
        public int SUBGROUPNO { get; set; }
        [Display(Name = "Date")]
        public DateTime? SUBGROUPDATE { get; set; }
        public int? GROUPID { get; set; }
        [Display(Name = "Lot No.")]
        public int? LOTID { get; set; }
        [Display(Name = "Ratio")]
        public string RATIO { get; set; }
        [Display(Name = "Reed")]
        public string REED { get; set; }
        [Display(Name = "Beam Qty.")]
        public string BEAM_NO { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }
        public string OPT3 { get; set; }

        public PL_PRODUCTION_PLAN_MASTER GROUP { get; set; }
        public BAS_YARN_LOTINFO LOT { get; set; }

        [NotMapped]
        public List<PL_PRODUCTION_SETDISTRIBUTION> PlProductionSetDistributionList { get; set; }
        public ICollection<PL_PRODUCTION_SETDISTRIBUTION> PL_PRODUCTION_SETDISTRIBUTION { get; set; }
        public ICollection<F_PR_WARPING_PROCESS_ROPE_MASTER> F_PR_WARPING_PROCESS_ROPE_MASTER { get; set; }
        public ICollection<F_DYEING_PROCESS_ROPE_DETAILS> F_DYEING_PROCESS_ROPE_DETAILS { get; set; }
        public ICollection<F_LCB_PRODUCTION_ROPE_MASTER> F_LCB_PRODUCTION_ROPE_MASTER { get; set; }
    }
}
