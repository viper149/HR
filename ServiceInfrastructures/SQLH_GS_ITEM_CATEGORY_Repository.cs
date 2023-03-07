using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Data;
using DenimERP.Models;
using DenimERP.Security;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLH_GS_ITEM_CATEGORY_Repository : BaseRepository<H_GS_ITEM_CATEGORY>, IH_GS_ITEM_CATEGORY
    {
        private readonly IDataProtector _protector;

        public SQLH_GS_ITEM_CATEGORY_Repository(DenimDbContext denimDbContext,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings) : base(denimDbContext)
        {
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        async Task<bool> IH_GS_ITEM_CATEGORY.FindByCatName(string catName)
        {
            return !await DenimDbContext.H_GS_ITEM_CATEGORY.AnyAsync(d => d.CATNAME.Equals(catName));
        }

        async Task<List<H_GS_ITEM_CATEGORY>> IH_GS_ITEM_CATEGORY.GetAllHGsItemCategory()
        {
            return await DenimDbContext.H_GS_ITEM_CATEGORY
                .Select(d => new H_GS_ITEM_CATEGORY
                {
                    CATID = d.CATID,
                    EncryptedId = _protector.Protect(d.CATID.ToString()),
                    CATNAME = d.CATNAME,
                    DESCRIPTION = d.DESCRIPTION,
                    REMARKS = d.REMARKS
                }).ToListAsync();
        }
    }
}
