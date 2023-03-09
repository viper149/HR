using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HRMS.Models.BaseModels;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.Models
{
    public partial class BAS_BEN_BANK_MASTER : BaseEntity
    {
        public int BANKID { get; set; }
        [NotMapped]
        public string EncryptedId { get; set; }
        [Remote(action: "AlreadyInUse", controller: "BeneficiaryBank")]
        [Display(Name = "Bank Name (Beneficiary)")]
        public string BEN_BANK { get; set; }
        [Display(Name = "Address")]
        public string ADDRESS { get; set; }
        [Display(Name = "Branch")]
        public string BRANCH { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }
    }
}
