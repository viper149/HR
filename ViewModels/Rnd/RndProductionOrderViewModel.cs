using System.Collections.Generic;
using DenimERP.Models;

namespace DenimERP.ViewModels.Rnd
{
    public class RndProductionOrderViewModel
    {
        public RndProductionOrderViewModel()
        {
          //  PlOrderwiseLotInfoList = new List<PL_ORDERWISE_LOTINFO>();
        }
        public RND_PRODUCTION_ORDER RndProductionOrder { get; set; }
        //public PL_ORDERWISE_LOTINFO PlOrderwiseLotInfo { get; set; }
        //public List<PL_ORDERWISE_LOTINFO> PlOrderwiseLotInfoList { get; set; }
        public List<RND_ORDER_TYPE> RndOrderTypes { get; set; }
        public List<RND_ORDER_REPEAT> RndOrderRepeats { get; set; }
        public List<COM_EX_PI_DETAILS> ComExPiDetailsList { get; set; }
        public List<RND_MSTR_ROLL> RndMstrRolls { get; set; }
        public List<BAS_YARN_LOTINFO> BasYarnLotInfos { get; set; }
        public List<BAS_SUPPLIERINFO> BasSupplierInfos { get; set; }
        public List<RndFabricCountInfoViewModel> RndFabricCountInfoViewModels { get; set; }
        public List<YARNFOR> Yarnfor { get; set; }
        public UpdateCountInfoViewModel UpdateCountInfoViewModel { get; set; }
        public List<BAS_YARN_COUNTINFO> BasYarnCountInfos{ get; set; }
    }
}
