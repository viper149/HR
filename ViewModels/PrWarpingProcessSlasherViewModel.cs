using System.Collections.Generic;
using DenimERP.Models;

namespace DenimERP.ViewModels
{
    public class PrWarpingProcessSlasherViewModel
    {
        public PrWarpingProcessSlasherViewModel()
        {
            FPrWarpingProcessDwYarnConsumDetailsList = new List<F_PR_WARPING_PROCESS_DW_YARN_CONSUM_DETAILS>();
            FPrWarpingProcessDwDetailsList = new List<F_PR_WARPING_PROCESS_DW_DETAILS>();
        }

        public F_PR_WARPING_PROCESS_DW_MASTER FPrWarpingProcessDwMaster { get; set; }
        public F_PR_WARPING_PROCESS_DW_DETAILS FPrWarpingProcessDwDetails { get; set; }
        public List<F_PR_WARPING_PROCESS_DW_DETAILS> FPrWarpingProcessDwDetailsList { get; set; }
        public F_PR_WARPING_PROCESS_DW_YARN_CONSUM_DETAILS FPrWarpingProcessDwYarnConsumDetails { get; set; }
        public List<F_PR_WARPING_PROCESS_DW_YARN_CONSUM_DETAILS> FPrWarpingProcessDwYarnConsumDetailsList { get; set; }
        public List<PL_PRODUCTION_SETDISTRIBUTION> PlProductionSetDistributions { get; set; }
        public List<F_BAS_BALL_INFO> BasBallInfos { get; set; }
        public List<F_PR_WARPING_MACHINE> FPrWarpingMachines { get; set; }
    }
}
