using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces.MenuMaster;
using Microsoft.EntityFrameworkCore;

namespace DenimERP.ServiceInfrastructures.MenuMaster
{
    public class SQLMenuMasterRoles_Repository : BaseRepository<MenuMasterRoles>, IMenuMasterRoles
    {
        public SQLMenuMasterRoles_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }

        public async Task<IEnumerable<MenuMasterRoles>> FindByMenuIdentityIdAsync(int menuIdnetityId)
        {
            var menuMasterRoleses = await DenimDbContext.MenuMasterRoles
                .Where(e => e.MenuIdentityId.Equals(menuIdnetityId))
                .ToListAsync();

            return menuMasterRoleses;
        }
    }
}
