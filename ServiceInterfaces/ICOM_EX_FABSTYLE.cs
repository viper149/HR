using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels;

namespace DenimERP.ServiceInterfaces
{
    public interface ICOM_EX_FABSTYLE : IBaseService<COM_EX_FABSTYLE>
    {
        Task<DataTableObject<COM_EX_FABSTYLE>> GetForDataTableByAsync(string sortColumn, string sortColumnDirection, string searchValue, string draw, int skip, int pageSize);
        Task<bool> DeleteFabStyle(int id);
        Task<COM_EX_FABSTYLE> GetComExFabricInfoAsync(int styleId);
        Task<COM_EX_PI_DETAILS> GetComExFabricInfoAsync(int trnsId, int lcId);
        Task<IEnumerable<COM_EX_PIMASTER>> FindPIListByStyleIdAsync(int styleId); 
        Task<COM_EX_FABSTYLE> FindByIdForStyleNameAsync(int styleId);
        Task<ComExFabStyleViewModel> GetFabricInfoAsync(int fabCode);
        Task<ComExFabStyleViewModel> GetInitObjects(ComExFabStyleViewModel comExFabStyleViewModel);
        Task<double> GetGetInvBalanceAsync(int trnsId, int lcId);
    }
}
