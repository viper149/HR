using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels.Factory.Fabric_Store;

namespace DenimERP.ServiceInterfaces
{
    public interface IF_FS_DELIVERYCHALLAN_PACK_MASTER : IBaseService<F_FS_DELIVERYCHALLAN_PACK_MASTER>
    {
        Task<IEnumerable<dynamic>> GetChallanListAsync();
        Task<List<F_FS_DELIVERYCHALLAN_PACK_MASTER>> GetAllByIssueType();
        Task<FabDeliveryChallanViewModel> GetInitObjects(FabDeliveryChallanViewModel fabDeliveryChallanViewModel);
        FabDeliveryChallanViewModel FindAllByIdAsync(int id);
        Task<int> InsertAndGetIdAsync(F_FS_DELIVERYCHALLAN_PACK_MASTER fFsDeliveryChallanPackMaster);
        Task<double> GetDoBalance(int doId);
        Task<dynamic> GetPiBalance(int piId, int trnsId);
        Task<double> GetSoBalance(int soId);
        Task<F_FS_DELIVERYCHALLAN_PACK_MASTER> GetDoWisePackingRollList(int doId);
        Task<dynamic> GetChallanNo(int delType);
    }
}
