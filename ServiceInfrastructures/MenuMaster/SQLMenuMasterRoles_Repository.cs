using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HRMS.Data;
using HRMS.Models;
using HRMS.ServiceInfrastructures.BaseInfrastructures;
using HRMS.ServiceInterfaces.MenuMaster;
using Microsoft.EntityFrameworkCore;

namespace HRMS.ServiceInfrastructures.MenuMaster
{
    public class SQLMenuMasterRoles_Repository : BaseRepository<MenuMasterRoles>, IMenuMasterRoles
    {
        public SQLMenuMasterRoles_Repository(HRDbContext hrDbContext) : base(hrDbContext)
        {
        }

        public async Task<IEnumerable<MenuMasterRoles>> FindByMenuIdentityIdAsync(int menuIdnetityId)
        {
            var menuMasterRoleses = await HrDbContext.MenuMasterRoles
                .Where(e => e.MenuIdentityId.Equals(menuIdnetityId))
                .ToListAsync();

            return menuMasterRoleses;
        }
    }
}
