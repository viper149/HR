using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;

namespace DenimERP.Models
{
    public partial class BAS_PRODCATEGORY
    {
        public BAS_PRODCATEGORY()
        {
            BAS_PRODUCTINFO = new HashSet<BAS_PRODUCTINFO>();
            cOM_IMP_LCINFORMATIONs = new HashSet<COM_IMP_LCINFORMATION>();
        }

        public int CATID { get; set; }
        [NotMapped]
        public string EncryptedId { get; set; }
        [Display(Name = "Category Name")]
        [Required(ErrorMessage = "Must have a category. Please write down a category.")]
        [Remote(action: "IsProductCategoryInUse", controller: "Basic")]
        public string CATEGORY { get; set; }        
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }

        public virtual ICollection<BAS_PRODUCTINFO> BAS_PRODUCTINFO { get; set; }
        public virtual ICollection<COM_IMP_LCINFORMATION> cOM_IMP_LCINFORMATIONs { get; set; }
    }
}
