using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;

namespace DenimERP.Models
{
    public partial class BAS_SUPP_CATEGORY
    {
        public BAS_SUPP_CATEGORY()
        {
            BAS_SUPPLIERINFO = new HashSet<BAS_SUPPLIERINFO>();
        }

        public int SCATID { get; set; }
        [NotMapped]
        public string EncryptedId { get; set; }
        [Remote(action: "IsSupplierCategoryInUse",controller: "BasicSupplierCategory")]
        [Display(Name = "Category Name")]
        public string CATNAME { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }

        public virtual ICollection<BAS_SUPPLIERINFO> BAS_SUPPLIERINFO { get; set; }
    }
}
