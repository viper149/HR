using System.Collections.Generic;
using DenimERP.Models;

namespace DenimERP.ViewModels.Rnd
{
    public class RndProductionOrderDetailViewModel
    {
        public RndProductionOrderDetailViewModel()
        {
            ComExPiDetailsList = new List<COM_EX_PI_DETAILS>();
            RndFabricCountInfoViewModels = new List<RndFabricCountInfoViewModel>();
            RndFabricCountInfos = new List<RND_FABRIC_COUNTINFO>();
            RndYarnconsumptions = new List<RND_YARNCONSUMPTION>();
            BasYarnLotInfos = new List<BAS_YARN_LOTINFO>();
        }

        public COM_EX_PI_DETAILS ComExPiDetails { get; set; }
        public RND_SAMPLE_INFO_DYEING RndSampleInfoDyeing { get; set; }
        public List<COM_EX_PI_DETAILS> ComExPiDetailsList { get; set; }
        public List<RndFabricCountInfoViewModel> RndFabricCountInfoViewModels { get; set; }
        public List<RND_FABRIC_COUNTINFO> RndFabricCountInfos { get; set; }
        public List<RND_YARNCONSUMPTION> RndYarnconsumptions { get; set; }
        public List<BAS_YARN_LOTINFO> BasYarnLotInfos { get; set; }
        public RND_FABRICINFO RndFabricInfo { get; set; }
        public COM_EX_PIMASTER ComExPimaster { get; set; }
        public RND_PRODUCTION_ORDER RndProductionOrder { get; set; }
        public PL_PRODUCTION_SETDISTRIBUTION PlProductionSetDistribution { get; set; }
        public PL_PRODUCTION_PLAN_DETAILS PlProductionPlanDetails { get; set; }
      
        public int OrdRepeat { get; set; }
    }
}
