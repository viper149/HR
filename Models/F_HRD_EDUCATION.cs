using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using HRMS.Models.BaseModels;

namespace HRMS.Models
{
    public partial class F_HRD_EDUCATION : BaseEntity
    {
        public int EDUID { get; set; }
        [Display(Name = "Name of Degree")]
        public int? DEGID { get; set; }
        public int? EMPID { get; set; }
        [Display(Name = "Group/Subject")]
        public string MAJOR { get; set; }
        [Display(Name = "Board/ University")]
        public string BOARD_UNI { get; set; }
        [Display(Name = "Passing Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? PASS_DATE { get; set; }
        [Display(Name = "Result/ Grade")]
        public string CGPA { get; set; }
        [Display(Name = "Out of")]
        public string OUTOF { get; set; }
        [Display(Name = "Remarks")]
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }
        public string OPT3 { get; set; }

        [NotMapped]
        public string EncryptedId { get; set; }
        [NotMapped]
        [Display(Name = "Graduated?")]
        public bool IS_GRADUATE { get; set; }
        [NotMapped]
        public string DateFormat
        {
            get
            {
                var dateTimeFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern.ToUpper();
                return !string.IsNullOrEmpty(dateTimeFormat) ? dateTimeFormat : "dd-MMM-yyyy";
            }
        }

        public F_HRD_EMP_EDU_DEGREE DEG { get; set; }
        public F_HRD_EMPLOYEE EMP { get; set; }
    }
}
