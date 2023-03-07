using System;
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
    public class SQLF_BAS_HRD_SHIFT_Repository : BaseRepository<F_BAS_HRD_SHIFT>, IF_BAS_HRD_SHIFT
    {
        private readonly IDataProtector _protector;

        public SQLF_BAS_HRD_SHIFT_Repository(DenimDbContext denimDbContext,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings) : base(denimDbContext)
        {
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        public async Task<bool> FindByShiftAsync(string shift)
        {
            return !await DenimDbContext.F_BAS_HRD_SHIFT.AnyAsync(d => d.SHIFT_NAME.Equals(shift));
        }

        public async Task<IEnumerable<F_BAS_HRD_SHIFT>> GetAllFBasHrdShiftAsync()
        {
            return await DenimDbContext.F_BAS_HRD_SHIFT
                .Select(d => new F_BAS_HRD_SHIFT
                {
                    SHIFTID = d.SHIFTID,
                    EncryptedId = _protector.Protect(d.SHIFTID.ToString()),
                    SHIFT_NAME = d.SHIFT_NAME,
                    SHORT_NAME = d.SHORT_NAME,
                    TimeStart = $"{Convert.ToDateTime((d.TIME_START ?? default).ToString()):hh: mm tt}",
                    TimeEnd = $"{Convert.ToDateTime((d.TIME_END ?? default).ToString()):hh: mm tt}",
                    REMARKS = d.REMARKS
                }).ToListAsync();
        }
    }
}
