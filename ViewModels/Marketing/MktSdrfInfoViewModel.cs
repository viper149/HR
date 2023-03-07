using System.Collections.Generic;
using DenimERP.Models;

namespace DenimERP.ViewModels.Marketing
{
    public class MktSdrfInfoViewModel
    {
        public MKT_SDRF_INFO MktSdrfInfo { get; set; }
        public IEnumerable<MKT_DEV_TYPE> DevTypes { get; set; }
        public IEnumerable<BAS_TEAMINFO> TeamInfos { get; set; }
        public IEnumerable<MKT_TEAM> MktTeams { get; set; }
        public IEnumerable<BAS_BUYERINFO> BuyerInfos { get; set; }
        public IEnumerable<MKT_FACTORY> MktFactories { get; set; }
        public IEnumerable<COUNTRIES> Countries { get; set; }
        public IEnumerable<RND_FINISHTYPE> RndFinishType { get; set; }
        public IEnumerable<RND_ANALYSIS_SHEET> RndAnalysisSheets { get; set; }
        public IEnumerable<BAS_BRANDINFO> BasBrandinfos { get; set; }
    }
}
