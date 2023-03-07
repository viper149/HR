using System;
using System.Collections.Generic;

namespace DenimERP.Models
{
    public partial class F_PR_WEAVING_PROCESS_MASTER_S
    {
        public F_PR_WEAVING_PROCESS_MASTER_S()
        {
            F_PR_WEAVING_PROCESS_BEAM_DETAILS_S = new HashSet<F_PR_WEAVING_PROCESS_BEAM_DETAILS_S>();
        }

        public int SW_PPROCESSID { get; set; }
        public DateTime? SWP_PROCESS_DATE { get; set; }
        public int? SETID { get; set; }
        public bool? STATUS { get; set; }
        public string REMARKS { get; set; }
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }
        public string OPT3 { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime? CREATED_AT { get; set; }
        public string UPDATED_BY { get; set; }
        public DateTime? UPDATED_AT { get; set; }

        public PL_PRODUCTION_SETDISTRIBUTION SET { get; set; }
        public ICollection<F_PR_WEAVING_PROCESS_BEAM_DETAILS_S> F_PR_WEAVING_PROCESS_BEAM_DETAILS_S { get; set; }
    }
}
