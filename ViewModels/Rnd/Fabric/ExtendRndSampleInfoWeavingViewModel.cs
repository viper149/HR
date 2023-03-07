using DenimERP.Models;
using DenimERP.ViewModels.Rnd.Finish;

namespace DenimERP.ViewModels.Rnd.Fabric
{
    public class ExtendRndSampleInfoWeavingViewModel
    {
        public RND_SAMPLEINFO_FINISHING RndSampleInfoFinishing { get; set; }
        public RND_SAMPLE_INFO_WEAVING RndSampleInfoWeaving { get; set; }
        public RND_SAMPLE_INFO_DYEING RndSampleInfoDyeing { get; set; }
        public RND_ANALYSIS_SHEET RndAnalysisSheet { get; set; }
        public FnEpiPpi FnEpiPpi { get; set; }
        public RND_FABTEST_GREY RndFabtestGrey { get; set; }
        public RND_FABTEST_SAMPLE RndFabtestSample { get; set; }
    }
}
