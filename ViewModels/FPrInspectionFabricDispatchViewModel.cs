using System.Collections.Generic;
using DenimERP.Models;

namespace DenimERP.ViewModels
{
    public class FPrInspectionFabricDispatchViewModel
    {
        public FPrInspectionFabricDispatchViewModel()
        {
            FPrInspectionFabricDDetailsList = new List<F_PR_INSPECTION_FABRIC_D_DETAILS>();
            FBasSections = new List<F_BAS_SECTION>();
            FFsLocations = new List<F_FS_LOCATION>();
        }

        public F_PR_INSPECTION_FABRIC_D_MASTER FPrInspectionFabricDMaster { get; set; }
        public F_PR_INSPECTION_FABRIC_D_DETAILS FPrInspectionFabricDDetails { get; set; }

        public List<F_BAS_SECTION> FBasSections { get; set; }
        public List<F_FS_LOCATION> FFsLocations { get; set; }
        public List<F_PR_INSPECTION_FABRIC_D_DETAILS> FPrInspectionFabricDDetailsList { get; set; }

        public int RemoveIndex { get; set; }
        
    }
}
