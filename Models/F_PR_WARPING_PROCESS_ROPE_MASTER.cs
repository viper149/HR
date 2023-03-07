using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using DenimERP.Models.BaseModels;

namespace DenimERP.Models
{
    public partial class F_PR_WARPING_PROCESS_ROPE_MASTER: BaseEntity
    {
        public F_PR_WARPING_PROCESS_ROPE_MASTER()
        {
            F_PR_WARPING_PROCESS_ROPE_DETAILS = new HashSet<F_PR_WARPING_PROCESS_ROPE_DETAILS>();
            F_PR_WARPING_PROCESS_ROPE_YARN_CONSUM_DETAILS = new HashSet<F_PR_WARPING_PROCESS_ROPE_YARN_CONSUM_DETAILS>();
            F_PR_WARPING_PROCESS_ROPE_BALL_DETAILS = new HashSet<F_PR_WARPING_PROCESS_ROPE_BALL_DETAILS>();
        }

        public int WARPID { get; set; }
        [NotMapped]
        public string EncryptedId { get; set; }
        [Display(Name = "Start Time")]
        public DateTime? TIME_START { get; set; }
        [Display(Name = "End Time")]
        public DateTime? TIME_END { get; set; }
        [Display(Name = "Sub Group")]
        [Required]
        public int? SUBGROUPID { get; set; }
        [Display(Name = "Warp Length")]
        public double? WARP_LENGTH { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [Display(Name = "Delivery Date")]
        public DateTime? DELIVERY_DATE { get; set; }
        [Display(Name = "Declaration")]
        public bool IS_DECLARE { get; set; }
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }
        public string OPT3 { get; set; }
        public string OPT4 { get; set; }
        public string OPT5 { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }

        [NotMapped]
        public string WarpingPendingSetList { get; set; }

        [NotMapped]
        public string DateFormat
        {
            get
            {
                var dateTimeFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern.ToUpper();
                return !string.IsNullOrEmpty(dateTimeFormat) ? dateTimeFormat : "DD-MMM-YY";
            }
        }

        public PL_PRODUCTION_PLAN_DETAILS SUBGROUP { get; set; }
        public ICollection<F_PR_WARPING_PROCESS_ROPE_DETAILS> F_PR_WARPING_PROCESS_ROPE_DETAILS { get; set; }
        public ICollection<F_PR_WARPING_PROCESS_ROPE_BALL_DETAILS> F_PR_WARPING_PROCESS_ROPE_BALL_DETAILS { get; set; }
        public ICollection<F_PR_WARPING_PROCESS_ROPE_YARN_CONSUM_DETAILS> F_PR_WARPING_PROCESS_ROPE_YARN_CONSUM_DETAILS { get; set; }
    }
}
