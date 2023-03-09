using System.Collections.Generic;
using System.Threading.Tasks;
using HRMS.Models;
using HRMS.ServiceInterfaces.BaseInterfaces;

namespace HRMS.ServiceInterfaces.MenuMaster
{
    public interface IMenuMasterRoles : IBaseService<MenuMasterRoles>
    {
        Task<IEnumerable<MenuMasterRoles>> FindByMenuIdentityIdAsync(int menuIdnetityId);
    }
}
