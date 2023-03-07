using System;
using System.Collections.Generic;

namespace DenimERP.Models
{
    public partial class F_PR_FN_PROCESS_TYPEINFO
    {
        public F_PR_FN_PROCESS_TYPEINFO()
        {
            F_PR_FINISHING_FNPROCESS = new HashSet<F_PR_FINISHING_FNPROCESS>();
            F_PR_FINISHING_MACHINE_PREPARATION = new HashSet<F_PR_FINISHING_MACHINE_PREPARATION>();
        }

        public int FIN_PRO_TYPEID { get; set; }
        public string NAME { get; set; }
        public string DESCRIPTION { get; set; }
        public string REMARKS { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime? CREATED_AT { get; set; }
        public string UPDATED_BY { get; set; }
        public DateTime? UPDATED_AT { get; set; }

        public ICollection<F_PR_FINISHING_FNPROCESS> F_PR_FINISHING_FNPROCESS { get; set; }
        public ICollection<F_PR_FINISHING_MACHINE_PREPARATION> F_PR_FINISHING_MACHINE_PREPARATION { get; set; }
    }
}
