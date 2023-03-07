using System.Collections.Generic;
using DenimERP.Models;

namespace DenimERP.ViewModels.Rnd.Weaving
{
    public class RndSampleInfoWeavingWithDetailsViewModel
    {
        public RND_SAMPLE_INFO_WEAVING RndSampleInfoWeaving { get; set; }
        public List<RND_SAMPLE_INFO_WEAVING_DETAILS> RndSampleInfoWeavingDetailses { get; set; }
    }
}
