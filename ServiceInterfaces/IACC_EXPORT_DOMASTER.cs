using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels;
using DenimERP.ViewModels.AccountFinance;

namespace DenimERP.ServiceInterfaces
{
    public interface IACC_EXPORT_DOMASTER : IBaseService<ACC_EXPORT_DOMASTER>
    {
        Task<string> GetLastDoNoAsync();
        Task<COM_EX_LCINFO> GetLCDetailsByLCNoAsync(int? lcId);
        Task<bool> FindByDoNoInUseAsync(string doNo);
        Task<DetailsAccExportDoMasterViewModel> FindByDoTrnsIdAsync(int doTrnsId);
        Task<List<ACC_EXPORT_DOMASTER>> GetForDataTableByAsync();

        Task<ACC_EXPORT_DOMASTER> GetDoDetails(int doId);
        Task<COM_EX_LCINFO> GetFabStyleByLcIdAsync(int lcId);
        Task<AccExportDoMasterViewModel> FindByIdIncludeAllAsync(int doNo);
    }
}
