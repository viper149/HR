using System.Collections.Generic;

namespace DenimERP.Models
{
    public class FDyeingProcessSlasherViewModel
    {
        public FDyeingProcessSlasherViewModel()
        {
            FPrSlasherDyeingDetailsList = new List<F_PR_SLASHER_DYEING_DETAILS>();
            FPrSlasherChemConsmList = new List<F_PR_SLASHER_CHEM_CONSM>();
        }

        public F_PR_SLASHER_DYEING_MASTER FPrSlasherDyeingMaster { get; set; }
        public F_PR_SLASHER_DYEING_DETAILS FPrSlasherDyeingDetails { get; set; }
        public F_PR_SLASHER_CHEM_CONSM FPrSlasherChemConsm { get; set; }
        public F_PR_SLASHER_MACHINE_INFO FPrSlasherMachineInfo { get; set; }
        public List<F_PR_SLASHER_DYEING_DETAILS> FPrSlasherDyeingDetailsList { get; set; }
        public List<F_PR_SLASHER_CHEM_CONSM> FPrSlasherChemConsmList { get; set; }
        public List<PL_PRODUCTION_SETDISTRIBUTION> PlProductionSetDistributions { get; set; }
        public List<F_HRD_EMPLOYEE> FHrEmployees { get; set; }
        public List<F_WEAVING_BEAM> FWeavingBeams { get; set; }
        public List<F_PR_SLASHER_MACHINE_INFO> FPrSlasherMachineInfos { get; set; }
        public List<F_CHEM_STORE_PRODUCTINFO> FChemStoreProductInfos { get; set; }
        public List<F_BAS_UNITS> Units { get; set; }
    }
}
