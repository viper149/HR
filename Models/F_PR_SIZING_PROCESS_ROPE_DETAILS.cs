using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DenimERP.Models
{
    public partial class F_PR_SIZING_PROCESS_ROPE_DETAILS
    {
        public F_PR_SIZING_PROCESS_ROPE_DETAILS()
        {
            F_PR_WEAVING_PROCESS_BEAM_DETAILS_B = new HashSet<F_PR_WEAVING_PROCESS_BEAM_DETAILS_B>();
            F_PR_WEAVING_PROCESS_BEAM_DETAILS_S = new HashSet<F_PR_WEAVING_PROCESS_BEAM_DETAILS_S>();
            RND_SAMPLE_INFO_WEAVING = new HashSet<RND_SAMPLE_INFO_WEAVING>();
        }
        public int SDID { get; set; }
        public int? SID { get; set; }
        [Display(Name = "Machine No.")]
        public int? S_MID { get; set; }
        [Display(Name = "Weaving Beam")]
        public int? W_BEAMID { get; set; }
        [Display(Name = "Beam Type")]
        public string BEAM_TYPE { get; set; }
        [Display(Name = "Length/Beam")]
        public double? LENGTH_PER_BEAM { get; set; }
        [Display(Name = "Break/Beam")]
        public int? BREAK_PER_BEAM { get; set; }
        //[Display(Name = "Break/Beam(B)")]
        //public int? BREAK_PER_BEAM_B { get; set; }
        //[Display(Name = "Break/Beam(C)")]
        //public int? BREAK_PER_BEAM_C { get; set; }
        [Display(Name = "Shift")]
        public string SHIFT { get; set; }
        [Display(Name = "Employee Name")]
        public int? EMPID { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }
        [Display(Name = "Is Deliverable?")]
        public bool IS_DELIVERABLE { get; set; }
        [Display(Name = "Is Received?")]
        public bool IS_WEAVING_RECEIVED { get; set; }
        public string OPT5 { get; set; }
        public string OPT4 { get; set; }
        public string OPT3 { get; set; }
        public string OPT2 { get; set; }
        public string OPT1 { get; set; }
        public DateTime? CREATED_AT { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime? UPDATED_AT { get; set; }
        public string UPDATED_BY { get; set; }

        public F_HRD_EMPLOYEE EMP { get; set; }
        public F_PR_SIZING_PROCESS_ROPE_MASTER S { get; set; }
        public F_SIZING_MACHINE S_M { get; set; }
        public F_WEAVING_BEAM W_BEAM { get; set; }
        public ICollection<F_PR_WEAVING_PROCESS_BEAM_DETAILS_B> F_PR_WEAVING_PROCESS_BEAM_DETAILS_B { get; set; }
        public ICollection<F_PR_WEAVING_PROCESS_BEAM_DETAILS_S> F_PR_WEAVING_PROCESS_BEAM_DETAILS_S { get; set; }
        public ICollection<RND_SAMPLE_INFO_WEAVING> RND_SAMPLE_INFO_WEAVING { get; set; }
    }
}
