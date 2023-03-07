using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DenimERP.Models.BaseModels;

namespace DenimERP.Models
{
    public partial class F_PR_WEAVING_PROCESS_MASTER_B : BaseEntity
    {
        public F_PR_WEAVING_PROCESS_MASTER_B()
        {
            F_PR_WEAVING_PROCESS_BEAM_DETAILS_B = new HashSet<F_PR_WEAVING_PROCESS_BEAM_DETAILS_B>();
            F_PR_WEAVING_WEFT_YARN_CONSUM_DETAILS = new HashSet<F_PR_WEAVING_WEFT_YARN_CONSUM_DETAILS>();
        }

        public int WV_PROCESSID { get; set; }
        [NotMapped]
        public string EncryptedId { get; set; }
        [DisplayName("Process Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? WV_PROCESS_DATE { get; set; }
        [DisplayName("Program/Set No.")]
        [Required]
        public int? SETID { get; set; }
        [DisplayName("Remarks")]
        public string REMARKS { get; set; }
        [DisplayName("Is Complete?")]
        public bool STATUS { get; set; }
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }
        public string OPT3 { get; set; }
        [NotMapped] 
        [Display(Name = "Actual Length")]
        public double? AccLength { get; set; }

        public PL_PRODUCTION_SETDISTRIBUTION SET { get; set; }
        public ICollection<F_PR_WEAVING_PROCESS_BEAM_DETAILS_B> F_PR_WEAVING_PROCESS_BEAM_DETAILS_B { get; set; }
        public ICollection<F_PR_WEAVING_WEFT_YARN_CONSUM_DETAILS> F_PR_WEAVING_WEFT_YARN_CONSUM_DETAILS { get; set; }
    }
}
