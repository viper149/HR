using System;
using System.Collections.Generic;

namespace DenimERP.Models
{
    public partial class RND_ORDER_REPEAT
    {
        public RND_ORDER_REPEAT()
        {
            RND_FABTEST_GREY = new HashSet<RND_FABTEST_GREY>();
            RND_PRODUCTION_ORDER = new HashSet<RND_PRODUCTION_ORDER>();
        }

        public int ORPTID { get; set; }
        public string ORPTNAME { get; set; }
        public string REMARKS { get; set; }
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }
        public string OPT3 { get; set; }
        public string CREATED_BY { get; set; }
        public string UPDATED_BY { get; set; }
        public DateTime? CREATED_AT { get; set; }
        public DateTime? UPDATED_AT { get; set; }

        public ICollection<RND_FABTEST_GREY> RND_FABTEST_GREY { get; set; }
        public ICollection<RND_PRODUCTION_ORDER> RND_PRODUCTION_ORDER { get; set; }
    }
}
