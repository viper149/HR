using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DenimERP.Models.BaseModels;

namespace DenimERP.Models
{
    public partial class F_PR_WEAVING_PROCESS_DETAILS_B : BaseEntity
    {
        public F_PR_WEAVING_PROCESS_DETAILS_B()
        {
            RND_FABTEST_GREY = new HashSet<RND_FABTEST_GREY>();
            F_PR_FINISHING_PROCESS_MASTER = new HashSet<F_PR_FINISHING_PROCESS_MASTER>();
            F_FS_FABRIC_CLEARENCE_2ND_BEAM = new HashSet<F_FS_FABRIC_CLEARENCE_2ND_BEAM>();
            F_PR_INSPECTION_REJECTION_B = new HashSet<F_PR_INSPECTION_REJECTION_B>();
        }
        public int TRNSID { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [DisplayName("Trns. Date")]
        public DateTime? TRNSDATE { get; set; }
        public int? WV_BEAMID { get; set; }
        [Display(Name = "Loom No")]
        public int? LOOM_NO { get; set; }
        [Display(Name = "Loom Type")]
        public int? LOOM_TYPE { get; set; }
        [Display(Name = "Doff. Length")]
        public double? LENGTH_BULK { get; set; }
        [Display(Name = "Other Doff.")]
        public int? OTHERS_DOFF { get; set; }
        [Display(Name = "Sample Fabcode")]
        public int? SAMPLE_FABCODE { get; set; }
        [Display(Name = "Doff. Time")]
        public DateTime? DOFF_TIME { get; set; }
        [Display(Name = "Shift")]
        public string SHIFT { get; set; }
        [Display(Name = "Doffer Name")]
        public int? DOFFER_NAME { get; set; }
        [Display(Name = "Is Doff. Received?")]
        public bool IS_RECEIVED_BY_FINISHING { get; set; }
        [Display(Name = "Is Deliverable?")]
        public bool IS_DELIVERABLE { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }
        public string OPT3 { get; set; }
        public string OPT4 { get; set; }
        public string OPT5 { get; set; }
        [NotMapped]
        public string OPT6 { get; set; }

        public F_HRD_EMPLOYEE DOFFER_NAMENavigation { get; set; }
        public F_LOOM_MACHINE_NO LOOM_NONavigation { get; set; }
        public LOOM_TYPE LOOM_TYPENavigation { get; set; }
        public F_PR_WEAVING_PROCESS_BEAM_DETAILS_B WV_BEAM { get; set; }
        public F_PR_WEAVING_OTHER_DOFF OTHER_DOFF { get; set; }
        public RND_SAMPLE_INFO_WEAVING WV { get; set; }

        public ICollection<RND_FABTEST_GREY> RND_FABTEST_GREY { get; set; }
        public ICollection<F_PR_FINISHING_PROCESS_MASTER> F_PR_FINISHING_PROCESS_MASTER { get; set; }
        public ICollection<F_FS_FABRIC_CLEARENCE_2ND_BEAM> F_FS_FABRIC_CLEARENCE_2ND_BEAM { get; set; }
        public ICollection<F_PR_INSPECTION_REJECTION_B> F_PR_INSPECTION_REJECTION_B { get; set; }
    }
}
