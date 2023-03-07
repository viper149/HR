using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DenimERP.Models.BaseModels;

namespace DenimERP.Models
{
    public partial class F_PR_WARPING_PROCESS_DW_MASTER : BaseEntity
    {
        public F_PR_WARPING_PROCESS_DW_MASTER()
        {
            F_PR_WARPING_PROCESS_DW_DETAILS = new HashSet<F_PR_WARPING_PROCESS_DW_DETAILS>();
            F_PR_WARPING_PROCESS_DW_YARN_CONSUM_DETAILS = new HashSet<F_PR_WARPING_PROCESS_DW_YARN_CONSUM_DETAILS>();
        }

        public int WARPID { get; set; }
        [NotMapped]
        public string EncryptedId { get; set; }
        [Display(Name = "Production Date")]
        [DisplayFormat(DataFormatString = "{0:dd MMMM yyyy}")]
        [DataType(DataType.Date)]
        public DateTime? PRODDATE { get; set; }
        [Display(Name = "Time Start")]
        [DisplayFormat(DataFormatString = "{0:dd MMMM yyyy}")]
        public DateTime? TIME_START { get; set; }
        [Display(Name = "Time End")]
        [DisplayFormat(DataFormatString = "{0:dd MMMM yyyy}")]
        public DateTime? TIME_END { get; set; }
        [Display(Name = "Delivery Date")]
        [DisplayFormat(DataFormatString = "{0:dd MMMM yyyy}")]
        [DataType(DataType.Date)]
        public DateTime? DEL_DATE { get; set; }
        [Display(Name = "Program/Set No.")]
        [Required]
        public int? SETID { get; set; }
        [Display(Name = "Ball No")]
        public string BALL_NO { get; set; }
        [Display(Name = "Ratio")]
        public string WARPRATIO { get; set; }
        [Display(Name = "Length Warped")]
        public string WARPLENGTH { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }
        public string OPT3 { get; set; }
        public bool IS_DECLARE { get; set; }

        public PL_PRODUCTION_SETDISTRIBUTION SET { get; set; }
        public ICollection<F_PR_WARPING_PROCESS_DW_DETAILS> F_PR_WARPING_PROCESS_DW_DETAILS { get; set; }
        public ICollection<F_PR_WARPING_PROCESS_DW_YARN_CONSUM_DETAILS> F_PR_WARPING_PROCESS_DW_YARN_CONSUM_DETAILS { get; set; }
    }
}
