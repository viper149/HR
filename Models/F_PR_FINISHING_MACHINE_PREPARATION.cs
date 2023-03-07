using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DenimERP.Models
{
    public partial class F_PR_FINISHING_MACHINE_PREPARATION
    {
        public F_PR_FINISHING_MACHINE_PREPARATION()
        {
            F_PR_FINIGHING_DOFF_FOR_MACHINE = new HashSet<F_PR_FINIGHING_DOFF_FOR_MACHINE>();
            F_PR_FN_CHEMICAL_CONSUMPTION = new HashSet<F_PR_FN_CHEMICAL_CONSUMPTION>();
            //F_PR_FINISHING_PROCESS_MASTER = new HashSet<F_PR_FINISHING_PROCESS_MASTER>();
        }

        public int FPMID { get; set; }
        [NotMapped]
        public string EncryptedId { get; set; }
        public int? FABCODE { get; set; }
        public int? MACHINE_NO { get; set; }
        public int? FIN_PRO_TYPEID { get; set; }
        public string FINISH_ROUTE { get; set; }
        public string REMARKS { get; set; }
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }
        public string OPT3 { get; set; }
        public string CREATED_BY { get; set; }
        [DisplayName("Process Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? TRNSDATE { get; set; }
        public DateTime? CREATED_AT { get; set; }
        public string UPDATED_BY { get; set; }
        public DateTime? UPDATED_AT { get; set; }

        public RND_FABRICINFO FABCODENavigation { get; set; }
        public F_PR_FN_MACHINE_INFO MACHINE_NONavigation { get; set; }
        public F_PR_FN_PROCESS_TYPEINFO ProcessType { get; set; }
        public ICollection<F_PR_FINIGHING_DOFF_FOR_MACHINE> F_PR_FINIGHING_DOFF_FOR_MACHINE { get; set; }
        public ICollection<F_PR_FN_CHEMICAL_CONSUMPTION> F_PR_FN_CHEMICAL_CONSUMPTION { get; set; }
        //public ICollection<F_PR_FINISHING_PROCESS_MASTER> F_PR_FINISHING_PROCESS_MASTER { get; set; }
    }
}
