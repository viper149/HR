using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels.Factory.Production;
using DenimERP.ViewModels.Rnd;

namespace DenimERP.ServiceInterfaces
{
    public interface IF_PR_INSPECTION_PROCESS_MASTER:IBaseService<F_PR_INSPECTION_PROCESS_MASTER>
    {
        Task<IEnumerable<F_PR_INSPECTION_PROCESS_MASTER>> GetAllAsync();
        Task<IEnumerable<F_PR_INSPECTION_PROCESS_DETAILS>> GetAllDAsync();
        Task<F_PR_INSPECTION_PROCESS_MASTER> FindByIdAllAsync(int insId);
        Task<FPrInspectionProcessViewModel> GetInitObjects(FPrInspectionProcessViewModel prInspectionProcessViewModel);
        Task<int> InsertAndGetIdAsync(F_PR_INSPECTION_PROCESS_MASTER fPrInspectionProcessMaster);
        Task<string> GetRollNoBySetId(int? setId);
        Task<RndProductionOrderDetailViewModel> GetSetDetails(int setId);
        Task<bool> GetRollConfirm(string roll);
        Task<dynamic> GetRemarks();
        Task<dynamic> GetConstruction();
    }
}
