using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DenimERP.Models
{
    public partial class F_SAMPLE_FABRIC_DISPATCH_SAMPLE_TYPE
    {
        public F_SAMPLE_FABRIC_DISPATCH_SAMPLE_TYPE()
        {
            F_SAMPLE_FABRIC_DISPATCH_DETAILS = new HashSet<F_SAMPLE_FABRIC_DISPATCH_DETAILS>();
        }

        public int STYPEID { get; set; }
        [Display(Name = "Sample Type")]
        public string STYPE { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }

        public ICollection<F_SAMPLE_FABRIC_DISPATCH_DETAILS> F_SAMPLE_FABRIC_DISPATCH_DETAILS { get; set; }
    }
}
