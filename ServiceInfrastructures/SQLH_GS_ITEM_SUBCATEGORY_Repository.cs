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
    public class SQLH_GS_ITEM_SUBCATEGORY_Repository : BaseRepository<H_GS_ITEM_SUBCATEGORY>, IH_GS_ITEM_SUBCATEGORY
    {
        private readonly IDataProtector _protector;

        public SQLH_GS_ITEM_SUBCATEGORY_Repository(DenimDbContext denimDbContext,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings) : base(denimDbContext)
        {
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        public async Task<List<H_GS_ITEM_SUBCATEGORY>> GetAllHGsItemSubCategoryAsync()
        {
            return await DenimDbContext.H_GS_ITEM_SUBCATEGORY
                .Include(d => d.CAT)
                .Select(d => new H_GS_ITEM_SUBCATEGORY
                {
                    SCATID = d.SCATID,
                    EncryptedId = _protector.Protect(d.SCATID.ToString()),
                    SCATNAME = d.SCATNAME,
                    DESCRIPTION = d.DESCRIPTION,
                    REMARKS = d.REMARKS,
                    CAT = new H_GS_ITEM_CATEGORY
                    {
                        CATNAME = $"{d.CAT.CATNAME}"
                    }
                }).ToListAsync();
        }
        public async Task<bool> FindBySCatName(string scatName)
        {
            return !await DenimDbContext.H_GS_ITEM_SUBCATEGORY.AnyAsync(d => d.SCATNAME.Equals(scatName));
        }
    }

}
