using System;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Data;
using DenimERP.Models;
using DenimERP.Security;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;
using DenimERP.ViewModels;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLF_FS_UP_MASTER_Repository : BaseRepository<F_FS_UP_MASTER>, IF_FS_UP_MASTER
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IDataProtector _protector;


        public SQLF_FS_UP_MASTER_Repository(DenimDbContext denimDbContext, UserManager<ApplicationUser> userManager,
            IHttpContextAccessor httpContextAccessor,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings) : base(denimDbContext)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        public async Task<FFsFabricUPViewModel> GetInitObjByAsync(FFsFabricUPViewModel fFsFabricUPViewModel)
        {
            try
            {
                fFsFabricUPViewModel.ComExLCInfoList = await DenimDbContext.COM_EX_LCINFO
                    .Select(x => new COM_EX_LCINFO
                    {
                        LCID = x.LCID,
                        LCNO = x.LCNO,
                    }).ToListAsync();

                return fFsFabricUPViewModel;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
