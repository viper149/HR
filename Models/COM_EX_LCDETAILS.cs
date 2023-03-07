using System.ComponentModel.DataAnnotations;

namespace DenimERP.Models
{
    public partial class COM_EX_LCDETAILS
    {
        public int TRNSID { get; set; }
        [Display(Name = "L/C No")]
        public string LCNO { get; set; }
        public int LCID { get; set; }
        [Display(Name = "PI No")]
        public int? PIID { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }
        [Display(Name = "Author")]
        public string USRID { get; set; }
        public bool? ISDELETE { get; set; }
        [Display(Name = "PI File")]
        public string PIFILE { get; set; }
        [Display(Name = "Add. Bank")]
        public int? BANKID { get; set; }
        [Display(Name = "Nego. Bank")]
        public int? BANK_ID { get; set; }

        public COM_EX_LCINFO LC { get; set; }
        public COM_EX_PIMASTER PI { get; set; }
        public BAS_BEN_BANK_MASTER BANK { get; set; }
        public BAS_BEN_BANK_MASTER BANK_ { get; set; }
    }
}
