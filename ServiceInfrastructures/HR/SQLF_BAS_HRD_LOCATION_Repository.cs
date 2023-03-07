using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Data;
using DenimERP.Models;
using DenimERP.Security;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces.HR;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;

namespace DenimERP.ServiceInfrastructures.HR
{
    public class SQLF_BAS_HRD_LOCATION_Repository : BaseRepository<F_BAS_HRD_LOCATION>, IF_BAS_HRD_LOCATION
    {
        private readonly IDataProtector _protector;

        public SQLF_BAS_HRD_LOCATION_Repository(DenimDbContext denimDbContext,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings) : base(denimDbContext)
        {
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        public async Task<IEnumerable<F_BAS_HRD_LOCATION>> GetAllFBasHrdLocationAsync()
        {
            return await DenimDbContext.F_BAS_HRD_LOCATION
                .Select(d => new F_BAS_HRD_LOCATION
                {
                    LOCID = d.LOCID,
                    EncryptedId = _protector.Protect(d.LOCID.ToString()),
                    LOC_NAME = d.LOC_NAME,
                    DESCRIPTION = d.DESCRIPTION,
                    REMARKS = d.REMARKS
                }).ToListAsync();
        }

        public async Task<bool> FindByLocNameAsync(string locName)
        {
            return !await DenimDbContext.F_BAS_HRD_LOCATION.AnyAsync(d => d.LOC_NAME.Equals(locName));
        }

        public async Task<List<F_BAS_HRD_LOCATION>> GetAllLocationsAsync()
        {
            return await DenimDbContext.F_BAS_HRD_LOCATION
                .Select(d => new F_BAS_HRD_LOCATION
                {
                    LOCID = d.LOCID,
                    LOC_NAME = d.LOC_NAME
                }).ToListAsync();
        }
    }
}
