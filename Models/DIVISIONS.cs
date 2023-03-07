using System.Collections.Generic;

namespace DenimERP.Models
{
    public partial class DIVISIONS
    {
        public DIVISIONS()
        {
            DISTRICTS = new HashSet<DISTRICTS>();
        }

        public int ID { get; set; }
        public string NAME { get; set; }
        public string BN_NAME { get; set; }

        public ICollection<DISTRICTS> DISTRICTS { get; set; }
    }
}
