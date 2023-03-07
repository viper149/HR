using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DenimERP.Models
{
    public partial class EXPORTSTATUS
    {
        public EXPORTSTATUS()
        {
            COM_EX_PIMASTER = new HashSet<COM_EX_PIMASTER>();
        }

        [Display(Name = "Export Type ID")]
        public int EXTYPEID { get; set; }
        [Display(Name = "Export Type")]
        public string EXPPORTTYPE { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }

        public ICollection<COM_EX_PIMASTER> COM_EX_PIMASTER { get; set; }
    }
}
