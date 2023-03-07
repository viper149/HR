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
    public class SQLF_FS_WASTAGE_PARTY_Repository : BaseRepository<F_FS_WASTAGE_PARTY>, IF_FS_WASTAGE_PARTY
    {
        private readonly IDataProtector _protector;

        public SQLF_FS_WASTAGE_PARTY_Repository(DenimDbContext denimDbContext,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings) : base(denimDbContext)
        {
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        public async Task<IEnumerable<F_FS_WASTAGE_PARTY>> GetAllFFsWastagePartyAsync()
        {
            return await DenimDbContext.F_FS_WASTAGE_PARTY
                .Select(d => new F_FS_WASTAGE_PARTY
                {
                    EncryptedId = _protector.Protect(d.PID.ToString()),
                    PNAME = d.PNAME,

                    ADDRESS=d.ADDRESS,
                    PHONE=d.PHONE,
                    REMARKS=d.REMARKS

                }).ToListAsync();

        }

        public async Task<F_FS_WASTAGE_PARTY> GetInitObjByAsync(F_FS_WASTAGE_PARTY f_FS_WASTAGE_PARTY)
        {
            


            return f_FS_WASTAGE_PARTY;
        }

        public async Task<bool> FindByProductName(string pName)
        {
            return !await DenimDbContext.F_FS_WASTAGE_PARTY.AnyAsync(d => d.PNAME.Equals(pName));
        }


    }
}


