using System.Collections.Generic;

namespace DenimERP.Models
{
    public partial class F_LCB_MACHINE
    {
        public F_LCB_MACHINE()
        {
            F_LCB_PRODUCTION_ROPE_PROCESS_INFO = new HashSet<F_LCB_PRODUCTION_ROPE_PROCESS_INFO>();
        }

        public int ID { get; set; }
        public string MACHINE_NO { get; set; }
        public string REMARKS { get; set; }

        public ICollection<F_LCB_PRODUCTION_ROPE_PROCESS_INFO> F_LCB_PRODUCTION_ROPE_PROCESS_INFO { get; set; }
    }
}
