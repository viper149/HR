using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels;
using DenimERP.ViewModels.MenuMaster;

namespace DenimERP.ServiceInterfaces.MenuMaster
{
    public interface IMenuMaster : IBaseService<Models.MenuMaster>
    {
        IEnumerable<Models.MenuMaster> GetMenuMaster();
        Task<IEnumerable<Models.MenuMaster>> GetMenuMaster(IList<string> userRoles);
        Task<MenuMasterViewModel> GetInitObjects(MenuMasterViewModel menuMasterViewModel);
        Task<bool> IsMenuIdAlreadyExistByAsync(string menuId);
        Task<bool> IsMenuNameAlreadyExistByAsync(string menuName);
        Task<DataTableObject<Models.MenuMaster>> GetForDataTableByAsync(string sortColumn, string sortColumnDirection, string searchValue, string draw, int skip, int pageSize);
        Task<ExtendMenuMasterViewModel> FindByMenuIdentityIdAsync(int menuIdentityId);
        Task<COMPANY_INFO> GetCompayInfo();
        Task<COMPANY_INFO> GetCompayInfo(int BID);
    }
}
