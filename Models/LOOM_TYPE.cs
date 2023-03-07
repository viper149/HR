using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DenimERP.Models
{
    public partial class LOOM_TYPE
    {
        public LOOM_TYPE()
        {
            RND_FABRICINFO = new HashSet<RND_FABRICINFO>();
            RND_SAMPLE_INFO_DYEING = new HashSet<RND_SAMPLE_INFO_DYEING>();
            RND_SAMPLE_INFO_WEAVING = new HashSet<RND_SAMPLE_INFO_WEAVING>();
            F_PR_WEAVING_PROCESS_DETAILS_B = new HashSet<F_PR_WEAVING_PROCESS_DETAILS_B>();
            F_PR_WEAVING_PROCESS_DETAILS_S = new HashSet<F_PR_WEAVING_PROCESS_DETAILS_S>();
            LOOM_SETTING_STYLE_WISE_M = new HashSet<LOOM_SETTING_STYLE_WISE_M>();
            F_PR_WEAVING_PRODUCTION = new HashSet<F_PR_WEAVING_PRODUCTION>();
            F_PR_WEAVING_WORKLOAD_EFFICIENCELOSS = new HashSet<F_PR_WEAVING_WORKLOAD_EFFICIENCELOSS>();

        }

        public int LOOMID { get; set; }
        [Display(Name = "Loom Name")]
        public string LOOM_TYPE_NAME { get; set; }
        public string REMARKS { get; set; }

        public ICollection<RND_FABRICINFO> RND_FABRICINFO { get; set; }
        public ICollection<RND_SAMPLE_INFO_DYEING> RND_SAMPLE_INFO_DYEING { get; set; }
        public ICollection<RND_SAMPLE_INFO_WEAVING> RND_SAMPLE_INFO_WEAVING { get; set; }
        public ICollection<F_PR_WEAVING_PROCESS_DETAILS_B> F_PR_WEAVING_PROCESS_DETAILS_B { get; set; }
        public ICollection<F_PR_WEAVING_PROCESS_DETAILS_S> F_PR_WEAVING_PROCESS_DETAILS_S { get; set; }
        public ICollection<LOOM_SETTING_STYLE_WISE_M> LOOM_SETTING_STYLE_WISE_M { get; set; }
        public ICollection<F_PR_WEAVING_PRODUCTION> F_PR_WEAVING_PRODUCTION { get; set; }
        public ICollection<F_PR_WEAVING_WORKLOAD_EFFICIENCELOSS> F_PR_WEAVING_WORKLOAD_EFFICIENCELOSS { get; set; }
    }
}
