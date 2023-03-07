using System.ComponentModel.DataAnnotations;
using DenimERP.Models;

namespace DenimERP.ViewModels.Com
{
    public class ComExPIViewModel
    {
        public string EN_TRNSID { get; set; }
        [Display(Name = "PI No")]
        public int PIID { get; set; }
        public string EncryptedId { get; set; }
        public string EN_LCNO { get; set; }
        [Display(Name = "Total")]
        public double? TOTAL { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }
        [Display(Name = "L/C Value")]
        public double? LCVALUE1 { get; set; }
        [Display(Name = "Add. Bank")]
        public int? BANKID { get; set; }
        [Display(Name = "Nego. Bank")]
        public int? BANK_ID { get; set; }
        [Display(Name = "PI File")]
        public string PIFILEUPLOADTEXT { get; set; }

        public COM_EX_PIMASTER ComExPimaster { get; set; }
        public BAS_BEN_BANK_MASTER BasBenBankMaster_Nego { get; set; }
        public BAS_BEN_BANK_MASTER BasBenBankMaster { get; set; }
    }
}
