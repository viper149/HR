using System.Collections.Generic;
using DenimERP.Models;
using DenimERP.ViewModels.Rnd;
using DenimERP.ViewModels.Rnd.Fabric;

namespace DenimERP.ViewModels
{
    public class RndFabricInfoViewModel
    {
        public RndFabricInfoViewModel()
        {
            //rND_FABRICINFO = new RND_FABRICINFO();
            //rND_FABRIC_COUNTINFO = new RND_FABRIC_COUNTINFO();
            //RndFabricInfoAndYarnConsumption = new RndFabricInfoAndYarnConsumption();
            //rND_YARNCONSUMPTION = new RND_YARNCONSUMPTION();
            //RndFinishtypes = new List<RND_FINISHTYPE>();
            //rND_DYEING_TYPEs = new List<RND_DYEING_TYPE>();
            //bAS_YARN_COUNTINFOs = new List<BAS_YARN_COUNTINFO>();
            //bAS_YARN_LOTINFOs = new List<BAS_YARN_LOTINFO>();
            //bAS_SUPPLIERINFOs = new List<BAS_SUPPLIERINFO>();
            //bAS_BUYERINFOs = new List<BAS_BUYERINFO>();
            //RndWeaves = new List<RND_WEAVE>();
            //RndFinishmcs = new List<RND_FINISHMC>();
            //Items = new List<RndFabricInfoAndYarnConsumption>();
            //RndSampleInfoWeavings = new List<RND_SAMPLE_INFO_WEAVING>();
            LoomTypes = new List<LOOM_TYPE>();
            //AgeGroupViewModels = new List<AgeGroupViewModel>();
            //TargetGenderViewModels = new List<TargetGenderViewModel>();
            //TargetCharacterViewModels = new List<TargetCharacterViewModel>();
            //TargetPriceSegmentViewModels = new List<TargetPriceSegmentViewModel>();
            //TargetFitStyleViewModels = new List<TargetFitStyleViewModel>();
            //SegmentSeasonViewModels = new List<SegmentSeasonViewModel>();
            //ComSegmentViewModels = new List<ComSegmentViewModel>();
            //OtherSimilarViewModels = new List<OtherSimilarViewModel>();
            RndFabricCountInfos = new List<RND_FABRIC_COUNTINFO>();
            RndYarnConsumptions = new List<RND_YARNCONSUMPTION>();
            FHrEmployeesList = new List<F_HRD_EMPLOYEE>();

            rND_FABRICINFO = new RND_FABRICINFO
            {
                METHOD = "ISO-6330-6N"
            };
        }
        public string WrapRatio { get; set; }
        public string WeaveRatio { get; set; }
        public RND_FABRICINFO rND_FABRICINFO { get; set; }
        public RND_FABRIC_COUNTINFO rND_FABRIC_COUNTINFO { get; set; }
        public RND_YARNCONSUMPTION rND_YARNCONSUMPTION { get; set; }

        public List<RND_FABRIC_COUNTINFO> RndFabricCountInfos { get; set; }
        public List<RND_YARNCONSUMPTION> RndYarnConsumptions { get; set; }

        public List<RND_DYEING_TYPE> rND_DYEING_TYPEs { get; set; }
        public List<BAS_YARN_COUNTINFO> bAS_YARN_COUNTINFOs { get; set; }
        public List<BAS_YARN_LOTINFO> bAS_YARN_LOTINFOs { get; set; }
        public List<BAS_SUPPLIERINFO> bAS_SUPPLIERINFOs { get; set; }
        public List<BAS_COLOR> BasColors { get; set; }
        public List<BAS_BUYERINFO> bAS_BUYERINFOs { get; set; }
        public List<RND_FINISHTYPE> RndFinishtypes { get; set; }
        public List<RND_WEAVE> RndWeaves { get; set; }
        public List<RND_FINISHMC> RndFinishmcs { get; set; }
        //public List<RndFabricInfoAndYarnConsumption> Items { get; set; }
        public List<RND_SAMPLE_INFO_WEAVING> RndSampleInfoWeavings { get; set; }
        public List<RND_SAMPLEINFO_FINISHING> RndSampleInfoFinishings { get; set; }
        public List<LOOM_TYPE> LoomTypes { get; set; }
        public List<AgeGroupViewModel> AgeGroupViewModels { get; set; }
        public List<TargetGenderViewModel> TargetGenderViewModels { get; set; }
        public List<TargetCharacterViewModel> TargetCharacterViewModels { get; set; }
        public List<TargetPriceSegmentViewModel> TargetPriceSegmentViewModels { get; set; }
        public List<TargetFitStyleViewModel> TargetFitStyleViewModels { get; set; }
        public List<SegmentSeasonViewModel> SegmentSeasonViewModels { get; set; }
        public List<ComSegmentViewModel> ComSegmentViewModels { get; set; }
        public List<OtherSimilarViewModel> OtherSimilarViewModels { get; set; }
        public List<RND_FABTEST_SAMPLE> FabTestSamples { get; set; }
        public List<RND_FABTEST_GREY> FabTestGreyList { get; set; }
        public List<F_HRD_EMPLOYEE> FHrEmployeesList { get; set; }


        public List<RndFabricCountinfoAndRndYarnConsumptionViewModel> rndFabricCountinfoAndRndYarnConsumptionViewModels { get; set; }

        public int RemoveIndexValue { get; set; }
        //public int NumberOfItems
        //{
        //    get => Items.Count();
        //}
    }
}
