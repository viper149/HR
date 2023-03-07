using System.Collections.Generic;
using DenimERP.Models;

namespace DenimERP.ViewModels
{
    public class FPrFinishingBeamReceiveViewModel
    {
        public F_PR_FINISHING_BEAM_RECEIVE FPrFinishingBeamReceive { get; set; }
        public List<TypeTableViewModel> FPrWeavingProcessBeamDetailsBs { get; set; }
        public List<RND_FABRICINFO> RndFabricInfos { get; set; }
        public List<PL_PRODUCTION_SETDISTRIBUTION> PlProductionSetDistributions { get; set; }
        public List<F_HRD_EMPLOYEE> FHrEmployees { get; set; }
    }
}
