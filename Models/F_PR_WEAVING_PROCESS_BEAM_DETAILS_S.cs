using System;
using System.Collections.Generic;

namespace DenimERP.Models
{
    public partial class F_PR_WEAVING_PROCESS_BEAM_DETAILS_S
    {
        public F_PR_WEAVING_PROCESS_BEAM_DETAILS_S()
        {
            F_PR_WEAVING_PROCESS_DETAILS_S = new HashSet<F_PR_WEAVING_PROCESS_DETAILS_S>();
        }

        public int SWV_BEAMID { get; set; }
        public int? SW_PPROCESSID { get; set; }
        public int? BEAMID { get; set; }
        public double? BEAM_LENGTH { get; set; }
        public double? ACT_BEAM_LENGTH { get; set; }
        public DateTime? MOUNT_TIME { get; set; }
        public double? CRIMP { get; set; }
        public string REMARKS { get; set; }
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }
        public string OPT3 { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime? CREATED_AT { get; set; }
        public string UPDATED_BY { get; set; }
        public DateTime? UPDATED_AT { get; set; }

        public F_PR_SIZING_PROCESS_ROPE_DETAILS F_PR_SIZING_PROCESS_ROPE_DETAILS { get; set; }
        public F_PR_WEAVING_PROCESS_MASTER_S SW_PPROCESS { get; set; }
        public ICollection<F_PR_WEAVING_PROCESS_DETAILS_S> F_PR_WEAVING_PROCESS_DETAILS_S { get; set; }
    }
}
