using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;

namespace DenimERP.ServiceInterfaces.MenuMaster
{
    public interface IMenuMasterRoles : IBaseService<MenuMasterRoles>
    {
        Task<IEnumerable<MenuMasterRoles>> FindByMenuIdentityIdAsync(int menuIdnetityId);
    }
}
