using System.Collections.Generic;

namespace DenimERP.Models
{
    public partial class F_PR_ROPE_MACHINE_INFO
    {
        public F_PR_ROPE_MACHINE_INFO()
        {
            F_DYEING_PROCESS_ROPE_DETAILS = new HashSet<F_DYEING_PROCESS_ROPE_DETAILS>();
        }

        public int ID { get; set; }
        public string ROPE_MC_NO { get; set; }
        public string REMARKS { get; set; }

        public ICollection<F_DYEING_PROCESS_ROPE_DETAILS> F_DYEING_PROCESS_ROPE_DETAILS { get; set; }
    }
}
