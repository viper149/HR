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
    public class SQLBAS_SEASON_Repository : BaseRepository<BAS_SEASON>,
        IBAS_SEASON
    {

        private readonly IDataProtector _protector;

        public SQLBAS_SEASON_Repository(DenimDbContext denimDbContext,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings) : base(denimDbContext)
        {
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }



        public async Task<IEnumerable<BAS_SEASON>> GetAllBasSeason()
        {
            return await DenimDbContext.BAS_SEASON

                .Select(d => new BAS_SEASON()
                {
                    SID = d.SID,
                    EncryptedId = _protector.Protect(d.SID.ToString()),
                    SNAME = d.SNAME,
                    DESCRIPTION = d.DESCRIPTION,

                }).ToListAsync();
        }

    }
}
