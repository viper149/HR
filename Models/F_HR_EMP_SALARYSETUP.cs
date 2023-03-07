using System;
using System.ComponentModel.DataAnnotations;

namespace DenimERP.Models
{
    public partial class F_HR_EMP_SALARYSETUP
    {
        public int ESID { get; set; }
        public int? EMPID { get; set; }
        [Display(Name = "Joining Salary")]
        public double? JOINING_SALARY { get; set; }
        [Display(Name = "Present Salary")]
        public double? PRESENT_SALARY { get; set; }
        [Display(Name = "Basic")]
        public double? BASIC { get; set; }
        [Display(Name = "Medical")]
        public double? MEDICAL { get; set; }
        [Display(Name = "Others")]
        public double? OTHERS { get; set; }
        [Display(Name = "Bonus")]
        public double? BONUS { get; set; }
        [Display(Name = "AIT")]
        public double? AIT { get; set; }
        [Display(Name = "Other Allowance")]
        public double? OTHER_ALW { get; set; }
        [Display(Name = "Allowance Note")]
        public string ALW_NOTES { get; set; }
        [Display(Name = "Other Ded.")]
        public double? OTHER_DED { get; set; }
        [Display(Name = "Ded. Note")]
        public string DED_NOTES { get; set; }
        public string OPN1 { get; set; }
        public string OPN2 { get; set; }
        public double? OPN3 { get; set; }
        public double? OPN4 { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [Display(Name = "Last Increment Date")]
        public DateTime? LAST_INCREMENT_DATE { get; set; }
        [Display(Name = "Last Increment Amount")]
        public double? LAST_INCREMENT_AMOUNT { get; set; }
        [Display(Name = "Bank Account Number")]
        public string BANKACCOUNT { get; set; }

        public F_HRD_EMPLOYEE EMP { get; set; }
    }
}
