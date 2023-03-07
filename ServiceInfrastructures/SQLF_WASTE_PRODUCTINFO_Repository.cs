using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Data;
using DenimERP.Models;
using DenimERP.Security;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;
using DenimERP.ViewModels;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLF_WASTE_PRODUCTINFO_Repository : BaseRepository<F_WASTE_PRODUCTINFO>,
        IF_WASTE_PRODUCTINFO
    {
         
        private readonly IDataProtector _protector;

        public SQLF_WASTE_PRODUCTINFO_Repository(DenimDbContext denimDbContext,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings) : base(denimDbContext)
        {
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        public async Task<IEnumerable<F_WASTE_PRODUCTINFO>> GetAllWasteProductInfoAsync()
        {
            return await DenimDbContext.F_WASTE_PRODUCTINFO
                .Include(e => e.WP)
                .Select(d => new F_WASTE_PRODUCTINFO()
                {
                    WPID = d.WPID,
                    EncryptedId = _protector.Protect(d.WPID.ToString()),
                    PRODUCT_NAME=d.PRODUCT_NAME,
                    WPTYPE= d.WPTYPE,
                    REMARKS = d.REMARKS,
                    CREATED_BY = d.CREATED_BY,
                    CREATED_AT = d.CREATED_AT,
                    UPDATED_BY = d.UPDATED_BY,
                    WP = new F_BAS_UNITS()
                    {
                        UNAME=d.WP.UNAME
                    }

                }).ToListAsync();
        }


        public async Task<FWasteProductInfoViewModel> GetInitObjByAsync(
           FWasteProductInfoViewModel fWasteProductInfoViewModel)
        {
            fWasteProductInfoViewModel.FBasUnitList = await DenimDbContext.F_BAS_UNITS
                .Select(d => new F_BAS_UNITS
                {
                    UID = d.UID,
                    UNAME=d.UNAME,
                }).ToListAsync();

            return fWasteProductInfoViewModel;
        }


        public async Task<bool> FindByProductName(string pName)
        {
            return !await DenimDbContext.F_WASTE_PRODUCTINFO.AnyAsync(d => d.PRODUCT_NAME.Equals(pName));
        }


    }
}
