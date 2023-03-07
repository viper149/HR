using System.Collections.Generic;
using DenimERP.Models;

namespace DenimERP.ViewModels.Factory.Production
{
    public class FDyeingProcessRopeViewModel
    {
        public FDyeingProcessRopeViewModel()
        {
            FDyeingProcessRopeChemList = new List<F_DYEING_PROCESS_ROPE_CHEM>();
            FDyeingProcessRopeDetailsList = new List<F_DYEING_PROCESS_ROPE_DETAILS>();
            PlProductionPlanMasterList = new List<PL_PRODUCTION_PLAN_MASTER>();
        }
        public F_DYEING_PROCESS_ROPE_MASTER FDyeingProcessRopeMaster { get; set; }
        public F_DYEING_PROCESS_ROPE_DETAILS FDyeingProcessRopeDetails { get; set; }
        public F_DYEING_PROCESS_ROPE_CHEM FDyeingProcessRopeChem { get; set; }

        public List<F_DYEING_PROCESS_ROPE_DETAILS> FDyeingProcessRopeDetailsList { get; set; }
        public List<F_DYEING_PROCESS_ROPE_CHEM> FDyeingProcessRopeChemList { get; set; }

        public List<PL_PRODUCTION_PLAN_MASTER> PlProductionPlanMasterList { get; set; }
        public List<F_PR_WARPING_PROCESS_ROPE_BALL_DETAILS> PrWarpingProcessRopeBallDetailsList { get; set; }
        public List<F_CHEM_STORE_PRODUCTINFO> ChemStoreProductInfoList { get; set; }
        //public List<PL_PRODUCTION_SETDISTRIBUTION> PlProductionSetDistributions { get; set; }
        public List<PL_PRODUCTION_PLAN_DETAILS> PlProductionPlanDetailsList { get; set; }
        public List<F_PR_ROPE_INFO> FPrRopeInfos { get; set; }
        public List<F_PR_ROPE_MACHINE_INFO> FPrRopeMachineInfos { get; set; }
        public List<F_PR_TUBE_INFO> FPrTubeInfos { get; set; }
    }
}
