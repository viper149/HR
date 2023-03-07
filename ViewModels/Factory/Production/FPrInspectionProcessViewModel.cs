using System.Collections.Generic;
using DenimERP.Models;

namespace DenimERP.ViewModels.Factory.Production
{
    public class FPrInspectionProcessViewModel
    {
        public FPrInspectionProcessViewModel()
        {
            FPrInspectionProcessDetailsList = new List<F_PR_INSPECTION_PROCESS_DETAILS>();
        }
        public F_PR_INSPECTION_PROCESS_MASTER FPrInspectionProcessMaster { get; set; }
        public F_PR_INSPECTION_PROCESS_DETAILS FPrInspectionProcessDetails { get; set; }
        public List<F_PR_INSPECTION_PROCESS_DETAILS> FPrInspectionProcessDetailsList { get; set; }
        public F_PR_INSPECTION_DEFECT_POINT FPrInspectionDefectPoint { get; set; }
        
        public List<PL_PRODUCTION_SETDISTRIBUTION> PlProductionSetDistributions { get; set; }
        public List<F_HRD_EMPLOYEE> FHrEmployees { get; set; }
        public List<F_PR_FINISHING_FNPROCESS> FPrFinTrollies { get; set; }
        public List<F_PR_FINISHING_FNPROCESS> FPrFinTrolliesEdit { get; set; }
        public List<F_PR_INSPECTION_MACHINE> FPrInspectionMachines { get; set; }
        public List<F_PR_INSPECTION_BATCH> FPrInspectionBatches { get; set; }
        public List<F_PR_INSPECTION_DEFECTINFO> FPrInspectionDefectInfos { get; set; }
        public List<F_PR_INSPECTION_PROCESS> FPrInspectionProcess { get; set; }
        public List<F_BAS_SECTION> FBasSections { get; set; }
        public List<F_PR_INSPECTION_PROCESS_DETAILS> RollList { get; set; }
        public List<RND_FABRICINFO> StyleList { get; set; }
    }
}
