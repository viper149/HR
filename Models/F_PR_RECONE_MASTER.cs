using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using DenimERP.Models.BaseModels;

namespace DenimERP.Models
{
    public partial class F_PR_RECONE_MASTER : BaseEntity
    {
        public F_PR_RECONE_MASTER()
        {
            F_PR_RECONE_YARN_CONSUMPTION = new HashSet<F_PR_RECONE_YARN_CONSUMPTION>();
            F_PR_RECONE_YARN_DETAILS = new HashSet<F_PR_RECONE_YARN_DETAILS>();
        }

        public int? TRANSID { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [Display(Name = "Trans Date")]
        public DateTime? TRANSDATE { get; set; }
        [Display(Name = "Set No.")]
        public int? SET_NO { get; set; }
        [Display(Name = "Ball No.")]
        public int? NO_OF_BALL { get; set; }
        [Display(Name = "Warp Length")]
        public double? WARP_LENGTH { get; set; }
        [Display(Name = "Converted")]
        public string CONVERTED { get; set; }
        [Display(Name = "Warp Ratio")]
        public double? WARP_RATIO { get; set; }
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }
        public string OPT3 { get; set; }
        public string OPT4 { get; set; }
        public string OPT5 { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }

        [NotMapped]
        public string EncryptedId { get; set; }

        [NotMapped]
        public string DateFormat
        {
            get
            {
                var dateTimeFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern.ToUpper();
                return !string.IsNullOrEmpty(dateTimeFormat) ? dateTimeFormat : "DD-MMM-YY";
            }
        }

        public ICollection<F_PR_RECONE_YARN_CONSUMPTION> F_PR_RECONE_YARN_CONSUMPTION { get; set; }
        public ICollection<F_PR_RECONE_YARN_DETAILS> F_PR_RECONE_YARN_DETAILS { get; set; }
    }
}
