using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels;

namespace DenimERP.ServiceInterfaces
{
    public interface ICOM_EX_PI_DETAILS : IBaseService<COM_EX_PI_DETAILS>
    {
        Task<double?> GetTotalSumByPiNoAsync(int piId); 
         Task<IEnumerable<COM_EX_PI_DETAILS>> FindPIListByPINoAsync(int piId);
        Task<IEnumerable<COM_EX_PI_DETAILS>> FindPIListByPINoAndTransIDAsync(int piId, int trnsId);
        Task<COM_EX_PI_DETAILS> FindSoInPOTableAsync(int trnsId);
        Task<COM_EX_PI_DETAILS> FindSoDetailsAsync(int trnsId);
        Task<IEnumerable<COM_EX_PI_DETAILS>> FindPiListByPiIdAndStyleIdAsync(int piId, int styleId);
        Task<string> GetLastSoNoAsync();
        Task<IEnumerable<TypeTableViewModel>> GetSoList();
        Task<IEnumerable<TypeTableViewModel>> GetSoListWithProductionOrder();
        Task<IEnumerable<COM_EX_PI_DETAILS>> GetStyleByPiAsync(int piId);
        Task<string> GetUnitByPiAsync(int id);

        Task<F_YS_INDENT_DETAILS> GetYarnDetailsbyId(int transId);
        Task<int> GetPoIdBySO(int id);
    }
}
