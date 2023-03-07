using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using DenimERP.Models.BaseModels;

namespace DenimERP.Models
{
    public partial class H_GS_ITEM_SUBCATEGORY : BaseEntity
    {
        public H_GS_ITEM_SUBCATEGORY()
        {
            H_GS_PRODUCT = new HashSet<H_GS_PRODUCT>();
        }

        public int SCATID { get; set; }
        [NotMapped]
        public string EncryptedId { get; set; }
        public int? CATID { get; set; }
        public string SCATNAME { get; set; }
        public string DESCRIPTION { get; set; }
        public string REMARKS { get; set; }
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }
        public string OPT3 { get; set; }

        public H_GS_ITEM_CATEGORY CAT { get; set; }
        public ICollection<H_GS_PRODUCT> H_GS_PRODUCT { get; set; }
    }
}
