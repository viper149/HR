using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DenimERP.Models.BaseModels;
using Microsoft.AspNetCore.Mvc;

namespace DenimERP.Models
{
    public partial class F_BAS_HRD_SHIFT : BaseEntity
    {
        public F_BAS_HRD_SHIFT()
        {
            F_HRD_EMPLOYEE = new HashSet<F_HRD_EMPLOYEE>();
        }

        public int SHIFTID { get; set; }
        [Display(Name = "Shift")]
        [Remote(action: "AlreadyInUse", controller: "Shift")]
        [Required(ErrorMessage = "{0} can not be empty.")]
        public string SHIFT_NAME { get; set; }
        [Display(Name = "Short Name")]
        public string SHORT_NAME { get; set; }
        [Display(Name = "Starting Time")]
        [Required(ErrorMessage = "{0} can not be empty.")]
        public TimeSpan? TIME_START { get; set; }
        [Display(Name = "End Time")]
        [Required(ErrorMessage = "{0} can not be empty.")]
        public TimeSpan? TIME_END { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }
        public string OPT3 { get; set; }


        [NotMapped]
        public string TimeStart { get; set; }
        [NotMapped]
        public string TimeEnd { get; set; }

        [NotMapped]
        public string EncryptedId { get; set; }

        public ICollection<F_HRD_EMPLOYEE> F_HRD_EMPLOYEE { get; set; }
    }
}
