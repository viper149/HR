using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using DenimERP.Models.BaseModels;

namespace DenimERP.Models
{
    public partial class F_QA_FIRST_MTR_ANALYSIS_M : BaseEntity
    {
        public F_QA_FIRST_MTR_ANALYSIS_M()
        {
            F_QA_FIRST_MTR_ANALYSIS_D = new HashSet<F_QA_FIRST_MTR_ANALYSIS_D>();
        }
        public int FMID { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [Display(Name = "Trans. Date")]
        public DateTime? TRANS_DATE { get; set; }
        [Display(Name = "Employee Name")]
        public int? EMPID { get; set; }
        [Display(Name = "Set No.")]
        [Required(ErrorMessage = "{0} must be selected")]
        public int? SETID { get; set; }
        [Display(Name = "Beam No.")]
        [Required(ErrorMessage = "{0} must be selected")]
        public int? BEAMID { get; set; }
        [Display(Name = "Length Trial")]
        public double? LENGTH_TRIAL { get; set;}
        [Display(Name = "Actual Reed")]
        public int? ACT_REED { get; set; }
        [Display(Name = "Actual Dent")]
        public int? ACT_DENT { get; set; }
        [Display(Name = "Actual Reed Space")]
        public double? ACT_RS { get; set; }
        [Display(Name = "Actual Width")]
        public double? ACT_WIDTH { get; set; }
        [Display(Name = "Actual Ratio")]
        public string ACT_RATIO { get; set; }
        [Display(Name = "RPM")]
        public int? RPM { get; set; }
        [Display(Name = "Bypass Yarn")]
        public string BYPASS_YARN { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }
        [Display(Name = "Report No.")]
        public string RPTNO { get; set; }
        public string OPT2 { get; set; }
        public string OPT3 { get; set; }

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
        [NotMapped]
        public bool IsLocked { get; set; }

        public ICollection<F_QA_FIRST_MTR_ANALYSIS_D> F_QA_FIRST_MTR_ANALYSIS_D { get; set; }

        public F_PR_WEAVING_PROCESS_BEAM_DETAILS_B BEAM { get; set; }
        public F_HRD_EMPLOYEE EMP { get; set; }
        public PL_PRODUCTION_SETDISTRIBUTION SET { get; set; }
    }
}
