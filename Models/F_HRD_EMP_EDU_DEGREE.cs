using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HRMS.Models.BaseModels;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.Models
{
    public partial class F_HRD_EMP_EDU_DEGREE : BaseEntity
    {
        public F_HRD_EMP_EDU_DEGREE()
        {
            F_HRD_EDUCATION = new HashSet<F_HRD_EDUCATION>();
        }

        public int DEGID { get; set; }
        [Display(Name = "Degree")]
        [Remote(action: "AlreadyInUse", controller: "EduDegree")]
        [Required(ErrorMessage = "{0} can not be empty.")]
        public string DEGNAME { get; set; }
        [Display(Name = "Description")]
        public string DESCRIPTION { get; set; }
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }

        [NotMapped]
        public string EncryptedId { get; set; }

        public ICollection<F_HRD_EDUCATION> F_HRD_EDUCATION { get; set; }
    }
}
