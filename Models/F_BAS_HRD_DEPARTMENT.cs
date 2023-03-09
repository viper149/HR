using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HRMS.Models.BaseModels;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.Models
{
    public partial class F_BAS_HRD_DEPARTMENT : BaseEntity
    {
        public F_BAS_HRD_DEPARTMENT()
        {
            F_HRD_EMPLOYEE = new HashSet<F_HRD_EMPLOYEE>();
        }

        public int DEPTID { get; set; }
        [Display(Name = "Department")]
        [Remote(action: "AlreadyInUse", controller: "Department")]
        [Required(ErrorMessage = "{0} can not be empty.")]
        public string DEPTNAME { get; set; }
        [Display(Name = "ডিপার্টমেন্ট")]
        [Remote(action: "AlreadyInUseBn", controller: "Department")]
        [Required(ErrorMessage = "{0} অবশ্যই পূরণ করতে হবে।")]
        public string DEPTNAME_BN { get; set; }
        [Display(Name = "Location")]
        public int? LOCATIONID { get; set; }
        [Display(Name = "Description")]
        public string DESCRIPTION { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }
        public int? OPT3 { get; set; }

        [NotMapped]
        public string EncryptedId { get; set; }

        public F_BAS_HRD_LOCATION LOCATION { get; set; }

        public ICollection<F_HRD_EMPLOYEE> F_HRD_EMPLOYEE { get; set; }
    }
}
