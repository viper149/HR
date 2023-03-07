using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DenimERP.Models.BaseModels;
using Microsoft.AspNetCore.Mvc;

namespace DenimERP.Models
{
    public partial class F_BAS_HRD_NATIONALITY : BaseEntity
    {
        public F_BAS_HRD_NATIONALITY()
        {
            F_BAS_HRD_DIVISION = new HashSet<F_BAS_HRD_DIVISION>();
            F_HRD_EMPLOYEE = new HashSet<F_HRD_EMPLOYEE>();
        }

        public int NATIONID { get; set; }
        [Display(Name = "Nationality")]
        [Remote(action: "NationalityAlreadyInUse", controller: "Nationality")]
        [Required(ErrorMessage = "{0} can not be empty.")]
        public string NATION_DESC { get; set; }
        [Display(Name = "Country")]
        [Remote(action: "CountryAlreadyInUse", controller: "Nationality")]
        [Required(ErrorMessage = "{0} can not be empty.")]
        public string COUNTRY_NAME { get; set; }
        [Display(Name = "Currency")]
        [Required(ErrorMessage = "{0} must be selected.")]
        public int? CURRENCYID { get; set; }
        [Display(Name = "Short Name")]
        public string SHORT_NAME { get; set; }
        [Display(Name = "জাতীয়তা")]
        [Remote(action: "NationalityAlreadyInUseBn", controller: "Nationality")]
        [Required(ErrorMessage = "{0} অবশ্যই পূরণ করতে হবে।")]
        public string NATION_DESC_BN { get; set; }
        [Display(Name = "দেশের নাম")]
        [Remote(action: "CountryAlreadyInUseBn", controller: "Nationality")]
        [Required(ErrorMessage = "{0} অবশ্যই পূরণ করতে হবে।")]
        public string COUNTRY_NAME_BN { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }

        [NotMapped]
        public string EncryptedId { get; set; }

        public CURRENCY CUR { get; set; }

        public ICollection<F_BAS_HRD_DIVISION> F_BAS_HRD_DIVISION { get; set; }
        public ICollection<F_HRD_EMPLOYEE> F_HRD_EMPLOYEE { get; set; }
    }
}