using System.Collections.Generic;
using DenimERP.Models;

namespace DenimERP.ViewModels.Factory.Production
{
    public class PrWarpingProcessRopeViewModel
    {
        public PrWarpingProcessRopeViewModel()
        {
            FPrWarpingProcessRopeYarnConsumDetailsList = new List<F_PR_WARPING_PROCESS_ROPE_YARN_CONSUM_DETAILS>();
            FPrWarpingProcessRopeDetailsList = new List<F_PR_WARPING_PROCESS_ROPE_DETAILS>();
            FPrWarpingProcessRopeBallDetailsList = new List<F_PR_WARPING_PROCESS_ROPE_BALL_DETAILS>();
        }

        public F_PR_WARPING_PROCESS_ROPE_MASTER FPrWarpingProcessRopeMaster { get; set; }
        public F_PR_WARPING_PROCESS_ROPE_DETAILS FPrWarpingProcessRopeDetails { get; set; }
        public F_PR_WARPING_PROCESS_ROPE_BALL_DETAILS FPrWarpingProcessRopeBallDetails { get; set; }
        public F_PR_WARPING_PROCESS_ROPE_YARN_CONSUM_DETAILS FPrWarpingProcessRopeYarnConsumDetails { get; set; }

        public List<F_PR_WARPING_PROCESS_ROPE_DETAILS> FPrWarpingProcessRopeDetailsList { get; set; }
        public List<PL_PRODUCTION_PLAN_DETAILS> PlProductionPlanDetailsList { get; set; }
        public List<PL_PRODUCTION_SETDISTRIBUTION> PlProductionSetDistributions { get; set; }
        public List<F_HRD_EMPLOYEE> FHrEmployees { get; set; }
        public List<BAS_YARN_COUNTINFO> BasYarnCountInfos { get; set; }
        public List<F_BAS_BALL_INFO> BasBallInfos { get; set; }
        public List<F_PR_WARPING_MACHINE> FPrWarpingMachines { get; set; }
        public List<F_PR_WARPING_PROCESS_ROPE_YARN_CONSUM_DETAILS> FPrWarpingProcessRopeYarnConsumDetailsList { get; set; }
        public List<F_PR_WARPING_PROCESS_ROPE_BALL_DETAILS> FPrWarpingProcessRopeBallDetailsList { get; set; }
    }
}
