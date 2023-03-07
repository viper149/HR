using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using DenimERP.Models.BaseModels;

namespace DenimERP.Models
{
    public partial class F_LCB_PRODUCTION_ROPE_MASTER : BaseEntity
    {
        public F_LCB_PRODUCTION_ROPE_MASTER()
        {
            F_LCB_PRODUCTION_ROPE_DETAILS = new HashSet<F_LCB_PRODUCTION_ROPE_DETAILS>();
        }

        public int LCBPROID { get; set; }
        [NotMapped]
        public string EncryptedId { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [Display(Name = "Trans. Date")]
        public DateTime? TRANSDATE { get; set; }
        [Display(Name = "Program/Set No.")]
        [Required]
        public int? SUBGROUPID { get; set; }
        [Display(Name = "Length/Set")]
        public double? PER_SET_LENGTH { get; set; }
        [Display(Name = "LCB Process Length")]
        [Required]
        public double? LCB_LENGTH { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }
        public string OPT5 { get; set; }
        public string OPT4 { get; set; }
        public string OPT3 { get; set; }
        public string OPT2 { get; set; }
        public string OPT1 { get; set; }

        [NotMapped]
        public string DateFormat
        {
            get
            {
                var dateTimeFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern.ToUpper();
                return !string.IsNullOrEmpty(dateTimeFormat) ? dateTimeFormat : "DD-MMM-YY";
            }
        }

        //public PL_PRODUCTION_SETDISTRIBUTION SET { get; set; }
        public PL_PRODUCTION_PLAN_DETAILS SUBGROUP { get; set; }

        public ICollection<F_LCB_PRODUCTION_ROPE_DETAILS> F_LCB_PRODUCTION_ROPE_DETAILS { get; set; }
    }
}
