using System.Collections.Generic;

namespace DenimERP.Models
{
    public partial class F_BAS_TESTMETHOD
    {
        public F_BAS_TESTMETHOD()
        {
            RND_FABTEST_BULK = new HashSet<RND_FABTEST_BULK>();
            RND_FABTEST_SAMPLE = new HashSet<RND_FABTEST_SAMPLE>();
            RND_FABTEST_SAMPLE_BULK = new HashSet<RND_FABTEST_SAMPLE_BULK>();
        }
        public int TMID { get; set; }
        public string TMNAME { get; set; }
        public string REMARKS { get; set; }

        public ICollection<RND_FABTEST_BULK> RND_FABTEST_BULK { get; set; }
        public ICollection<RND_FABTEST_SAMPLE> RND_FABTEST_SAMPLE { get; set; }
        public ICollection<RND_FABTEST_SAMPLE_BULK> RND_FABTEST_SAMPLE_BULK { get; set; }
    }
}
