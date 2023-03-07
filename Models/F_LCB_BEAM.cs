using System.Collections.Generic;

namespace DenimERP.Models
{
    public partial class F_LCB_BEAM
    {
        public F_LCB_BEAM()
        {
            F_LCB_PRODUCTION_ROPE_PROCESS_INFO = new HashSet<F_LCB_PRODUCTION_ROPE_PROCESS_INFO>();
        }

        public int ID { get; set; }
        public string BEAM_NO { get; set; }
        public string FOR_ { get; set; }
        public string REMARKS { get; set; }

        public ICollection<F_LCB_PRODUCTION_ROPE_PROCESS_INFO> F_LCB_PRODUCTION_ROPE_PROCESS_INFO { get; set; }
    }
}
