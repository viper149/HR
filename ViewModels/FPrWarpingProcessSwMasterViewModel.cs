using System.Collections.Generic;
using DenimERP.Models;

namespace DenimERP.ViewModels
{
    public class FPrWarpingProcessSwMasterViewModel
    {
        public FPrWarpingProcessSwMasterViewModel()
        {
            FPrWarpingProcessSwYarnConsumDetailsList = new List<F_PR_WARPING_PROCESS_SW_YARN_CONSUM_DETAILS>();
            FPrWarpingProcessSwDetailsList = new List<F_PR_WARPING_PROCESS_SW_DETAILS>();
        }

        public F_PR_WARPING_PROCESS_SW_MASTER FPrWarpingProcessSwMaster { get; set; }
        public F_PR_WARPING_PROCESS_SW_DETAILS FPrWarpingProcessSwDetails { get; set; }
        public List<F_PR_WARPING_PROCESS_SW_DETAILS> FPrWarpingProcessSwDetailsList { get; set; }
        public F_PR_WARPING_PROCESS_SW_YARN_CONSUM_DETAILS FPrWarpingProcessSwYarnConsumDetails { get; set; }
        public List<F_PR_WARPING_PROCESS_SW_YARN_CONSUM_DETAILS> FPrWarpingProcessSwYarnConsumDetailsList { get; set; }
        public List<PL_PRODUCTION_SETDISTRIBUTION> PlProductionSetDistributions { get; set; }
        public List<F_BAS_BALL_INFO> BasBallInfos { get; set; }
        public List<F_PR_WARPING_MACHINE> FPrWarpingMachines { get; set; }

    }
}
