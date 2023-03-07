using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using DenimERP.Models.BaseModels;

namespace DenimERP.Models
{
    public partial class PL_PRODUCTION_PLAN_MASTER : BaseEntity
    {
        public PL_PRODUCTION_PLAN_MASTER()
        {
            PL_PRODUCTION_PLAN_DETAILS = new HashSet<PL_PRODUCTION_PLAN_DETAILS>();
            PL_PRODUCTION_SO_DETAILS = new HashSet<PL_PRODUCTION_SO_DETAILS>();
            F_DYEING_PROCESS_ROPE_MASTER = new HashSet<F_DYEING_PROCESS_ROPE_MASTER>();
        }

        public int GROUPID { get; set; }
        [Display(Name = "Group No.")]
        public int GROUP_NO { get; set; }
        [Display(Name = "Serial No.")]
        public int? SERIAL_NO { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [Display(Name = "Production Date")]
        public DateTime? PRODUCTION_DATE { get; set; }
        [NotMapped]
        public string EncryptedId { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [Display(Name = "Trans. Date")]
        public DateTime? GROUPDATE { get; set; }
        [Display(Name = "Dyeing Ref.")]
        public string DYEING_REFERANCE { get; set; }
        [Display(Name = "Dyeing Type")]
        public int DYEING_TYPE { get; set; }
        [Display(Name = "Sets")]
        public string OPTION1 { get; set; }
        public string OPTION2 { get; set; }
        public string OPTION3 { get; set; }
        public string OPTION4 { get; set; }
        public string OPTION5 { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }

        [NotMapped]
        public string DateFormat
        {
            get
            {
                var dateTimeFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern.ToUpper();
                return !string.IsNullOrEmpty(dateTimeFormat) ? dateTimeFormat : "DD-MMM-YY";
            }
        }

        public ICollection<PL_PRODUCTION_PLAN_DETAILS> PL_PRODUCTION_PLAN_DETAILS { get; set; }
        public ICollection<PL_PRODUCTION_SO_DETAILS> PL_PRODUCTION_SO_DETAILS { get; set; }
        public ICollection<F_DYEING_PROCESS_ROPE_MASTER> F_DYEING_PROCESS_ROPE_MASTER { get; set; }
        public RND_DYEING_TYPE RND_DYEING_TYPE { get; set; }
    }
}
