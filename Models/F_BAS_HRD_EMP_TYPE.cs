using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DenimERP.Models.BaseModels;
using Microsoft.AspNetCore.Mvc;

namespace DenimERP.Models
{
    public partial class F_BAS_HRD_EMP_TYPE : BaseEntity
    {
        public F_BAS_HRD_EMP_TYPE()
        {
            F_HRD_EMPLOYEE = new HashSet<F_HRD_EMPLOYEE>();
        }

        public int TYPEID { get; set; }
        [Display(Name = "Employee Type")]
        [Remote(action: "AlreadyInUse", controller: "EmployeeType")]
        [Required(ErrorMessage = "{0} can not be empty.")]
        public string TYPE_NAME { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }

        [NotMapped]
        public string EncryptedId { get; set; }

        public ICollection<F_HRD_EMPLOYEE> F_HRD_EMPLOYEE { get; set; }
    }
}
