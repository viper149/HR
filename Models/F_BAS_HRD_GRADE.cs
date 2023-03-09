using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HRMS.Models.BaseModels;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.Models
{
    public partial class F_BAS_HRD_GRADE : BaseEntity
    {
        public F_BAS_HRD_GRADE()
        {
            F_BAS_HRD_DESIGNATION = new HashSet<F_BAS_HRD_DESIGNATION>();
        }

        public int GRADEID { get; set; }
        [Display(Name = "Grade")]
        [Remote(action: "AlreadyInUse", controller: "Grade")]
        [Required(ErrorMessage = "{0} can not be empty.")]
        public string GRADE_NAME { get; set; }
        [Display(Name = "Description")]
        public string DESCRIPTION { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }

        [NotMapped]
        public string EncryptedId { get; set; }

        public ICollection<F_BAS_HRD_DESIGNATION> F_BAS_HRD_DESIGNATION { get; set; }
    }
}
