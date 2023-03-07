using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DenimERP.Models.BaseModels;

namespace DenimERP.Models
{
    public partial class H_GS_ITEM_CATEGORY : BaseEntity
    {
        public H_GS_ITEM_CATEGORY()
        {
            H_GS_ITEM_SUBCATEGORY = new HashSet<H_GS_ITEM_SUBCATEGORY>();
        }

        public int CATID { get; set; }
        [NotMapped]
        public string EncryptedId { get; internal set; }
        [Display(Name = "Item Category Name")]
        public string CATNAME { get; set; }
        [Display(Name = "Item Category Description")]
        public string DESCRIPTION { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }
        public string OPT3 { get; set; }
        
        public ICollection<H_GS_ITEM_SUBCATEGORY> H_GS_ITEM_SUBCATEGORY { get; set; }
    }
}
