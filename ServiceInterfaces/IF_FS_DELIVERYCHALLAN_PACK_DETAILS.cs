using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels.Factory.Fabric_Store;
using DenimERP.ViewModels.Home;

namespace DenimERP.ServiceInterfaces
{
    public interface IF_FS_DELIVERYCHALLAN_PACK_DETAILS:IBaseService<F_FS_DELIVERYCHALLAN_PACK_DETAILS>
    {
        Task<FabDeliveryChallanViewModel> GetRollDetailsAsync(FabDeliveryChallanViewModel fabDeliveryChallanViewModel);

        Task<FabDeliveryChallanViewModel> GetRollsByScanAsync(FabDeliveryChallanViewModel fabDeliveryChallanViewModel);
        Task<FabDeliveryChallanViewModel> GetRollDetailsList(FabDeliveryChallanViewModel fabDeliveryChallanViewModel);

        Task<FabDeliveryChallanViewModel> GetRollDetails(FabDeliveryChallanViewModel fabDeliveryChallanViewModel);
        Task<F_FS_FABRIC_RCV_DETAILS> GetRollIdByRollNo(string rollNo);
        Task<F_FS_FABRIC_CLEARANCE_DETAILS> GetRollShadeByRollNo(string rollNo);
        Task<F_FS_DELIVERYCHALLAN_PACK_DETAILS> GetRollDetailsByRcvid(int rcvId);

        Task<DashboardViewModel> GetFabricDeliveryChallanLength();
    }
}
