using System.Collections.Generic;

namespace DenimERP.Models
{
    public partial class F_WEAVING_BEAM
    {
        public F_WEAVING_BEAM()
        {
            F_PR_SIZING_PROCESS_ROPE_DETAILS = new HashSet<F_PR_SIZING_PROCESS_ROPE_DETAILS>();
            F_PR_SLASHER_DYEING_DETAILS = new HashSet<F_PR_SLASHER_DYEING_DETAILS>();
        }

        public int ID { get; set; }
        public string BEAM_NO { get; set; }
        public string REMARKS { get; set; }

        public ICollection<F_PR_SIZING_PROCESS_ROPE_DETAILS> F_PR_SIZING_PROCESS_ROPE_DETAILS { get; set; }
        public ICollection<F_PR_SLASHER_DYEING_DETAILS> F_PR_SLASHER_DYEING_DETAILS { get; set; }
    }
}
