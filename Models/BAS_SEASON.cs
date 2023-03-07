using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DenimERP.Models
{
    public partial class BAS_SEASON
    {
        public BAS_SEASON()
        {
            COM_EX_PIMASTER = new HashSet<COM_EX_PIMASTER>();
        }

        public int SID { get; set; }
        [Display(Name = "Season Name")]
        public string SNAME { get; set; }
        [Display(Name = "Description")]
        public string DESCRIPTION { get; set; }
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }
        public string OPT3 { get; set; }

        [NotMapped]
        public string EncryptedId { get; set; }

        public ICollection<COM_EX_PIMASTER> COM_EX_PIMASTER { get; set; }
    }
}
