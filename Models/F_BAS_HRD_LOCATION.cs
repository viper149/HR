using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DenimERP.Models.BaseModels;
using Microsoft.AspNetCore.Mvc;

namespace DenimERP.Models
{
    public partial class F_BAS_HRD_LOCATION : BaseEntity
    {
        public F_BAS_HRD_LOCATION()
        {
            F_BAS_HRD_DEPARTMENT = new HashSet<F_BAS_HRD_DEPARTMENT>();
        }

        public int LOCID { get; set; }
        [Display(Name = "Location")]
        [Remote(action: "AlreadyInUse", controller: "Location")]
        [Required(ErrorMessage = "{0} can not be empty.")]
        public string LOC_NAME { get; set; }
        [Display(Name = "Description")]
        public string DESCRIPTION { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }

        [NotMapped]
        public string EncryptedId { get; set; }

        public ICollection<F_BAS_HRD_DEPARTMENT> F_BAS_HRD_DEPARTMENT { get; set; }
    }
}
