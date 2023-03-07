using System.Collections.Generic;
using DenimERP.Models;

namespace DenimERP.ViewModels.Rnd.Finish
{
    public class CreateRndSampleInfoFinishViewModel
    {
        public CreateRndSampleInfoFinishViewModel()
        {
            RndSampleinfoFinishing = new RND_SAMPLEINFO_FINISHING();
            RndFabtestGreys = new List<RND_FABTEST_GREY>();
        }

        public RND_SAMPLEINFO_FINISHING RndSampleinfoFinishing { get; set; }
        public RND_FABTEST_GREY RndFabtestGrey { get; set; }
        public List<RND_FABTEST_GREY> RndFabtestGreys { get; set; }
        public List<BAS_COLOR> BasColors { get; set; }
    }
}
