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
    public class SQLF_YS_PARTY_INFO_Repository : BaseRepository<F_YS_PARTY_INFO>, IF_YS_PARTY_INFO
    {
        private readonly IDataProtector _protector;

        public SQLF_YS_PARTY_INFO_Repository(DenimDbContext denimDbContext,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings) : base(denimDbContext)
        {
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        public async Task<IEnumerable<F_YS_PARTY_INFO>> GetAllFYsPartyInfoAsync()
        {
            return await DenimDbContext.F_YS_PARTY_INFO
                .Select(d => new F_YS_PARTY_INFO
                {
                    EncryptedId = _protector.Protect(d.PARTY_ID.ToString()),
                    PARTY_NAME = d.PARTY_NAME,
                    CONTRACT_PERSON=d.CONTRACT_PERSON,
                    ADDRESS = d.ADDRESS,
                    CELL_NO = d.CELL_NO,
                    REMARKS = d.REMARKS

                }).ToListAsync();

        }

        public async Task<F_YS_PARTY_INFO> GetInitObjByAsync(F_YS_PARTY_INFO f_YS_PARTY_INFO)
        {
            return f_YS_PARTY_INFO;
        }
    }
}
