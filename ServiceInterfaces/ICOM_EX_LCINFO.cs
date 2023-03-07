using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels.Com;
using DenimERP.ViewModels.Com.Export;
using DenimERP.ViewModels.Com.InvoiceExport;
using DenimERP.ViewModels.Home;

namespace DenimERP.ServiceInterfaces
{
    public interface ICOM_EX_LCINFO : IBaseService<COM_EX_LCINFO>
    {
        #region MyRegion
        Task<IEnumerable<COM_EX_LCINFO>> ComExLcInfoListWithPaged(int pageNumber = 1, int pageSize = 5);
        Task<int> TotalNumberOfComExLcInfoList();
        Task<COM_EX_LCINFO> FindByLcIdIsDeleteAsync(int lcId);
        Task<COM_EX_LCINFO> FindByLcNoIsDeleteAsync(string lcNo);
        Task<COM_EX_LCINFO> FindByIdIncludeAllAsync(int lcId);
        Task<COM_EX_LCINFO> FindByIdIsDeleteFalseAsync(int id);
        Task<IEnumerable<COM_EX_LCINFO>> GetAllGreaterThan(DateTime compaDateTime);
        Task<ComExLcInfoViewModel> InitComExLcInfoViewModel(ComExLcInfoViewModel comExLcInfoViewModel);
        Task<IEnumerable<COM_EX_LCINFO>> GetForDataTableByAsync();
        Task<int> TotalPercentageOfComExLcInfoList(DateTime dateTime, int days = 7);
        Task<COM_EX_LCINFO> FindByLcIdWithDetailsIsDeleteAsync(int lcId);
        Task<COM_EX_PIMASTER> FindByPiIdIncludeAllAsync(int piId);
        Task<ComExLcInfoWithDetailsForEditViewModel> GetInitObjects(ComExLcInfoWithDetailsForEditViewModel comExLcInfoWithDetailsForEditViewModel);
        Task<double> GetSumOfTotalFromComExPiDetails(int lcId);
        Task<ComExInvoiceMasterCreateViewModel> GetLcDetailsByIdAsync(int lcId);
        Task<ComExLcInfoViewModel> FindByLcIdAllAsync(int lcId);
        Task<IEnumerable<COM_EX_LCINFO>> GetAllLcByLcNo(string lcNo);
        Task<CreateComExLcInfoViewModel> GetInitObjForDetailsTable(CreateComExLcInfoViewModel comExLcInfoViewModel);
        #endregion

        Task<CreateComExLcInfoViewModel> GetInitObjByAsync(CreateComExLcInfoViewModel comExInvoiceMasterViewModel);
        Task<CreateComExLcInfoViewModel> FindBy_IdIncludeAllAsync(int lcId);
        Task<bool> IsFileNoInUseByAsync(string fileNo);
        Task<string> GetNextLcFileNoByAsync(DateTime? dateTime = null, bool prevYear = false);
        Task<CreateComExLcInfoViewModel> FindByLcIdForDeleteAsync(int lcId);
        Task<DashboardViewModel> GetLCChartData();
    }
}
