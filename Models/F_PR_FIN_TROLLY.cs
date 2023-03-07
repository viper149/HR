using System;
using System.Collections.Generic;

namespace DenimERP.Models
{
    public partial class F_PR_FIN_TROLLY
    {
        public F_PR_FIN_TROLLY()
        {
            RND_FABTEST_SAMPLE = new HashSet<RND_FABTEST_SAMPLE>();
            F_PR_FINISHING_FAB_PROCESS = new HashSet<F_PR_FINISHING_FAB_PROCESS>();
            F_PR_FINISHING_FNPROCESS = new HashSet<F_PR_FINISHING_FNPROCESS>();
            F_FS_FABRIC_CLEARENCE_2ND_BEAM = new HashSet<F_FS_FABRIC_CLEARENCE_2ND_BEAM>();
        }

        public int FIN_TORLLY_ID { get; set; }
        public string NAME { get; set; }
        public string DESCRIPTION { get; set; }
        public string REMARKS { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime? CREATED_AT { get; set; }
        public string UPDATED_BY { get; set; }
        public DateTime? UPDATED_AT { get; set; }

        public ICollection<F_PR_FINISHING_FAB_PROCESS> F_PR_FINISHING_FAB_PROCESS { get; set; }
        public ICollection<F_PR_FINISHING_FNPROCESS> F_PR_FINISHING_FNPROCESS { get; set; }
        public ICollection<RND_FABTEST_SAMPLE> RND_FABTEST_SAMPLE { get; set; }
        public ICollection<F_FS_FABRIC_CLEARENCE_2ND_BEAM> F_FS_FABRIC_CLEARENCE_2ND_BEAM { get; set; }
    }
}
