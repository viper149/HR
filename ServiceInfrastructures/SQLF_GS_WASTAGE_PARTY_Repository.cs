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
    public class SQLF_GS_WASTAGE_PARTY_Repository : BaseRepository<F_GS_WASTAGE_PARTY>, IF_GS_WASTAGE_PARTY
    {
        private readonly IDataProtector _protector;

        public SQLF_GS_WASTAGE_PARTY_Repository(DenimDbContext denimDbContext,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings) : base(denimDbContext)
        {
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        public async Task<IEnumerable<F_GS_WASTAGE_PARTY>> GetAllFGsWastagePartyAsync()
        {
            return await DenimDbContext.F_GS_WASTAGE_PARTY
                .Select(d => new F_GS_WASTAGE_PARTY
                {
                    PID = d.PID,
                    EncryptedId = _protector.Protect(d.PID.ToString()),
                    PNAME = d.PNAME,
                    ADDRESS = d.ADDRESS,
                    PHONE = d.PHONE,
                    REMARKS = d.REMARKS

                }).ToListAsync();

        }

        public async Task<F_GS_WASTAGE_PARTY> GetInitObjByAsync(F_GS_WASTAGE_PARTY fGsWastageParty)
        {
            return fGsWastageParty;
        }

        public async Task<bool> FindByProductName(string pName)
        {
            return !await DenimDbContext.F_FS_WASTAGE_PARTY.AnyAsync(d => d.PNAME.Equals(pName));
        }
    }
}
