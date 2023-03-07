using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DenimERP.Models
{
    public partial class F_LOOM_MACHINE_NO
    {
        public F_LOOM_MACHINE_NO()
        {
            F_PR_WEAVING_PROCESS_DETAILS_B = new HashSet<F_PR_WEAVING_PROCESS_DETAILS_B>();
            F_PR_WEAVING_PROCESS_DETAILS_S = new HashSet<F_PR_WEAVING_PROCESS_DETAILS_S>();
            RND_FABTEST_SAMPLE = new HashSet<RND_FABTEST_SAMPLE>();
            RND_FABTEST_SAMPLE_BULK = new HashSet<RND_FABTEST_SAMPLE_BULK>();
            F_PR_WEAVING_PROCESS_BEAM_DETAILS_B = new HashSet<F_PR_WEAVING_PROCESS_BEAM_DETAILS_B>();
        }

        public int ID { get; set; }
        [Display(Name = "Loom No.")]
        public string LOOM_NO { get; set; }
        [Display(Name = "Loom Type")]
        public int? LOOM_TYPE { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }

        public ICollection<F_PR_WEAVING_PROCESS_BEAM_DETAILS_B> F_PR_WEAVING_PROCESS_BEAM_DETAILS_B { get; set; }
        public ICollection<F_PR_WEAVING_PROCESS_DETAILS_B> F_PR_WEAVING_PROCESS_DETAILS_B { get; set; }
        public ICollection<F_PR_WEAVING_PROCESS_DETAILS_S> F_PR_WEAVING_PROCESS_DETAILS_S { get; set; }
        public ICollection<RND_FABTEST_SAMPLE> RND_FABTEST_SAMPLE { get; set; }
        public ICollection<RND_FABTEST_SAMPLE_BULK> RND_FABTEST_SAMPLE_BULK { get; set; }
    }
}
