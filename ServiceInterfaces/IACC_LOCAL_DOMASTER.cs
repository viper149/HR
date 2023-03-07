using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels;

namespace DenimERP.ServiceInterfaces
{
    public interface IACC_LOCAL_DOMASTER : IBaseService<ACC_LOCAL_DOMASTER>
    {
        Task<IEnumerable<ACC_LOCAL_DOMASTER>> GetAccDoMasterAllAsync();
        Task<bool> FindByDoNoInUseAsync(string doNo);
        Task<string> GetLastDoNoAsync();
        Task<AccLocalDoMasterViewModel> GetInitObjectsByAsync(AccLocalDoMasterViewModel accLocalDoMasterViewModel);
        Task<AccLocalDoMasterViewModel> GetInitObjectsForDetailsTable(AccLocalDoMasterViewModel accLocalDoMasterViewModel);
        Task<AccLocalDoMasterViewModel> GetLocalSaleOrdersByAsync(string search, int page);
        Task<AccLocalDoMasterViewModel> GetCommercialExportLCByAsync(AccLocalDoMasterViewModel accLocalDoMasterViewModel, string search, int page);
        Task<IEnumerable<COM_EX_LCDETAILS>> GetGetStylesLcWiseByAsync(int lcId);
        Task<dynamic> GetOtherInfoByAsync(AccLocalDoMasterViewModel accLocalDoMasterViewModel);
        Task<AccLocalDoMasterViewModel> FindByIdIncludeAllByAsync(int trnsId);
        Task<ACC_LOCAL_DOMASTER> FindByIdIncludeAllForDeleteAsync(int parse);
    }
}
