using System.Collections.Generic;

namespace DenimERP.Models
{
    public partial class F_SIZING_MACHINE
    {
        public F_SIZING_MACHINE()
        {
            F_PR_SIZING_PROCESS_ROPE_DETAILS = new HashSet<F_PR_SIZING_PROCESS_ROPE_DETAILS>();
        }

        public int ID { get; set; }
        public string MACHINE_NO { get; set; }
        public string REMARKS { get; set; }

        public ICollection<F_PR_SIZING_PROCESS_ROPE_DETAILS> F_PR_SIZING_PROCESS_ROPE_DETAILS { get; set; }
    }
}
