using System.Collections.Generic;
using DenimERP.Models;

namespace DenimERP.ViewModels.Marketing
{
    public class CreateMktSwatchCardViewModel
    {
        public CreateMktSwatchCardViewModel()
        {
            BasColors = new List<BAS_COLOR>();
        }

        public MKT_SWATCH_CARD MktSwatchCard { get; set; }
        public List<BAS_BUYERINFO> BasBuyerinfos { get; set; }
        public List<RND_FINISHTYPE> RndFinishtypes { get; set; }
        public List<MKT_TEAM> MktTeams { get; set; }
        public List<BAS_COLOR> BasColors { get; set; }
    } 
}
