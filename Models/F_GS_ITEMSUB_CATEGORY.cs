using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DenimERP.Models.BaseModels;
using Microsoft.AspNetCore.Mvc;

namespace DenimERP.Models
{
    public partial class F_GS_ITEMSUB_CATEGORY : BaseEntity
    {
        public F_GS_ITEMSUB_CATEGORY()
        {
            F_GS_PRODUCT_INFORMATION = new HashSet<F_GS_PRODUCT_INFORMATION>();
        }

        public int SCATID { get; set; }
        [Display(Name = "Category")]
        [Required(ErrorMessage = "{0} must be selected")]
        public int? CATID { get; set; }
        [Display(Name = "Sub-Category", Prompt = "Sub-Category")]
        [Remote(action: "IsScatNameInUse", controller: "FGsItemSubCategory")]
        [Required(ErrorMessage = "{0} can't be empty")]
        public string SCATNAME { get; set; }
        [Display(Name = "Description")]
        public string DESCRIPTION { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }
        public string OPT3 { get; set; }

        [NotMapped]
        public string EncryptedId { get; set; }

        public F_GS_ITEMCATEGORY CAT { get; set; }
        public ICollection<F_GS_PRODUCT_INFORMATION> F_GS_PRODUCT_INFORMATION { get; set; }
    }
}
