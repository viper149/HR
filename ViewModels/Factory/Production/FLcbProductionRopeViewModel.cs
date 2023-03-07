using System.Collections.Generic;
using DenimERP.Models;

namespace DenimERP.ViewModels.Factory.Production
{
    public class FLcbProductionRopeViewModel
    {
        public FLcbProductionRopeViewModel()
        {
            FLcbProductionRopeDetailsList = new List<F_LCB_PRODUCTION_ROPE_DETAILS>();
        }

        public F_LCB_PRODUCTION_ROPE_MASTER FLcbProductionRopeMaster { get; set; }
        public F_LCB_PRODUCTION_ROPE_DETAILS FLcbProductionRopeDetails { get; set; }
        public F_LCB_PRODUCTION_ROPE_PROCESS_INFO FLcbProductionRopeProcessInfo { get; set; }
        public List<F_LCB_PRODUCTION_ROPE_DETAILS> FLcbProductionRopeDetailsList { get; set; }
        public List<PL_PRODUCTION_PLAN_MASTER> PlProductionPlanMasters { get; set; }
        //public List<PL_PRODUCTION_SETDISTRIBUTION> PlProductionSetDistributions { get; set; }
        public List<PL_PRODUCTION_PLAN_DETAILS> PlProductionPlanDetailsList { get; set; }
        public List<PL_PRODUCTION_PLAN_DETAILS> PlProductionPlanDetailsEditList { get; set; }
        public List<F_DYEING_PROCESS_ROPE_DETAILS> FDyeingProcessRopeDetailsList { get; set; }
        public List<F_LCB_MACHINE> FLcbMachines { get; set; }
        public List<F_LCB_BEAM> FLcbBeams { get; set; }
        public List<F_HRD_EMPLOYEE> FHrEmployees { get; set; }
    }
}
