using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using DenimERP.Models.BaseModels;

namespace DenimERP.Models
{
    public partial class F_DYEING_PROCESS_ROPE_MASTER : BaseEntity
    {
        public F_DYEING_PROCESS_ROPE_MASTER()
        {
            F_DYEING_PROCESS_ROPE_CHEM = new HashSet<F_DYEING_PROCESS_ROPE_CHEM>();
            F_DYEING_PROCESS_ROPE_DETAILS = new HashSet<F_DYEING_PROCESS_ROPE_DETAILS>();
        }

        public int ROPE_DID { get; set; }
        [NotMapped]
        public string EncryptedId { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [Display(Name = "Trans. Date")]
        public DateTime? TRNSDATE { get; set; }
        [Display(Name = "Group No.")]
        [Required]
        public int? GROUPID { get; set; }
        [Display(Name = "Group Length")]
        public double? GROUP_LENGTH { get; set; }
        [Display(Name = "Dyeing Length")]
        public double? DYEING_LENGTH { get; set; }
        [Display(Name = "Dyeing Code")]
        public string DYEING_CODE { get; set; }
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

        public PL_PRODUCTION_PLAN_MASTER GROUP { get; set; }
        public ICollection<F_DYEING_PROCESS_ROPE_CHEM> F_DYEING_PROCESS_ROPE_CHEM { get; set; }
        public ICollection<F_DYEING_PROCESS_ROPE_DETAILS> F_DYEING_PROCESS_ROPE_DETAILS { get; set; }
    }
}
