using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DenimERP.Models
{
    public partial class GATEPASS_TYPE
    {
        public GATEPASS_TYPE()
        {
            F_SAMPLE_DESPATCH_MASTER = new HashSet<F_SAMPLE_DESPATCH_MASTER>();
            F_SAMPLE_FABRIC_DISPATCH_MASTER = new HashSet<F_SAMPLE_FABRIC_DISPATCH_MASTER>();
        }

        public int GPTYPEID { get; set; }
        [Display(Name = "Gate Pass Type Name")]
        public string GPTYPENAME { get; set; }

        public ICollection<F_SAMPLE_DESPATCH_MASTER> F_SAMPLE_DESPATCH_MASTER { get; set; }
        public ICollection<F_SAMPLE_FABRIC_DISPATCH_MASTER> F_SAMPLE_FABRIC_DISPATCH_MASTER { get; set; }
    }
}
