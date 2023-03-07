using System.Collections.Generic;

namespace DenimERP.Models
{
    public partial class F_PR_INSPECTION_MACHINE
    {
        public F_PR_INSPECTION_MACHINE()
        {
            F_PR_INSPECTION_PROCESS_DETAILS = new HashSet<F_PR_INSPECTION_PROCESS_DETAILS>();
        }

        public int ID { get; set; }
        public string NAME { get; set; }
        public string REMARKS { get; set; }

        public ICollection<F_PR_INSPECTION_PROCESS_DETAILS> F_PR_INSPECTION_PROCESS_DETAILS { get; set; }
    }
}
