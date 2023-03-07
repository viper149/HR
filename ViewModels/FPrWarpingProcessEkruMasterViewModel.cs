using System.Collections.Generic;
using DenimERP.Models;

namespace DenimERP.ViewModels
{
    public class FPrWarpingProcessEkruMasterViewModel
    {
        public FPrWarpingProcessEkruMasterViewModel()
        {
            FPrWarpingProcessEcruDetailsList = new List<F_PR_WARPING_PROCESS_ECRU_DETAILS>();
            FPrWarpingProcessEcruYarnConsumDetailsList = new List<F_PR_WARPING_PROCESS_ECRU_YARN_CONSUM_DETAILS>();
            PlProductionList = new List<PL_PRODUCTION_SETDISTRIBUTION>();
            FBasBallInfoList = new List<F_BAS_BALL_INFO>();
            FPrWarpingMachineList = new List<F_PR_WARPING_MACHINE>();
            RndFabricCountinfoList = new List<RND_FABRIC_COUNTINFO>();
            PlProductionSetDistributions = new List<PL_PRODUCTION_SETDISTRIBUTION>();
        }


        public F_PR_WARPING_PROCESS_ECRU_MASTER FPrWarpingProcessEcruMaster { get; set; }
        public F_PR_WARPING_PROCESS_ECRU_YARN_CONSUM_DETAILS FPrWarpingProcessEcruYarnConsumDetails { get; set; }
        public F_PR_WARPING_PROCESS_ECRU_DETAILS FPrWarpingProcessEcruDetails { get; set; }

        public List<F_PR_WARPING_PROCESS_ECRU_DETAILS> FPrWarpingProcessEcruDetailsList { get; set; }
        public List<F_PR_WARPING_PROCESS_ECRU_YARN_CONSUM_DETAILS> FPrWarpingProcessEcruYarnConsumDetailsList { get; set; }
        public List<PL_PRODUCTION_SETDISTRIBUTION> PlProductionList { get; set; }
        public List<F_BAS_BALL_INFO> FBasBallInfoList { get; set; }
        public List<F_PR_WARPING_MACHINE> FPrWarpingMachineList { get; set; }
        public List<RND_FABRIC_COUNTINFO> RndFabricCountinfoList { get; set; }
        public List<PL_PRODUCTION_SETDISTRIBUTION> PlProductionSetDistributions { get; set; }


    }
    
}
