using System.Collections.Generic;
using DenimERP.Models;

namespace DenimERP.ViewModels.Rnd
{
    public class RndRsProductionOrderDetailsViewModel
    {
        public RND_SAMPLE_INFO_DYEING RndSampleInfoDyeing { get; set; }
        public PL_SAMPLE_PROG_SETUP PlSampleProgSetup { get; set; }
        public RND_SAMPLE_INFO_WEAVING RndSampleInfoWeaving { get; set; }
        public List<RND_SAMPLE_INFO_DETAILS> RndSampleInfoDetailsList { get; set; }
    }
}
