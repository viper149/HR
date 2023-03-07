using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DenimERP.Models.BaseModels;

namespace DenimERP.Models
{
    public partial class F_BAS_RECEIVE_TYPE : BaseEntity
    {
        public F_BAS_RECEIVE_TYPE()
        {
            F_YS_YARN_RECEIVE_MASTER = new HashSet<F_YS_YARN_RECEIVE_MASTER>();
            F_YARN_TRANSACTION = new HashSet<F_YARN_TRANSACTION>();
            F_CHEM_STORE_RECEIVE_MASTER = new HashSet<F_CHEM_STORE_RECEIVE_MASTER>();
            F_CHEM_TRANSECTION = new HashSet<F_CHEM_TRANSECTION>();
            F_GEN_S_RECEIVE_MASTER = new HashSet<F_GEN_S_RECEIVE_MASTER>();
            F_YARN_TRANSACTION_S = new HashSet<F_YARN_TRANSACTION_S>();
            F_YS_YARN_RECEIVE_MASTER_S = new HashSet<F_YS_YARN_RECEIVE_MASTER_S>();
        }

        public int RCVTID { get; set; }
        [NotMapped]
        public string EncryptedId { get; set; }
        [Display(Name = "Receive Type")]
        public string RCVTYPE { get; set; }
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }

        public ICollection<F_YS_YARN_RECEIVE_MASTER> F_YS_YARN_RECEIVE_MASTER { get; set; }
        public ICollection<F_YARN_TRANSACTION> F_YARN_TRANSACTION { get; set; }
        public ICollection<F_CHEM_STORE_RECEIVE_MASTER> F_CHEM_STORE_RECEIVE_MASTER { get; set; }
        public ICollection<F_CHEM_TRANSECTION> F_CHEM_TRANSECTION { get; set; }
        public ICollection<F_GEN_S_RECEIVE_MASTER> F_GEN_S_RECEIVE_MASTER { get; set; }
        public ICollection<F_YARN_TRANSACTION_S> F_YARN_TRANSACTION_S { get; set; }
        public ICollection<F_YS_YARN_RECEIVE_MASTER_S> F_YS_YARN_RECEIVE_MASTER_S { get; set; }
    }
}
