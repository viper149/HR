using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels;

namespace DenimERP.ServiceInterfaces
{
    public interface IF_PR_INSPECTION_FABRIC_D_DETAILS : IBaseService<F_PR_INSPECTION_FABRIC_D_DETAILS>
    {
        Task<FPrInspectionFabricDispatchViewModel> GetRollsAsync(DateTime dDate);
        Task<IEnumerable<F_PR_INSPECTION_FABRIC_D_DETAILS>> GetRollListAsync();
        Task<F_PR_INSPECTION_FABRIC_D_DETAILS> GetRollIDetails(int rollId);
        Task<bool> GetRollBalance(int rollId, double fullLength);
        Task<FPrInspectionFabricDispatchViewModel> GetRollsByScanAsync(FPrInspectionFabricDispatchViewModel fPrInspectionFabricDispatchViewModel);
        Task<F_PR_INSPECTION_FABRIC_D_DETAILS> FindRollDetails(int rollId, DateTime dDate);
        Task<F_PR_INSPECTION_FABRIC_D_DETAILS> GetRollIdByRollNo(string rollNo);
        Task<FPrInspectionFabricDispatchViewModel> GetRollDetailsList(FPrInspectionFabricDispatchViewModel fPrInspectionFabricDispatchViewModel);
        Task<F_PR_INSPECTION_FABRIC_D_DETAILS> GetRcvRollIdByRollNo(string rollNo);
    }
}
