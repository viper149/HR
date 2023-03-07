using System.Collections.Generic;
using DenimERP.Models;

namespace DenimERP.ViewModels.Rnd
{
    public class RndAnalysisSheetInfoViewModel
    {
        public RndAnalysisSheetInfoViewModel()
        {
            RndAnalysisSheetDetailsList = new List<RND_ANALYSIS_SHEET_DETAILS>();
            BasBrandinfos = new List<BAS_BRANDINFO>();
            BasYarnCountinfos = new List<BAS_YARN_COUNTINFO>();
        }

        public RND_ANALYSIS_SHEET RndAnalysisSheet { get; set; }
        public RND_ANALYSIS_SHEET_DETAILS RndAnalysisSheetDetails { get; set; }
        public List<RND_ANALYSIS_SHEET_DETAILS> RndAnalysisSheetDetailsList { get; set; }
        public List<MKT_TEAM> MktTeams { get; set; }
        public List<BAS_BUYERINFO> BasBuyerInfos { get; set; }
        public List<RND_WEAVE> RndWeaves { get; set; }
        public List<RND_FINISHTYPE> RndFinishTypes { get; set; }
        public List<MKT_SWATCH_CARD> MktSwatchCards { get; set; }
        public List<BAS_BRANDINFO> BasBrandinfos { get; set; }
        public List<BAS_YARN_COUNTINFO> BasYarnCountinfos { get; set; }
    }
}
