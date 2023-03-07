using System.Collections.Generic;

namespace DenimERP.Models
{
    public partial class F_FS_ISSUE_TYPE
    {
        public F_FS_ISSUE_TYPE()
        {
            F_FS_DELIVERYCHALLAN_PACK_MASTER = new HashSet<F_FS_DELIVERYCHALLAN_PACK_MASTER>();
        }

        public int ID { get; set; }
        public string ISSUE_TYPE { get; set; }
        public string REMARKS { get; set; }
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }
        public string OPT3 { get; set; }

        public ICollection<F_FS_DELIVERYCHALLAN_PACK_MASTER> F_FS_DELIVERYCHALLAN_PACK_MASTER { get; set; }
    }
}
