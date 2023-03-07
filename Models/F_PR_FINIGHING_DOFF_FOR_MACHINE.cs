using System;
using System.ComponentModel.DataAnnotations;

namespace DenimERP.Models
{
    public partial class F_PR_FINIGHING_DOFF_FOR_MACHINE
    {
        public int DID { get; set; }
        public int? FPMID { get; set; }
        [Display(Name = "Finishing Process No.")]
        public int? FN_PROCESSID { get; set; }
        public string REMARKS { get; set; }
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }
        public string OPT3 { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime? CREATED_AT { get; set; }
        public string UPDATED_BY { get; set; }
        public DateTime? UPDATED_AT { get; set; }

        public F_PR_FINISHING_MACHINE_PREPARATION FPM { get; set; }
        public F_PR_FINISHING_PROCESS_MASTER DOFF { get; set; }
    }
}
