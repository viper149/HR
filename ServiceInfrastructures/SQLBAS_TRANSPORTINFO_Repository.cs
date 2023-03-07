using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Data;
using DenimERP.Models;
using DenimERP.Security;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;
using DenimERP.ViewModels.Basic;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLBAS_TRANSPORTINFO_Repository : BaseRepository<BAS_TRANSPORTINFO>, IBAS_TRANSPORTINFO
    {
        private readonly IDataProtector _protector;

        public SQLBAS_TRANSPORTINFO_Repository(DenimDbContext denimDbContext,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings) : base(denimDbContext)
        {
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        public async Task<IEnumerable<BAS_TRANSPORTINFO>> GetAllForDataTables()
        {
            return await DenimDbContext.BAS_TRANSPORTINFO
                .Select(e => new BAS_TRANSPORTINFO
                {
                    EncryptedId = _protector.Protect(e.TRNSPID.ToString()),
                    TRNSPNAME = e.TRNSPNAME,
                    ADDRESS = e.ADDRESS,
                    CPERSON = e.CPERSON,
                    REMARKS = e.REMARKS
                }).ToListAsync();
        }

        public async Task<BasTransportInfoViewModel> FindByIdIncludeAllAsync(int trnspId)
        {
            return await DenimDbContext.BAS_TRANSPORTINFO.Select(e => new BasTransportInfoViewModel
            {
                BasTransportinfo = new BAS_TRANSPORTINFO
                {
                    TRNSPID = e.TRNSPID,
                    EncryptedId = _protector.Protect(e.TRNSPID.ToString()),
                    TRNSPNAME = e.TRNSPNAME,
                    ADDRESS = e.ADDRESS,
                    PHONE = e.PHONE,
                    EMAIL = e.EMAIL,
                    CPERSON = e.CPERSON,
                    REMARKS = e.REMARKS
                }
            }).FirstOrDefaultAsync(e => e.BasTransportinfo.TRNSPID.Equals(trnspId));
        }
    }
}
