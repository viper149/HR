using System.Collections.Generic;
using DenimERP.Models;

namespace DenimERP.ViewModels
{
    public class FFsFabricClearanceViewModel
    {
        public FFsFabricClearanceViewModel()
        {
            FFsFabricClearanceDetailsList = new List<F_FS_FABRIC_CLEARANCE_DETAILS>();
        }
        public F_FS_FABRIC_CLEARANCE_MASTER FFsFabricClearanceMaster { get; set; }
        public F_FS_FABRIC_CLEARANCE_DETAILS FFsFabricClearanceDetails { get; set; }
        public List<F_FS_FABRIC_CLEARANCE_DETAILS> FFsFabricClearanceDetailsList { get; set; }

        public List<RND_FABRICINFO> RndFabricInfoList { get; set; }
        public List<BAS_BUYERINFO> BasBuyerInfos { get; set; }
        public List<RND_PRODUCTION_ORDER> RndProductionOrders { get; set; }
        
    }
}
