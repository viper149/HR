using System.Collections.Generic;
using DenimERP.Models;
using DenimERP.ViewModels.Rnd.Fabric;

namespace DenimERP.ViewModels.Rnd
{
    public class RndFabricInfoCountAndYarnConsumptionViewModel
    {
        public RndFabricInfoCountAndYarnConsumptionViewModel()
        {
            AgeGroupViewModels = new List<AgeGroupViewModel>();
            TargetGenderViewModels = new List<TargetGenderViewModel>();
            TargetCharacterViewModels = new List<TargetCharacterViewModel>();
            TargetPriceSegmentViewModels = new List<TargetPriceSegmentViewModel>();
            TargetFitStyleViewModels = new List<TargetFitStyleViewModel>();
            SegmentSeasonViewModels = new List<SegmentSeasonViewModel>();
            ComSegmentViewModels = new List<ComSegmentViewModel>();
            OtherSimilarViewModels = new List<OtherSimilarViewModel>();
        }

        public RND_FABRICINFO rND_FABRICINFO { get; set; }
        public List<RndFabricCountInfoViewModel> RndFabricCountInfoViewModels { get; set; }
        public List<RND_FABRIC_COUNTINFO> rND_FABRIC_COUNTINFOs { get; set; }
        public List<RND_YARNCONSUMPTION> rND_YARNCONSUMPTIONs { get; set; }

        public List<AgeGroupViewModel> AgeGroupViewModels { get; set; }
        public List<TargetGenderViewModel> TargetGenderViewModels { get; set; }
        public List<TargetCharacterViewModel> TargetCharacterViewModels { get; set; }
        public List<TargetPriceSegmentViewModel> TargetPriceSegmentViewModels { get; set; }
        public List<TargetFitStyleViewModel> TargetFitStyleViewModels { get; set; }
        public List<SegmentSeasonViewModel> SegmentSeasonViewModels { get; set; }
        public List<ComSegmentViewModel> ComSegmentViewModels { get; set; }
        public List<OtherSimilarViewModel> OtherSimilarViewModels { get; set; }
    }
}
