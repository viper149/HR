using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using DenimERP.Models.BaseModels;

namespace DenimERP.Models
{
    public partial class F_PR_WEAVING_WORKLOAD_EFFICIENCELOSS : BaseEntity
    {
       
        public int WWEID { get; set; }
        //[Display(Name = "Shift")]
        public int? SHIFTID { get; set; }
        [Display(Name = "Shift Incharge")]
        public int? SIEMPID { get; set; }
        [Display(Name = "Loom Type")]
        public int? LOOMID { get; set; }
        [Display(Name = "Date")]
        public DateTime? SHIFT_DATE { get; set; }
        [Display(Name = "No. of Knotting")]
        public double? KNOTTING { get; set; }
        [Display(Name = "No. of New Run")]
        public double? NEWRUN { get; set; }
        [Display(Name = "No. of Pending")]
        public double? PENDING { get; set; }
        [Display(Name = "No. of Re-Knotting")]
        public double? RE_KNOTTING { get; set; }
        [Display(Name = "No. of CAM Change")]
        public double? CAM_CHANGE { get; set; }
        [Display(Name = "No. of Article Change")]
        public double? ARTICLE_CHANGE { get; set; }
        [Display(Name = "No. of Reed Change")]
        public double? REED_CHANGE { get; set; }
        [Display(Name = "No. of Pattern Change")]
        public double? PATTERN_CHANGE { get; set; }
        [Display(Name = "No. of Extra Warp Insertion ")]
        public double? EXTRA_WARP_INSERTION { get; set; }
        [Display(Name = "No. of Extra Warp Out")]
        public double? EXTRA_WARP_OUT { get; set; }
        [Display(Name = "Beam Short")]
        public double? BEAM_SHORT { get; set; }
        [Display(Name = "Yarn Short")]
        public double? YARN_SHORT { get; set; }
        [Display(Name = "Mechanical Work")]
        public double? MECHANICAL_WORK { get; set; }
        [Display(Name = "Electrical Work")]
        public double? ELECTRICAL_WORK { get; set; }
        [Display(Name = "Compressor Work")]
        public double? COMPRESSOR_WORK { get; set; }
        [Display(Name = "QA Hold")]
        public double? QA_HOLD { get; set; }
        [Display(Name = "QA Stop")]
        public double? QA_STOP { get; set; }
        [Display(Name = "RND Stop")]
        public double? RND_STOP { get; set; }
        [Display(Name = "Other Stop")]
        public double? OTHER_STOP { get; set; }
        [Display(Name = "Knotting Gaiting")]
        public double? KNOTTING_GAITING { get; set; }
        [Display(Name = "Breakages")]
        public double? BREAKAGES { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }
        [Display(Name = "Shift")]
        public string OP1 { get; set; }
        public string OP2 { get; set; }
        public string OP3 { get; set; }
        public string OP4 { get; set; }
        public string OP5 { get; set; }

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

        public LOOM_TYPE LOOM { get; set; }
        public F_HR_SHIFT_INFO SHIFT { get; set; }
        public F_HRD_EMPLOYEE SIEMP { get; set; }
    }
}
