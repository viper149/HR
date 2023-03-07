using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DenimERP.Models.BaseModels;
using Microsoft.AspNetCore.Mvc;

namespace DenimERP.Models
{
    public partial class F_BAS_HRD_DESIGNATION : BaseEntity
    {
        public F_BAS_HRD_DESIGNATION()
        {
            F_HRD_EMPLOYEE = new HashSet<F_HRD_EMPLOYEE>();
            F_HRD_PROMOTIONNEW_DEG = new HashSet<F_HRD_PROMOTION>();
            F_HRD_PROMOTIONOLD_DEG = new HashSet<F_HRD_PROMOTION>();
        }

        public int DESID { get; set; }
        [Display(Name = "Designation")]
        [Remote(action: "AlreadyInUse", controller: "Designation")]
        [Required(ErrorMessage = "{0} can not be empty.")]
        public string DES_NAME { get; set; }
        [Display(Name = "Grade")]
        [Required(ErrorMessage = "{0} must be selected.")]
        public int? GRADEID { get; set; }
        [Display(Name = "Short Name")]
        public string SHORT_NAME { get; set; }
        [Display(Name = "উপাধি")]
        [Remote(action: "AlreadyInUseBn", controller: "Designation")]
        [Required(ErrorMessage = "{0} অবশ্যই পূরণ করতে হবে।")]
        public string DES_NAME_BN { get; set; }
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

        public F_BAS_HRD_GRADE GRADE { get; set; }

        public ICollection<F_HRD_EMPLOYEE> F_HRD_EMPLOYEE { get; set; }
        public ICollection<F_HRD_PROMOTION> F_HRD_PROMOTIONNEW_DEG { get; set; }
        public ICollection<F_HRD_PROMOTION> F_HRD_PROMOTIONOLD_DEG { get; set; }
    }
}
