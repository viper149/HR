using System.Collections.Generic;
using System.ComponentModel;
using DenimERP.Models.BaseModels;

namespace DenimERP.Models
{
    public partial class F_PR_SLASHER_DYEING_DETAILS : BaseEntity
    {
        public F_PR_SLASHER_DYEING_DETAILS()
        {

            F_PR_WEAVING_PROCESS_BEAM_DETAILS_B = new HashSet<F_PR_WEAVING_PROCESS_BEAM_DETAILS_B>();
            RND_SAMPLE_INFO_WEAVING = new HashSet<RND_SAMPLE_INFO_WEAVING>();
        }
        public int SLDID { get; set; }
        public int? SLID { get; set; }
        public int? SL_MID { get; set; }
        [DisplayName("Weaving Beam")]
        public int? W_BEAMID { get; set; }
        [DisplayName("Beam Type")]
        public string BEAM_TYPE { get; set; }
        [DisplayName("Length")]
        public double? LENGTH_PER_BEAM { get; set; }
        [DisplayName("Breakage")]
        public int? BREAK_PER_BEAM { get; set; }
        [DisplayName("Shift")]
        public string SHIFT { get; set; }
        public bool IS_WEAVING_RECEIVED { get; set; }
        [DisplayName("Is Deliverable?")]
        public bool IS_DELIVERABLE { get; set; }
        [DisplayName("Operator")]
        public int? EMPID { get; set; }
        [DisplayName("Res. Officer")]
        public int? OFFICERID { get; set; }
        [DisplayName("Remarks")]
        public string REMARKS { get; set; }
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }
        public string OPT3 { get; set; }
        public string OPT4 { get; set; }
        public string OPT5 { get; set; }

        public F_HRD_EMPLOYEE EMP { get; set; }
        public F_HRD_EMPLOYEE OFFICER { get; set; }
        public F_PR_SLASHER_DYEING_MASTER SL { get; set; }
        public F_PR_SLASHER_MACHINE_INFO SL_M { get; set; }
        public F_WEAVING_BEAM W_BEAM { get; set; }
        public ICollection<F_PR_WEAVING_PROCESS_BEAM_DETAILS_B> F_PR_WEAVING_PROCESS_BEAM_DETAILS_B { get; set; }
        public ICollection<RND_SAMPLE_INFO_WEAVING> RND_SAMPLE_INFO_WEAVING { get; set; }
    }
}
