using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DenimERP.Models.BaseModels;
using Microsoft.AspNetCore.Mvc;

namespace DenimERP.Models
{
    public partial class COMPANY_INFO : BaseEntity
    {
        public COMPANY_INFO()
        {
            F_HRD_EMPLOYEE = new HashSet<F_HRD_EMPLOYEE>();
        }

        public int ID { get; set; }
        [Display(Name = "Company Name")]
        [Remote(action: "AlreadyInUse", controller: "Company")]
        [Required(ErrorMessage = "{0} can not be empty.")]
        public string COMPANY_NAME { get; set; }
        [Display(Name = "Description")]
        public string DESCRIPTION { get; set; }
        [Display(Name = "Tag line")]
        public string TAGLINE { get; set; }
        [Display(Name = "Logo")]
        public byte[] LOGO { get; set; }
        [NotMapped]
        public string LogoBase64 { get; set; }
        [Display(Name = "Factory Address")]
        public string FACTORY_ADDRESS { get; set; }
        [Display(Name = "Head Office Address")]
        public string HEADOFFICE_ADDRESS { get; set; }
        [Display(Name = "Web Address")]
        public string WEB_ADDRESS { get; set; }
        [EmailAddress]
        [Display(Name = "Email")]
        [Required(ErrorMessage = "{0} can not be empty.")]
        public string EMAIL { get; set; }
        [Display(Name = "Phone1")]
        [Required(ErrorMessage = "{0} can not be empty.")]
        public string PHONE1 { get; set; }
        [Display(Name = "Phone2")]
        public string PHONE2 { get; set; }
        [Display(Name = "Phone3")]
        public string PHONE3 { get; set; }
        [Display(Name = "BIN No.")]
        [Required(ErrorMessage = "{0} can not be empty.")]
        public string BIN_NO { get; set; }
        [Display(Name = "EIIN No.")]
        [Required(ErrorMessage = "{0} can not be empty.")]
        public string ETIN_NO { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }

        [NotMapped]
        public string EncryptedId { get; set; }

        public ICollection<F_HRD_EMPLOYEE> F_HRD_EMPLOYEE { get; set; }
    }
}
