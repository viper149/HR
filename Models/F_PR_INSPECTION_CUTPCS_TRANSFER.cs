using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using DenimERP.Models.BaseModels;

namespace DenimERP.Models
{
    public partial class F_PR_INSPECTION_CUTPCS_TRANSFER : BaseEntity
    {
        public int CPID { get; set; }
        public DateTime? TRNS_DATE { get; set; }
        public int? ROLL_NO { get; set; }
        [Display(Name = "Qty (Yds)")]
        [Range(1, double.MaxValue, ErrorMessage = "The field {0} must be greater than {1}")]
        public double? QTY_YDS { get; set; }
        public double? QTY_KG { get; set; }
        public int? SHIFT { get; set; }
        public string REMARKS { get; set; }
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }
        public string OPT3 { get; set; }

        public F_HR_SHIFT_INFO SHIFTNavigation { get; set; }

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
    }
}
