using System.Collections.Generic;

namespace DenimERP.Models
{
    public partial class F_HR_SHIFT_INFO
    {
        public F_HR_SHIFT_INFO()
        {
            RND_FABTEST_BULK = new HashSet<RND_FABTEST_BULK>();
            RND_FABTEST_SAMPLE= new HashSet<RND_FABTEST_SAMPLE>();
            RND_FABTEST_GREY = new HashSet<RND_FABTEST_GREY>();
            RND_FABTEST_SAMPLE_BULK = new HashSet<RND_FABTEST_SAMPLE_BULK>();
            F_PR_INSPECTION_REJECTION_B = new HashSet<F_PR_INSPECTION_REJECTION_B>();
            F_PR_WEAVING_WORKLOAD_EFFICIENCELOSS = new HashSet<F_PR_WEAVING_WORKLOAD_EFFICIENCELOSS>();
            F_PR_INSPECTION_CUTPCS_TRANSFER = new HashSet<F_PR_INSPECTION_CUTPCS_TRANSFER>();
        }
        public int ID { get; set; }
        public string SHIFT { get; set; }
        public string REMARKS { get; set; }
        public int? OLD_CODE { get; set; }

        public ICollection<RND_FABTEST_BULK> RND_FABTEST_BULK { get; set; }
        public ICollection<RND_FABTEST_SAMPLE> RND_FABTEST_SAMPLE { get; set; }
        public ICollection<RND_FABTEST_GREY> RND_FABTEST_GREY { get; set; }
        public ICollection<RND_FABTEST_SAMPLE_BULK> RND_FABTEST_SAMPLE_BULK { get; set; }
        public ICollection<F_PR_INSPECTION_REJECTION_B> F_PR_INSPECTION_REJECTION_B { get; set; }
        public ICollection<F_PR_WEAVING_WORKLOAD_EFFICIENCELOSS> F_PR_WEAVING_WORKLOAD_EFFICIENCELOSS { get; set; }
        public ICollection<F_PR_INSPECTION_CUTPCS_TRANSFER> F_PR_INSPECTION_CUTPCS_TRANSFER { get; set; }
    }
}
