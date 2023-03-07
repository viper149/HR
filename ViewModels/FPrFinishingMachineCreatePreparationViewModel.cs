using System.Collections.Generic;
using DenimERP.Models;

namespace DenimERP.ViewModels
{
    public class FPrFinishingMachineCreatePreparationViewModel
    {

        public FPrFinishingMachineCreatePreparationViewModel()
        {
            FPrFnChemicalConsumptions = new List<F_PR_FN_CHEMICAL_CONSUMPTION>();
            FPrFinighingDoffForMachines = new List<F_PR_FINIGHING_DOFF_FOR_MACHINE>();
        }

        public F_PR_FINISHING_MACHINE_PREPARATION FPrFinishingMachinePreparation { get; set; }
        public F_PR_FINIGHING_DOFF_FOR_MACHINE FPrFinighingDoffForMachine { get; set; }
        public F_PR_FN_CHEMICAL_CONSUMPTION FPrFnChemicalConsumption { get; set; }
        public List<F_PR_FINIGHING_DOFF_FOR_MACHINE> FPrFinighingDoffForMachines { get; set; }
        public List<F_PR_FN_CHEMICAL_CONSUMPTION> FPrFnChemicalConsumptions { get; set; }

        public List<TypeTableViewModel> FPrWeavingProcessDetailsBs { get; set; }
        public List<RND_FABRICINFO> RndFabricInfos { get; set; }
        public List<F_PR_FN_MACHINE_INFO> FPrFnMachineInfos { get; set; }
        public List<F_PR_FN_PROCESS_TYPEINFO> FPrFnProcessTypeInfos { get; set; }
        public List<F_CHEM_STORE_PRODUCTINFO> FChemStoreProductInfos { get; set; }

    }
}
