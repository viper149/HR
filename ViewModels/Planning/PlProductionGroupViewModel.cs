using System.Collections.Generic;
using DenimERP.Models;

namespace DenimERP.ViewModels.Planning
{
    public class PlProductionGroupViewModel
    {
        public PlProductionGroupViewModel()
        {
            PlProductionPlanDetailsList = new List<PL_PRODUCTION_PLAN_DETAILS>();
            PlProductionSoDetailsList = new List<PL_PRODUCTION_SO_DETAILS>();
        }
        public PL_PRODUCTION_PLAN_MASTER PlProductionPlanMaster { get; set; }
        public PL_PRODUCTION_PLAN_DETAILS PlProductionPlanDetails { get; set; }
        public List<PL_PRODUCTION_PLAN_DETAILS> PlProductionPlanDetailsList { get; set; }
        public PL_PRODUCTION_SETDISTRIBUTION PlProductionSetDistribution { get; set; }
        public PL_PRODUCTION_SO_DETAILS PlProductionSoDetails { get; set; }
        public List<PL_PRODUCTION_SO_DETAILS> PlProductionSoDetailsList { get; set; }
        public List<TypeTableViewModel> ProductionOrderList { get; set; }
        public List<BAS_YARN_LOTINFO> LotInfoList { get; set; }
        public List<RND_DYEING_TYPE> DyeingTypes { get; set; }
        public List<PL_BULK_PROG_SETUP_D> PlBulkProgSetupDList { get; set; }
    }
}
