using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;

namespace DenimERP.Models
{
    public partial class BAS_BUYER_BANK_MASTER
    {
        public BAS_BUYER_BANK_MASTER()
        {
            COM_EX_LCINFO = new HashSet<COM_EX_LCINFO>();
            COM_EX_SCINFO = new HashSet<COM_EX_SCINFO>();
            COM_IMP_LCINFORMATION = new HashSet<COM_IMP_LCINFORMATION>();
            COM_EX_LCINFONTFYBANK = new HashSet<COM_EX_LCINFO>();
        }

        public int BANK_ID { get; set; }
        [NotMapped]
        public string EncryptedId { get; set; }
        [Remote(action: "IsBuyerBankNameInUse", controller: "BasicBuyerBankMaster")]
        [Display(Name = "Bank Name (Party Bank)")]
        public string PARTY_BANK { get; set; }
        public string ADDRESS { get; set; }
        public string BRANCH { get; set; }
        public string REMARKS { get; set; }

        public ICollection<COM_EX_LCINFO> COM_EX_LCINFO { get; set; }
        public ICollection<COM_EX_LCINFO> COM_EX_LCINFONTFYBANK { get; set; }
        public ICollection<COM_EX_SCINFO> COM_EX_SCINFO { get; set; }
        public ICollection<COM_IMP_LCINFORMATION> COM_IMP_LCINFORMATION { get; set; }
    }
}