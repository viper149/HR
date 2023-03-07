using System.Collections.Generic;
using DenimERP.Models;

namespace DenimERP.ViewModels.Factory.Production
{
    public class PrWarpingProcessRopeDataViewModel
    {
        public PrWarpingProcessRopeDataViewModel()
        {
            PlProductionPlanDetails = new List<PL_PRODUCTION_PLAN_DETAILS>();
        }

        public PL_BULK_PROG_SETUP_D PlBulkProgSetupD { get; set; }
        public COM_EX_PI_DETAILS PiDetails { get; set; }
        public PL_PRODUCTION_SETDISTRIBUTION PlProductionSetDistribution { get; set; }
        public PL_PRODUCTION_PLAN_DETAILS PL_PRODUCTION_PLAN_DETAILS { get; set; }

        public List<PL_PRODUCTION_PLAN_DETAILS> PlProductionPlanDetails { get; set; }
    }
}
