using System.Collections.Generic;
using DenimERP.Models;

namespace DenimERP.ViewModels.Factory.Fabric_Store
{
    public class FFabricRollViewModel
    {
        public List<F_FS_FABRIC_RCV_DETAILS> ApprovedList { get; set; }
        public List<F_FS_FABRIC_RCV_DETAILS> RejectList { get; set; }
    }
}
