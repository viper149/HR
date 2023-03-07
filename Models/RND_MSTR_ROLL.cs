using System.Collections.Generic;

namespace DenimERP.Models
{
    public partial class RND_MSTR_ROLL
    {
        public RND_MSTR_ROLL()
        {
            RND_PRODUCTION_ORDER = new HashSet<RND_PRODUCTION_ORDER>();
        }

        public int MID { get; set; }
        public string MASTER_ROLL { get; set; }
        public int? SUPPID { get; set; }
        public int? LOTID { get; set; }
        public string REMARKS { get; set; }

        public BAS_YARN_LOTINFO LOT { get; set; }
        public BAS_SUPPLIERINFO SUPP { get; set; }
        public ICollection<RND_PRODUCTION_ORDER> RND_PRODUCTION_ORDER { get; set; }
    }
}
