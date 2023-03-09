using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HRMS.Models.BaseModels;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.Models
{
    public partial class F_BAS_HRD_SECTION : BaseEntity
    {
        public F_BAS_HRD_SECTION()
        {
            F_HRD_EMPLOYEE = new HashSet<F_HRD_EMPLOYEE>();
        }
        public int SECID { get; set; }
        [Display(Name = "Section")]
        [Remote(action: "AlreadyInUse", controller: "Section")]
        [Required(ErrorMessage = "{0} can not be empty.")]
        public string SEC_NAME { get; set; }
        [Display(Name = "Short Name")]
        public string SHORT_NAME { get; set; }
        [Display(Name = "সেকশন")]
        [Remote(action: "AlreadyInUseBn", controller: "Section")]
        [Required(ErrorMessage = "{0} অবশ্যই পূরণ করতে হবে।")]
        public string SEC_NAME_BN { get; set; }
        [Display(Name = "সংক্ষিপ্ত নাম")]
        public string SHORT_NAME_BN { get; set; }
        [Display(Name = "Description")]
        public string DESCRIPTION { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }
        public int? OPT3 { get; set; }

        [NotMapped]
        public string EncryptedId { get; set; }

        public ICollection<F_HRD_EMPLOYEE> F_HRD_EMPLOYEE { get; set; }
    }
}
