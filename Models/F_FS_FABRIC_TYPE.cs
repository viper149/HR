using System;
using System.Collections.Generic;

namespace DenimERP.Models
{
    public partial class F_FS_FABRIC_TYPE
    {
        public F_FS_FABRIC_TYPE()
        {
            F_FS_FABRIC_CLEARENCE_2ND_BEAM = new HashSet<F_FS_FABRIC_CLEARENCE_2ND_BEAM>();
        }

        public int TTID { get; set; }
        public string NAME { get; set; }
        public string REMARKS { get; set; }
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }
        public string OPT3 { get; set; }
        public string OPT4 { get; set; }
        public string OPT5 { get; set; }
        public string OPT6 { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime? CREATED_AT { get; set; }
        public string UPDATED_BY { get; set; }
        public DateTime? UPDATED_AT { get; set; }

        public ICollection<F_FS_FABRIC_CLEARENCE_2ND_BEAM> F_FS_FABRIC_CLEARENCE_2ND_BEAM { get; set; }
    }
}
