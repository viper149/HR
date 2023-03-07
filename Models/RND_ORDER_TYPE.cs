using System;
using System.Collections.Generic;

namespace DenimERP.Models
{
    public partial class RND_ORDER_TYPE
    {
        public RND_ORDER_TYPE()
        {
            RND_PRODUCTION_ORDER = new HashSet<RND_PRODUCTION_ORDER>();
        }

        public int OTYPEID { get; set; }
        public string OTYPENAME { get; set; }
        public string REMARKS { get; set; }
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }
        public string OPT3 { get; set; }
        public string CREATED_BY { get; set; }
        public string UPDATED_BY { get; set; }
        public DateTime? CREATED_AT { get; set; }
        public DateTime? UPDATED_AT { get; set; }

        public ICollection<RND_PRODUCTION_ORDER> RND_PRODUCTION_ORDER { get; set; }
    }
}
