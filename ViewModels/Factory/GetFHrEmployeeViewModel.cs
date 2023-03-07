using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DenimERP.ViewModels.Factory
{
    public class GetFHrEmployeeViewModel
    {
        public string EMPID { get; set; }
        public string EncryptedId { get; set; }
        [DisplayName("Name")]
        [Display(Name = "Employee Number")]
        public string EMPNO { get; set; }
        [Display(Name = "Full Name")]
        public string FULL_NAME { get; set; }
        [Display(Name = "Department")]
        public string DEPARTMENT { get; set; }
        [Display(Name = "Designation")]
        public string DESIGNATION { get; set; }
        [Display(Name = "Section")]
        public string SECTION { get; set; }
        [Display(Name = "Employee Type")]
        public string EMP_TYPE { get; set; }
    }
}
