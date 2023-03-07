using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DenimERP.Models
{
    public class COM_TRADE_TERMS
    {
        public COM_TRADE_TERMS()
        {
            COM_EX_LCINFO = new HashSet<COM_EX_LCINFO>();
        }
        public int TTID { get; set; }
        [NotMapped]
        public string EncryptedId { get; set; }
        [Display(Name = "Trade Terms")]
        public string TRADE_TERMS { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }
        [Display(Name = "Active/Inactive")]
        public bool ISACTIVE { get; set; }
        public virtual ICollection<COM_EX_LCINFO> COM_EX_LCINFO { get; set; }
    }
}
