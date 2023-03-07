using System.Collections.Generic;
using DenimERP.Models;

namespace DenimERP.ViewModels.Factory.Production
{
    public class PrWeavingProcessViewModel
    {
        public F_PR_WEAVING_BEAM_RECEIVING FPrWeavingBeamReceiving { get; set; }
        public F_PR_WEAVING_WEFT_YARN_CONSUM_DETAILS FPrWeavingWeftYarnConsumDetails { get; set; }
        public List<F_PR_WEAVING_WEFT_YARN_CONSUM_DETAILS> FPrWeavingWeftYarnConsumDetailsList { get; set; }
        public List<PL_PRODUCTION_SETDISTRIBUTION> PlProductionSetDistributions { get; set; }
        public List<F_HRD_EMPLOYEE> FHrEmployees { get; set; }
    }
}
