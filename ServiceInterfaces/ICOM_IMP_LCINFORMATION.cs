using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels.Com;

namespace DenimERP.ServiceInterfaces
{
    public interface ICOM_IMP_LCINFORMATION : IBaseService<COM_IMP_LCINFORMATION>
    {
        Task<bool> FindByLcNoAsync(string lcNo);
        Task<COM_IMP_LCINFORMATION> FindByIdInlcudeOtherObjAsync(int id);
        Task<int> TotalNumberOfComImpLcInformationList();
        Task<int> TotalPercentageOfComImpLcInformationList(DateTime dateTime, int days = 7);
        Task<ComImpLcInformationForCreateViewModel> GetComImpLcInformationForCreateViewModelValue(ComImpLcInformationForCreateViewModel comImpLcInformationForCreateViewModel);
        Task<int?> GetFileNumberByCategory(int categoryId);
        Task<IEnumerable<COM_IMP_LCINFORMATION>> FindByExportLCIdAsync(int exportLcId);
        Task<IEnumerable<COM_IMP_LCINFORMATION>> GetAllAsync();
        Task<ComImpLcInformationForCreateViewModel> FindByLcIdInlcudeAllAsync(int lcId);
        Task<bool> FindFileNoByAsync(ComImpLcInformationForCreateViewModel comImpLcInformationForCreateViewModel);
    }
}
