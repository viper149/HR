using System.Collections.Generic;
using DenimERP.Models;

namespace DenimERP.ViewModels.Rnd.Fabric
{
    public class DyeingWeavingDetailsListViewModel
    {
        public RND_SAMPLE_INFO_WEAVING RndSampleInfoWeaving { get; set; }
        public List<RND_SAMPLE_INFO_WEAVING_DETAILS> RndSampleInfoWeavingDetailses { get; set; }
        public List<RND_SAMPLE_INFO_DETAILS> RndDyeingSampleInfoDetailses { get; set; }
    }
}
