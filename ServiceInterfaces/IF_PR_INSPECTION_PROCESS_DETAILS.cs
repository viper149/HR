using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels;
using DenimERP.ViewModels.Factory.Production;
using DenimERP.ViewModels.Home;

namespace DenimERP.ServiceInterfaces
{
    public interface IF_PR_INSPECTION_PROCESS_DETAILS:IBaseService<F_PR_INSPECTION_PROCESS_DETAILS>
    {
        Task<bool> IsRollNoExists(string rollNo);
        Task<F_PR_INSPECTION_PROCESS_MASTER> IsSetNoExists(int setNo);
        Task<FPrInspectionProcessViewModel> GetInitData(FPrInspectionProcessViewModel prInspectionProcessViewModel);
        Task<int> InsertAndGetIdAsync(F_PR_INSPECTION_PROCESS_DETAILS fPrInspectionProcessDetails);
        Task<IEnumerable<F_PR_FINISHING_FNPROCESS>> GetTrollyListBySetId(int setId);
        Task<bool> IsRollNoInUseAsync(string rollNo);
        Task<IEnumerable<F_PR_FINISHING_FNPROCESS>> GetTrollyListBySetIdEdit(int setId);
        Task<List<F_PR_INSPECTION_PROCESS_DETAILS>> GetRollListByInsIdAsync(int insId);
        Task<F_PR_INSPECTION_PROCESS_DETAILS> GetRollDetailsByInsIdAsync(int rollId);
        Task<F_PR_INSPECTION_PROCESS_DETAILS> GetDefectDetailsByInsIdAsync(int insId);

        Task<F_PR_FINISHING_FNPROCESS> GetTrollyDetails(int trollyId,int setId);
        Task<F_PR_INSPECTION_PROCESS_DETAILS> FindByRollNoAsync(string rollNO);
        Task<List<F_PR_INSPECTION_DEFECT_POINT>> GetDefectListByInsIdAsync(string rollNO);

        Task<List<F_PR_INSPECTION_PROCESS_DETAILS>> GetRollListByStyle(int fabcode);
        Task<List<F_PR_INSPECTION_PROCESS_DETAILS>> GetRollListByStyleDynamic(string search, int page, int fabcode);
        Task<List<F_PR_INSPECTION_PROCESS_DETAILS>> GetRollListByDate(DateTime? date);
        Task<List<ChartViewModel>> GetInspectionDateWiseLengthGraph();
        Task<DashboardViewModel> GetInspectionTotalLength();
        Task<IEnumerable<F_PR_INSPECTION_PROCESS_MASTER>> GetTopStyleProductionData();
    }
}
