using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HRMS.Data;
using HRMS.Models;
using HRMS.Security;
using HRMS.ServiceInfrastructures.BaseInfrastructures;
using HRMS.ServiceInterfaces.MenuMaster;
using HRMS.ViewModels;
using HRMS.ViewModels.MenuMaster;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;

namespace HRMS.ServiceInfrastructures.MenuMaster
{
    public class SQLMenuMaster_Repository : BaseRepository<Models.MenuMaster>, IMenuMaster
    {
        private readonly IDataProtector _protector;

        public SQLMenuMaster_Repository(HRDbContext hrDbContext,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings) : base(hrDbContext)
        {
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        public IEnumerable<Models.MenuMaster> GetMenuMaster()
        {
            return HrDbContext.MenuMaster.AsEnumerable();

        }

        public async Task<IEnumerable<Models.MenuMaster>> GetMenuMaster(IList<string> userRoles)
        {
            var result = await HrDbContext.MenuMaster
                .Where(m => HrDbContext.MenuMasterRoles
                    .Where(e => HrDbContext.Roles
                        .Where(f => userRoles.Contains(f.Name))
                        .Select(f => f.Id)
                        .Contains(e.RoleId))
                    .Select(e => e.MenuIdentityId).Distinct()
                    .Contains(m.MenuIdentity))
                .OrderBy(e => e.MenuName)
                .ThenBy(e => e.Priority)
                .ToListAsync();

            return result;
        }

        public async Task<MenuMasterViewModel> GetInitObjects(MenuMasterViewModel menuMasterViewModel)
        {
            menuMasterViewModel.MenuMasters = await HrDbContext.MenuMaster
                .Select(e => new Models.MenuMaster { MenuID = e.MenuID })
                .OrderBy(e => e.MenuID)
                .ToListAsync();

            menuMasterViewModel.MenuMasters.Insert(0, new Models.MenuMaster
            {
                MenuID = "*"
            });

            return menuMasterViewModel;
        }

        public async Task<bool> IsMenuIdAlreadyExistByAsync(string menuId)
        {
            return await HrDbContext.MenuMaster.Where(e => e.MenuID.Equals(menuId)).AnyAsync();
        }
        public async Task<bool> IsMenuNameAlreadyExistByAsync(string menuName)
        {
            return await HrDbContext.MenuMaster.Where(e => e.MenuName.Equals(menuName)).AnyAsync();
        }

        public async Task<DataTableObject<Models.MenuMaster>> GetForDataTableByAsync(string sortColumn, string sortColumnDirection, string searchValue, string draw, int skip,
            int pageSize)
        {
            var navigationPropertyStrings = new[] { "" };
            var menuMasters = HrDbContext.MenuMaster
                .Select(e => new Models.MenuMaster
                {
                    MenuIdentity = e.MenuIdentity,
                    EncryptedId = _protector.Protect(e.MenuIdentity.ToString()),
                    MenuID = e.MenuID,
                    MenuName = e.MenuName,
                    Parent_MenuID = e.Parent_MenuID,
                    MenuFileName = e.MenuFileName,
                    MenuURL = e.MenuURL,
                    USE_YN = e.USE_YN,
                    ParentMenuIcon = e.ParentMenuIcon,
                    CreatedDate = e.CreatedDate,
                    Priority = e.Priority
                });

            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
            {
                menuMasters = OrderedResult<Models.MenuMaster>.GetOrderedResult(sortColumnDirection, sortColumn, navigationPropertyStrings, menuMasters);
            }

            if (!string.IsNullOrEmpty(searchValue))
            {
                menuMasters = menuMasters
                    .Where(m => m.MenuID.ToUpper().Contains(searchValue)
                                || m.MenuName != null && m.MenuName.ToUpper().Contains(searchValue)
                                || m.Parent_MenuID != null && m.Parent_MenuID.ToUpper().Contains(searchValue)
                                || m.MenuFileName != null && m.MenuFileName.ToUpper().Contains(searchValue)
                                || m.MenuURL != null && m.MenuURL.ToUpper().Contains(searchValue)
                                || m.USE_YN.ToString().ToUpper().Contains(searchValue));

                menuMasters = OrderedResult<Models.MenuMaster>.GetOrderedResult(sortColumnDirection, sortColumn, navigationPropertyStrings, menuMasters);
            }

            var recordsTotal = await menuMasters.CountAsync();

            return new DataTableObject<Models.MenuMaster>(draw, recordsTotal, recordsTotal, await menuMasters.Skip(skip).Take(pageSize).ToListAsync());
        }

        public async Task<ExtendMenuMasterViewModel> FindByMenuIdentityIdAsync(int menuIdentityId)
        {
            try
            {
                return await HrDbContext.MenuMaster
                    .Select(e => new ExtendMenuMasterViewModel
                    {
                        MenuMaster = new Models.MenuMaster
                        {
                            MenuIdentity = e.MenuIdentity,
                            MenuID = e.MenuID,
                            EncryptedId = _protector.Protect(e.MenuIdentity.ToString()),
                            MenuName = e.MenuName,
                            Parent_MenuID = e.Parent_MenuID,
                            MenuFileName = e.MenuFileName,
                            MenuURL = e.MenuURL,
                            USE_YN = e.USE_YN,
                            ParentMenuIcon = e.ParentMenuIcon,
                            CreatedDate = e.CreatedDate,
                            Priority = e.Priority
                        },

                        MenuID = e.MenuID,
                        MenuName = e.MenuName
                    })
                    .FirstOrDefaultAsync(e => e.MenuMaster.MenuIdentity.Equals(menuIdentityId));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<COMPANY_INFO> GetCompayInfo()
        {
            return await HrDbContext.COMPANY_INFO.FirstOrDefaultAsync();
        }

        public async Task<COMPANY_INFO> GetCompayInfo(int BID)
        {
            try
            {
                var companyInfo= await HrDbContext.COMPANY_INFO.FirstOrDefaultAsync(c => c.ID==BID);
                return companyInfo;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }
    }
}
