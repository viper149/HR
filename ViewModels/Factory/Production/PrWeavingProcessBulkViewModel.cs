using System.Collections.Generic;
using DenimERP.Models;

namespace DenimERP.ViewModels.Factory.Production
{
    public class PrWeavingProcessBulkViewModel
    {
        public PrWeavingProcessBulkViewModel()
        {

            FPrWeavingProcessBeamDetailsBList = new List<F_PR_WEAVING_PROCESS_BEAM_DETAILS_B>();
            FPrWeavingWeftYarnConsumDetailsList = new List<F_PR_WEAVING_WEFT_YARN_CONSUM_DETAILS>();
        }

        public F_PR_WEAVING_PROCESS_MASTER_B FPrWeavingProcessMasterB { get; set; }
        public F_PR_WEAVING_PROCESS_DETAILS_B FPrWeavingProcessDetailsB { get; set; }
        public F_PR_WEAVING_PROCESS_BEAM_DETAILS_B FPrWeavingProcessBeamDetailsB { get; set; }
        public F_PR_WEAVING_WEFT_YARN_CONSUM_DETAILS FPrWeavingWeftYarnConsumDetails { get; set; }
        public List<F_PR_WEAVING_WEFT_YARN_CONSUM_DETAILS> FPrWeavingWeftYarnConsumDetailsList { get; set; }
        public List<F_PR_WEAVING_PROCESS_BEAM_DETAILS_B> FPrWeavingProcessBeamDetailsBList { get; set; }
        public List<PL_PRODUCTION_SETDISTRIBUTION> PlProductionSetDistributions { get; set; }
        public List<PL_PRODUCTION_SETDISTRIBUTION> PlProductionSetDistributionsForEdit { get; set; }
        public List<F_HRD_EMPLOYEE> FHrEmployees { get; set; }
        public List<F_WEAVING_BEAM> FWeavingBeams { get; set; }
        public List<F_LOOM_MACHINE_NO> FLoomMachineNo { get; set; }
        public List<F_PR_WEAVING_OTHER_DOFF> OtherDoffs { get; set; }
        public List<RND_SAMPLE_INFO_WEAVING> Weavings { get; set; }
        public List<LOOM_TYPE> LoomTypes { get; set; }
    }
}
