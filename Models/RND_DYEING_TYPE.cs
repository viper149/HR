using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DenimERP.Models
{
    public partial class RND_DYEING_TYPE
    {
        public RND_DYEING_TYPE()
        {
            RND_FABRICINFO = new HashSet<RND_FABRICINFO>();
            RND_SAMPLE_INFO_DYEING = new HashSet<RND_SAMPLE_INFO_DYEING>();
            PL_PRODUCTION_PLAN_MASTER = new HashSet<PL_PRODUCTION_PLAN_MASTER>();
        }

        public int DID { get; set; }
        [Display(Name = "Dyeing Type")]
        public string DTYPE { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }

        public ICollection<RND_FABRICINFO> RND_FABRICINFO { get; set; }
        public ICollection<RND_SAMPLE_INFO_DYEING> RND_SAMPLE_INFO_DYEING { get; set; }
        public ICollection<PL_PRODUCTION_PLAN_MASTER> PL_PRODUCTION_PLAN_MASTER { get; set; }
    }
}
