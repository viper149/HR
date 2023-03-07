using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DenimERP.Models
{
    public partial class F_HR_EMP_FAMILYDETAILS
    {
        public int EFID { get; set; }
        [NotMapped]
        public String EncryptedId { get; set; }
        [DisplayName("Employee Name")]
        public int? EMPID { get; set; }
        [Display(Name = "Person Name")]
        public string NAME { get; set; }
        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? DOB { get; set; }
        [Display(Name = "Profession")]
        public string PROFESSION { get; set; }
        [Display(Name = "Relation")]
        public string RELATION { get; set; }
        public string OPN1 { get; set; }
        public string OPN2 { get; set; }
        public string OPN3 { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }

        public F_HRD_EMPLOYEE EMP { get; set; }

        public static implicit operator F_HR_EMP_FAMILYDETAILS(List<F_HR_EMP_FAMILYDETAILS> v)
        {
            throw new NotImplementedException();
        }

        public static explicit operator List<object>(F_HR_EMP_FAMILYDETAILS v)
        {
            throw new NotImplementedException();
        }
    }
}
