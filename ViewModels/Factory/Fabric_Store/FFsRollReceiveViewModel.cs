using System.Collections.Generic;
using DenimERP.Models;

namespace DenimERP.ViewModels.Factory.Fabric_Store
{
    public class FFsRollReceiveViewModel
    {
        public FFsRollReceiveViewModel()
        {
            FFsFabricRcvDetailsList = new List<F_FS_FABRIC_RCV_DETAILS>();
        }

        public F_FS_FABRIC_RCV_MASTER FFsFabricRcvMaster { get; set; }
        public F_FS_FABRIC_RCV_DETAILS FFsFabricRcvDetails { get; set; }
        public List<F_FS_FABRIC_RCV_DETAILS> FFsFabricRcvDetailsList { get; set; }
        public List<F_BAS_SECTION> FBasSections { get; set; }
        public List<F_FS_LOCATION> FFsLocations { get; set; }
    }
}
