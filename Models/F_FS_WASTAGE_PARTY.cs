using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DenimERP.Models.BaseModels;
using Microsoft.AspNetCore.Mvc;

namespace DenimERP.Models
{
    public partial class F_FS_WASTAGE_PARTY : BaseEntity
    {
        public F_FS_WASTAGE_PARTY()
        {
            F_FS_WASTAGE_ISSUE_M = new HashSet<F_FS_WASTAGE_ISSUE_M>();
        }

        public int PID { get; set; }

        [Display(Name = "PRODUCT NAME")]
        [Remote(action: "IsProductNameInUse", controller: "FFsWastageParty")]

        [Required(ErrorMessage = "{0} is required.")]
        
        public string PNAME { get; set; }
        public string ADDRESS { get; set; }
        public string PHONE { get; set; }
        public string REMARKS { get; set; }
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }
        public string OPT3 { get; set; }

        [NotMapped]
        public string EncryptedId { get; set; }

        public ICollection<F_FS_WASTAGE_ISSUE_M> F_FS_WASTAGE_ISSUE_M { get; set; }


    }
}
