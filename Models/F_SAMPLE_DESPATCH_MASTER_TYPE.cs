using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DenimERP.Models
{
    public partial class F_SAMPLE_DESPATCH_MASTER_TYPE
    {
        public F_SAMPLE_DESPATCH_MASTER_TYPE()
        {
            F_SAMPLE_GARMENT_RCV_M = new HashSet<F_SAMPLE_DESPATCH_MASTER>();
        }

        public int TYPEID { get; set; }
        [Display(Name = "Gate Pass For - Type")]
        public string TYPENAME { get; set; }

        public ICollection<F_SAMPLE_DESPATCH_MASTER> F_SAMPLE_GARMENT_RCV_M { get; set; }
    }
}
