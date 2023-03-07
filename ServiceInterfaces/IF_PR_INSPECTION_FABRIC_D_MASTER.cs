using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels;

namespace DenimERP.ServiceInterfaces 
{
    public interface IF_PR_INSPECTION_FABRIC_D_MASTER : IBaseService<F_PR_INSPECTION_FABRIC_D_MASTER>
    {
        Task<IEnumerable<F_PR_INSPECTION_FABRIC_D_MASTER>> GetAllAsync();
        Task<FPrInspectionFabricDispatchViewModel> GetInitObjects(FPrInspectionFabricDispatchViewModel fPrInspectionFabricDispatchViewModel);
        Task<int> InsertAndGetIdAsync(F_PR_INSPECTION_FABRIC_D_MASTER fPrInspectionFabricDMaster);
        Task<F_PR_INSPECTION_FABRIC_D_MASTER> GetRollDetailsByDate(DateTime? dDate);
        Task<FPrInspectionFabricDispatchViewModel> FindByRollRcvIdAsync(int rollRcvId);
    }
}
