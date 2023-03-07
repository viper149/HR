using System.Collections.Generic;
using DenimERP.Models;

namespace DenimERP.ViewModels.Factory.Production
{
    public class PrFinishingProcessViewModel
    {
        public PrFinishingProcessViewModel()
        {
            //FPrFinishingFabProcessList = new List<F_PR_FINISHING_FAB_PROCESS>();
            FPrFinishingFnProcessList = new List<F_PR_FINISHING_FNPROCESS>();
            //FPrFnChemicalConsumptionsList = new List<F_PR_FN_CHEMICAL_CONSUMPTION>();

        }

        public F_PR_FINISHING_PROCESS_MASTER FPrFinishingProcessMaster { get; set; }
        //public F_PR_FINISHING_FAB_PROCESS FPrFinishingFabProcess { get; set; }
        //public List<F_PR_FINISHING_FAB_PROCESS> FPrFinishingFabProcessList { get; set; }
        public F_PR_FINISHING_FNPROCESS FPrFinishingFnProcess { get; set; }
        public List<F_PR_FINISHING_FNPROCESS> FPrFinishingFnProcessList { get; set; }
        //public F_PR_FN_CHEMICAL_CONSUMPTION FPrFnChemicalConsumptions { get; set; }
        //public List<F_PR_FN_CHEMICAL_CONSUMPTION> FPrFnChemicalConsumptionsList { get; set; }

        //public List<PL_PRODUCTION_SETDISTRIBUTION> PlProductionSetDistributions { get; set; }
        public List<RND_FABRICINFO> FabricInfos { get; set; }
        public List<PL_PRODUCTION_SETDISTRIBUTION> PlProductionSetDistributions { get; set; }
        public List<F_HRD_EMPLOYEE> FHrEmployees { get; set; }
        public List<F_PR_WEAVING_PROCESS_BEAM_DETAILS_B> FPrWeavingProcessBeamDetailsBs { get; set; }
        public List<TypeTableViewModel> FPrWeavingProcessDetailsBs { get; set; }

        public List<F_PR_FIN_TROLLY> FPrFinTrollies { get; set; }
        public List<F_PR_PROCESS_MACHINEINFO> FPrFabProcessMachineInfos { get; set; }
        public List<F_PR_PROCESS_TYPE_INFO> FPrFabProcessTypeInfos { get; set; }
        public List<F_PR_FN_PROCESS_TYPEINFO> FPrFnProcessTypeInfos { get; set; }
        public List<F_PR_FN_MACHINE_INFO> FPrFnMachineInfos { get; set; }
        public List<F_BAS_SECTION> FBasSections { get; set; }
        public List<F_CHEM_STORE_PRODUCTINFO> FChemStoreProductInfos { get; set; }
        public List<F_PR_FINISHING_MACHINE_PREPARATION> FPrFinishingMachinePreparations { get; set; }
    }
}
