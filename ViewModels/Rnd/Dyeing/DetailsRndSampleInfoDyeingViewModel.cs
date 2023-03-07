using System.Collections.Generic;
using DenimERP.Models;

namespace DenimERP.ViewModels.Rnd.Dyeing
{
    public class DetailsRndSampleInfoDyeingViewModel
    {
        public RND_SAMPLE_INFO_DYEING RndSampleInfoDyeing { get; set; }
        public List<RND_SAMPLE_INFO_DETAILS> RndSampleInfoDetailses { get; set; }
    }
}
