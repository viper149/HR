using System.Collections.Generic;

namespace DenimERP.Models
{
    public partial class F_FS_CLEARANCE_ROLL_TYPE
    {
        public F_FS_CLEARANCE_ROLL_TYPE()
        {
            F_FS_CLEARANCE_MASTER_SAMPLE_ROLL = new HashSet<F_FS_CLEARANCE_MASTER_SAMPLE_ROLL>();
        }

        public int RTID { get; set; }
        public string RTNAME { get; set; }
        public string REMARKS { get; set; }
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }

        public ICollection<F_FS_CLEARANCE_MASTER_SAMPLE_ROLL> F_FS_CLEARANCE_MASTER_SAMPLE_ROLL { get; set; }
    }
}
