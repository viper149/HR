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
    public class SQLF_GS_ITEMCATEGORY_Repository : BaseRepository<F_GS_ITEMCATEGORY>, IF_GS_ITEMCATEGORY
    {
        private readonly IDataProtector _protector;

        public SQLF_GS_ITEMCATEGORY_Repository(DenimDbContext denimDbContext, 
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings) : base(denimDbContext)
        {
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        public async Task<IEnumerable<F_GS_ITEMCATEGORY>> GetAllFGsItemCategoryAsync()
        {
            return await DenimDbContext.F_GS_ITEMCATEGORY
                .Select(d => new F_GS_ITEMCATEGORY
                {
                    CATID = d.CATID,
                    EncryptedId = _protector.Protect(d.CATID.ToString()),
                    CATNAME = d.CATNAME,
                    DESCRIPTION = d.DESCRIPTION,
                    REMARKS = d.REMARKS
                }).ToListAsync();
        }

        public async Task<bool> FindByCatName(string catName)
        {
            return !await DenimDbContext.F_GS_ITEMCATEGORY.AnyAsync(d => d.CATNAME.Equals(catName));
        }
    }
}
