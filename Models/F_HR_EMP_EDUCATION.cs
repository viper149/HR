using System.ComponentModel.DataAnnotations;

namespace DenimERP.Models
{
    public partial class F_HR_EMP_EDUCATION
    {
        public int EEID { get; set; }
        public int? EMPID { get; set; }
        [Display(Name = "Exam")]
        public string EXAM { get; set; }
        [Display(Name = "Institute")]
        public string INSTITUTE { get; set; }
        [Display(Name = "Passing Year")]
        public string PASSING_YEAR { get; set; }
        [Display(Name = "Result CGPA")]
        public string RESULT_CGPA { get; set; }
        public string OPN1 { get; set; }
        public string OPN2 { get; set; }
        public string OPN3 { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }

        public F_HRD_EMPLOYEE EMP { get; set; }
    }
}
