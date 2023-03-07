using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DenimERP.Models.BaseModels;

namespace DenimERP.Models
{
    public partial class F_PR_FINISHING_PROCESS_MASTER : BaseEntity
    {
        public F_PR_FINISHING_PROCESS_MASTER()
        {
            //F_PR_FINISHING_FAB_PROCESS = new HashSet<F_PR_FINISHING_FAB_PROCESS>();
            F_PR_FINISHING_FNPROCESS = new HashSet<F_PR_FINISHING_FNPROCESS>();
            F_PR_FINIGHING_DOFF_FOR_MACHINE = new HashSet<F_PR_FINIGHING_DOFF_FOR_MACHINE>();
        }

        public int FN_PROCESSID { get; set; }
        [NotMapped]
        public string EncryptedId { get; set; }
        [DisplayName("Process Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? FN_PROCESSDATE { get; set; }
        //[DisplayName("Style Name")]
        //public int? FPMID { get; set; }
        [DisplayName("Style Name")]
        [Required]
        public int? FABCODE { get; set; }
        [DisplayName("Doff.")]
        [Required]
        public int? DOFF_ID { get; set; }
        [DisplayName("Process Length")]
        public double? LENGTH_BEAM { get; set; }
        [DisplayName("Doff. Length")]
        public double? LENGTH_DOFF { get; set; }
        [DisplayName("Act. Length")]
        public double? LENGTH_ACT { get; set; }
        [DisplayName("Is Complete?")]
        public bool STATUS { get; set; }
        [DisplayName("Remarks")]
        public string REMARKS { get; set; }
        public int? SETID { get; set; }
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }
        public string OPT3 { get; set; }

        //public F_PR_FINISHING_MACHINE_PREPARATION MACHINE_PREPARATION { get; set; }
        public RND_FABRICINFO FABRICINFO { get; set; }
        public PL_PRODUCTION_SETDISTRIBUTION SET { get; set; }
        public F_PR_WEAVING_PROCESS_DETAILS_B DOFF { get; set; }
        //public ICollection<F_PR_FINISHING_FAB_PROCESS> F_PR_FINISHING_FAB_PROCESS { get; set; }
        public ICollection<F_PR_FINISHING_FNPROCESS> F_PR_FINISHING_FNPROCESS { get; set; }
        public ICollection<F_PR_FINIGHING_DOFF_FOR_MACHINE> F_PR_FINIGHING_DOFF_FOR_MACHINE { get; set; }
    }
}
