using System;
using System.Collections.Generic;

namespace DenimERP.Models
{
    public partial class F_PR_PROCESS_TYPE_INFO
    {
        public F_PR_PROCESS_TYPE_INFO()
        {
            F_PR_FINISHING_FAB_PROCESS = new HashSet<F_PR_FINISHING_FAB_PROCESS>();
        }

        public int FBPRTYPEID { get; set; }
        public string NAME { get; set; }
        public string DESCRIPTION { get; set; }
        public string REMARKS { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime? CREATED_AT { get; set; }
        public string UPDATED_BY { get; set; }
        public DateTime? UPDATED_AT { get; set; }

        public ICollection<F_PR_FINISHING_FAB_PROCESS> F_PR_FINISHING_FAB_PROCESS { get; set; }
    }
}
