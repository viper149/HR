using System.Collections.Generic;
using DenimERP.Models;

namespace DenimERP.ViewModels.Factory.Fabric_Store
{
    public class FabDeliveryChallanViewModel
    {

        public FabDeliveryChallanViewModel()
        {
            FFsDeliverychallanPackDetailsList = new List<F_FS_DELIVERYCHALLAN_PACK_DETAILS>();
            DoLocalList = new List<ACC_LOCAL_DOMASTER>();
            ScList = new List<COM_EX_SCINFO>();
        }

        public F_FS_DELIVERYCHALLAN_PACK_MASTER FFsDeliveryChallanPackMaster { get; set; }
        public F_FS_DELIVERYCHALLAN_PACK_DETAILS FFsDeliverychallanPackDetails { get; set; }

        public List<F_FS_DELIVERYCHALLAN_PACK_DETAILS> FFsDeliverychallanPackDetailsList { get; set; }
        public List<F_FS_FABRIC_RCV_DETAILS> FFsFabricRcvDetailsList { get; set; }
        public List<ACC_EXPORT_DOMASTER> DoMasters { get; set; }
        public List<COM_EX_PIMASTER> PiMasters { get; set; }
        public List<COM_EX_PI_DETAILS> PiDetails { get; set; }
        public List<F_BAS_VEHICLE_INFO> FBasVehicleInfos { get; set; }
        public List<BAS_BUYERINFO> BasBuyerInfos { get; set; }
        public List<F_BAS_DELIVERY_TYPE> FBasDeliveryTypes { get; set; }
        public List<F_FS_ISSUE_TYPE> FFsIssueTypes { get; set; }
        public List<F_HRD_EMPLOYEE> FHrEmployees { get; set; }
        public List<ACC_LOCAL_DOMASTER> DoLocalList { get; set; }
        public List<COM_EX_SCINFO> ScList { get; set; }
    }
}
