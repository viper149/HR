using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DenimERP.Models.BaseModels;
using Microsoft.AspNetCore.Mvc;

namespace DenimERP.Models
{
    public partial class F_BAS_HRD_OUT_REASON : BaseEntity
    {
        public F_BAS_HRD_OUT_REASON()
        {
            F_HRD_EMPLOYEE = new HashSet<F_HRD_EMPLOYEE>();
        }

        public int RESASON_ID { get; set; }
        [Display(Name = "Out Reason")]
        [Remote(action: "AlreadyInUse", controller: "OutReason")]
        [Required(ErrorMessage = "{0} can not be empty.")]
        public string RESASON_NAME { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }

        [NotMapped]
        public string EncryptedId { get; set; }

        public ICollection<F_HRD_EMPLOYEE> F_HRD_EMPLOYEE { get; set; }
    }
}
