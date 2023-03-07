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
    public class SQLF_FS_UP_DETAILS_Repository : BaseRepository<F_FS_UP_DETAILS>, IF_FS_UP_DETAILS
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IDataProtector _protector;


        public SQLF_FS_UP_DETAILS_Repository(DenimDbContext denimDbContext,
            UserManager<ApplicationUser> userManager,
            IHttpContextAccessor httpContextAccessor,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings) : base(denimDbContext)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

       
        public async Task<COM_EX_LCINFO> GetLcInfo(int lcId)
        {
            try
            {
                var result = await DenimDbContext.COM_EX_LCINFO
                    .Include(x=>x.BUYER)
                    .Where(x=>x.LCID.Equals(lcId))
                    .Select(x => new COM_EX_LCINFO
                    {
                        EX_DATE = x.EX_DATE,
                        VALUE = x.VALUE,
                        BUYER = new BAS_BUYERINFO()
                        {
                            BUYER_NAME = x.BUYER.BUYER_NAME,
                        }

                    }).FirstOrDefaultAsync();

                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<FFsFabricUPViewModel> GetInitObjectsOfSelectedItems(FFsFabricUPViewModel fFsFabricUPViewModel)
        {
            try
            {
                foreach (var item in fFsFabricUPViewModel.FFsFabricDetailsList)
                {
                    item.ComExLcInfo = await DenimDbContext.COM_EX_LCINFO
                        .Include(x=>x.BUYER)
                        .Where(x => x.LCID.Equals(item.LC_ID))
                        .Select(x => new COM_EX_LCINFO
                        {
                            LCID = x.LCID,
                            LCNO = x.LCNO,
                            EX_DATE = x.EX_DATE,
                            VALUE = x.VALUE,
                            BUYER = new BAS_BUYERINFO
                            {
                                BUYER_NAME = x.BUYER.BUYER_NAME
                            }
                        }).FirstOrDefaultAsync();
                }

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
