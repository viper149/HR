using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DenimERP.Models.BaseModels;

namespace DenimERP.Models
{
    public partial class F_BAS_ISSUE_TYPE : BaseEntity
    {
        public F_BAS_ISSUE_TYPE()
        {
            F_YARN_TRANSACTION = new HashSet<F_YARN_TRANSACTION>();
            F_YS_YARN_ISSUE_MASTER = new HashSet<F_YS_YARN_ISSUE_MASTER>();
            F_CHEM_TRANSECTION = new HashSet<F_CHEM_TRANSECTION>();
            F_CHEM_ISSUE_MASTER = new HashSet<F_CHEM_ISSUE_MASTER>();
            F_GEN_S_ISSUE_MASTER = new HashSet<F_GEN_S_ISSUE_MASTER>();
            F_YARN_TRANSACTION_S = new HashSet<F_YARN_TRANSACTION_S>();
            F_YS_YARN_ISSUE_MASTER_S = new HashSet<F_YS_YARN_ISSUE_MASTER_S>();
        }

        public int ISSUID { get; set; }
        [Display(Name = "Issue Type")]
        public string ISSUTYPE { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }

        public ICollection<F_YARN_TRANSACTION> F_YARN_TRANSACTION { get; set; }
        public ICollection<F_YS_YARN_ISSUE_MASTER> F_YS_YARN_ISSUE_MASTER { get; set; }
        public ICollection<F_CHEM_TRANSECTION> F_CHEM_TRANSECTION { get; set; }
        public ICollection<F_CHEM_ISSUE_MASTER> F_CHEM_ISSUE_MASTER { get; set; }
        public ICollection<F_GEN_S_ISSUE_MASTER> F_GEN_S_ISSUE_MASTER { get; set; }
        public ICollection<F_YARN_TRANSACTION_S> F_YARN_TRANSACTION_S { get; set; }
        public ICollection<F_YS_YARN_ISSUE_MASTER_S> F_YS_YARN_ISSUE_MASTER_S { get; set; }
    }
}
