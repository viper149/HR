using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DenimERP.Models.BaseModels;
using Microsoft.AspNetCore.Mvc;

namespace DenimERP.Models
{
    public partial class BAS_BEN_BANK_MASTER : BaseEntity
    {
        public BAS_BEN_BANK_MASTER()
        {
            COM_EX_LCINFO = new HashSet<COM_EX_LCINFO>();
            COM_EX_PIMASTER = new HashSet<COM_EX_PIMASTER>();
            COM_EX_PIMASTER_ = new HashSet<COM_EX_PIMASTER>();
            COM_IMP_LCINFORMATION = new HashSet<COM_IMP_LCINFORMATION>();
            COM_EX_LCDETAILS = new HashSet<COM_EX_LCDETAILS>();
            COM_EX_LCDETAILS_NEGO = new HashSet<COM_EX_LCDETAILS>();
            ACC_LOAN_MANAGEMENT_M = new HashSet<ACC_LOAN_MANAGEMENT_M>();
        }

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

        public ICollection<COM_EX_LCINFO> COM_EX_LCINFO { get; set; }
        public ICollection<COM_EX_PIMASTER> COM_EX_PIMASTER { get; set; }
        public ICollection<COM_EX_PIMASTER> COM_EX_PIMASTER_ { get; set; }
        public ICollection<COM_IMP_LCINFORMATION> COM_IMP_LCINFORMATION { get; set; }
        public ICollection<COM_EX_LCDETAILS> COM_EX_LCDETAILS { get; set; }
        public ICollection<COM_EX_LCDETAILS> COM_EX_LCDETAILS_NEGO { get; set; }
        public ICollection<ACC_LOAN_MANAGEMENT_M> ACC_LOAN_MANAGEMENT_M { get; set; }
    }
}
