using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DenimERP.Models.BaseModels;
using Microsoft.AspNetCore.Mvc;

namespace DenimERP.Models
{
    public partial class F_WASTE_PRODUCTINFO : BaseEntity
    {
        public F_WASTE_PRODUCTINFO()
        {
            F_GS_WASTAGE_RECEIVE_D = new HashSet<F_GS_WASTAGE_RECEIVE_D>();
            F_FS_WASTAGE_ISSUE_D = new HashSet<F_FS_WASTAGE_ISSUE_D>();

        }
        public int WPID { get; set; }

        [Display(Name = "PRODUCT NAME")]
        [Remote(action: "IsProductNameInUse", controller: "FWasteProductInfo")]

        [Required(ErrorMessage = "{0} is required.")]


        public string PRODUCT_NAME { get; set; }
        [Display(Name = "Unit")]
        public int? UID { get; set; }
        [Display(Name = "Wastage Type")]
        public string WPTYPE { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }
        public string OPT3 { get; set; }

        [NotMapped]
        public string EncryptedId { get; set; }


        public F_BAS_UNITS WP { get; set; }

        public ICollection<F_GS_WASTAGE_RECEIVE_D> F_GS_WASTAGE_RECEIVE_D { get; set; }
        public ICollection<F_FS_WASTAGE_RECEIVE_D> F_FS_WASTAGE_RECEIVE_D { get; set; }
        public ICollection<F_FS_WASTAGE_ISSUE_D> F_FS_WASTAGE_ISSUE_D { get; set; }
    }
}
