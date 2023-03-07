using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DenimERP.Models.BaseModels;
using Microsoft.AspNetCore.Mvc;

namespace DenimERP.Models
{
    public partial class F_GS_ITEMCATEGORY : BaseEntity
    {
        public F_GS_ITEMCATEGORY()
        {
            F_GS_ITEMSUB_CATEGORY = new HashSet<F_GS_ITEMSUB_CATEGORY>();
        }

        public int CATID { get; set; }
        [Display(Name = "Category")]
        [Remote(action: "IsCatNameInUse", controller: "FGsItemCategory")]
        [Required(ErrorMessage = "{0} must be selected")]
        public string CATNAME { get; set; }
        [Display(Name = "Description")]
        public string DESCRIPTION { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }
        public string OPT3 { get; set; }

        [NotMapped]
        public string EncryptedId { get; set; }

        public ICollection<F_GS_ITEMSUB_CATEGORY> F_GS_ITEMSUB_CATEGORY { get; set; }
    }
}
