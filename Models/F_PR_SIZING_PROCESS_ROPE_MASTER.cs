using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DenimERP.Models
{
    public partial class F_PR_SIZING_PROCESS_ROPE_MASTER
    {
        public F_PR_SIZING_PROCESS_ROPE_MASTER()
        {
            F_PR_SIZING_PROCESS_ROPE_CHEM = new HashSet<F_PR_SIZING_PROCESS_ROPE_CHEM>();
            F_PR_SIZING_PROCESS_ROPE_DETAILS = new HashSet<F_PR_SIZING_PROCESS_ROPE_DETAILS>();
        }

        public int SID { get; set; }
        [NotMapped]
        public string EncryptedId { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [Display(Name = "Trans. Date")]
        public DateTime? TRNSDATE { get; set; }
        [Display(Name = "Program/Set No.")]
        [Required]
        public int? SETID { get; set; }
        [Display(Name = "Beam Space")]
        public double? BEAM_SPACE { get; set; }
        [Display(Name = "Pick Up")]
        public double? PICK_UP { get; set; }
        [Display(Name = "Sizing Ends")]
        public int? SIZING_ENDS { get; set; }
        [Display(Name = "Total Ends")]
        public int? TOTAL_ENDS { get; set; }
        [Display(Name = "LCB Length")]
        public double? LCB_LENGTH { get; set; }
        [Display(Name = "Act. LCB Length")]
        public double? LCB_ACT_LENGTH { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }
        public string OPT5 { get; set; }
        public string OPT4 { get; set; }
        public string OPT3 { get; set; }
        public string OPT2 { get; set; }
        public string OPT1 { get; set; }
        public DateTime? CREATED_AT { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime? UPDATED_AT { get; set; }
        public string UPDATED_BY { get; set; }

        public PL_PRODUCTION_SETDISTRIBUTION SET { get; set; }
        //public F_PR_SIZING_PROCESS_ROPE_DETAILS SizingRopeDetails { get; set; }
        public ICollection<F_PR_SIZING_PROCESS_ROPE_CHEM> F_PR_SIZING_PROCESS_ROPE_CHEM { get; set; }
        public ICollection<F_PR_SIZING_PROCESS_ROPE_DETAILS> F_PR_SIZING_PROCESS_ROPE_DETAILS { get; set; }
    }
}
