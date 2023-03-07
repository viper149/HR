using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DenimERP.Models
{
    public partial class CURRENCY
    {
        public CURRENCY()
        {
            COM_EX_PIMASTER = new HashSet<COM_EX_PIMASTER>();
            COM_EX_CASHINFO = new HashSet<COM_EX_CASHINFO>();
            F_BAS_HRD_NATIONALITY = new List<F_BAS_HRD_NATIONALITY>();
        }

        public int Id { get; set; }
        [Display(Name = "Currency Of")]
        public string NAME { get; set; }
        [Display(Name = "Code")]
        public string CODE { get; set; }
        [Display(Name = "Symbol")]
        public string SYMBOL { get; set; }
        [Display(Name = "Conjugated (Code + Symbol)")]
        public string CONJUGATEDCODESYMBOL { get; set; }

        public ICollection<COM_EX_PIMASTER> COM_EX_PIMASTER { get; set; }
        public ICollection<COM_EX_CASHINFO> COM_EX_CASHINFO { get; set; }
        public ICollection<F_BAS_HRD_NATIONALITY> F_BAS_HRD_NATIONALITY { get; set; }
    }
}
