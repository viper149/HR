using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;

namespace DenimERP.Models
{
    public partial class F_BAS_DESIGNATION
    {
        public F_BAS_DESIGNATION()
        {
            F_HR_EMP_OFFICIALINFO = new HashSet<F_HR_EMP_OFFICIALINFO>();
        }

        public int DESID { get; set; }
        [NotMapped]
        public string EncryptedId { get; set; }
        [Display(Name ="Designation Name"), Required(ErrorMessage = "Designation can not be empty."), Remote(controller: "FBasDesignation", action: "IsDesignationNameInUse")]
        public string DESNAME { get; set; }
        [Display(Name ="Option 1")]
        public string OPN1 { get; set; }
        [Display(Name ="Option 2")]
        public string OPN2 { get; set; }
        [Display(Name ="Remarks")]
        public string REMARKS { get; set; }

        public ICollection<F_HR_EMP_OFFICIALINFO> F_HR_EMP_OFFICIALINFO { get; set; }
    }
}
