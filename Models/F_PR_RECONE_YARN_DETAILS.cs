using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using DenimERP.Models.BaseModels;

namespace DenimERP.Models
{
    public partial class F_PR_RECONE_YARN_DETAILS : BaseEntity
    {
        public int? TRANSID { get; set; }
        [Display(Name = "Recone")]
        public int? RECONE_ID { get; set; }
        [Display(Name = "Ball No.")]
        public int? BALL_NO { get; set; }
        [Display(Name = "Ball Length")]
        public double? BALL_LENGTH { get; set; }
        [Display(Name = "Link Ball")]
        public int? LINK_BALL { get; set; }
        [Display(Name = "Link Ball Length")]
        public double? LINK_BALL_LENGTH { get; set; }
        [Display(Name = "Shift")]
        public int? SHIFT { get; set; }
        [Display(Name = "Count")]
        public int? COUNTID { get; set; }
        [Display(Name = "Machine")]
        public int? MACHINEID { get; set; }
        [Display(Name = "Break Ends")]
        public string BREAK_ENDS { get; set; }
        [Display(Name = "Ends Rope")]
        public string ENDS_ROPE { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }
        public string OPT3 { get; set; }
        public string OPT4 { get; set; }
        public string OPT5 { get; set; }

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

        public F_PR_RECONE_MASTER RECONE_ { get; set; }
    }
}
