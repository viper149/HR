using System.Collections.Generic;
using DenimERP.Models;

namespace DenimERP.ViewModels.Rnd.Grey
{
    public class DetailsRndFabTestGreyViewModel
    {
        public RND_FABTEST_GREY RndFabtestGrey { get; set; }
        public List<RND_SAMPLE_INFO_WEAVING_DETAILS> RndSampleInfoWeavingDetailses { get; set; }
        public List<RND_SAMPLE_INFO_DETAILS> RndSampleInfoDetailses { get; set; }
    }
}
