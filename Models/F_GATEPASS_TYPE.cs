using System;
using System.Collections.Generic;

namespace DenimERP.Models
{
    public partial class F_GATEPASS_TYPE
    {
        public F_GATEPASS_TYPE()
        {
            F_GS_GATEPASS_INFORMATION_M = new HashSet<F_GS_GATEPASS_INFORMATION_M>();
        }

        public int GPTID { get; set; }
        public string NAME { get; set; }
        public string DESCRIPTION { get; set; }
        public string REMARKS { get; set; }
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime? CREATED_AT { get; set; }
        public string UPDATED_BY { get; set; }
        public DateTime? UPDATED_AT { get; set; }

        public ICollection<F_GS_GATEPASS_INFORMATION_M> F_GS_GATEPASS_INFORMATION_M { get; set; }
    }
}
