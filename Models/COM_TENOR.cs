using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DenimERP.Models
{
    public partial class COM_TENOR
    {
        public COM_TENOR()
        {
            COM_IMP_LCINFORMATION = new HashSet<COM_IMP_LCINFORMATION>();
            COM_EX_LCINFO = new HashSet<COM_EX_LCINFO>();
            COS_PRECOSTING_MASTER = new HashSet<COS_PRECOSTING_MASTER>();
            COM_EX_PIMASTER = new HashSet<COM_EX_PIMASTER>();
        }
        public int TID { get; set; }
        [NotMapped]
        public string EncryptedId { get; set; }
        [Display(Name = "Tenor Name")]
        public string NAME { get; set; }
        [Display(Name = "Cost")]
        public double COST { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }
        public int? CODE_LEVEL { get; set; }

        public ICollection<COM_IMP_LCINFORMATION> COM_IMP_LCINFORMATION { get; set; }
        public ICollection<COM_EX_LCINFO> COM_EX_LCINFO { get; set; }
        public ICollection<COS_PRECOSTING_MASTER> COS_PRECOSTING_MASTER { get; set; }
        public ICollection<COM_EX_PIMASTER> COM_EX_PIMASTER { get; set; }
    }
}
