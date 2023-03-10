using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HRMS.Data;
using HRMS.Models;
using HRMS.Security;
using HRMS.ServiceInfrastructures.BaseInfrastructures;
using HRMS.ServiceInterfaces.HR;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;

namespace HRMS.ServiceInfrastructures.HR
{
    public class SQLF_BAS_HRD_OUT_REASON_Repository : BaseRepository<F_BAS_HRD_OUT_REASON>, IF_BAS_HRD_OUT_REASON
    {
        private readonly IDataProtector _protector;

        public SQLF_BAS_HRD_OUT_REASON_Repository(HRDbContext hrDbContext,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings) : base(hrDbContext)
        {
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        public async Task<bool> FindByOutReasonAsync(string resason)
        {
            return !await HrDbContext.F_BAS_HRD_OUT_REASON.AnyAsync(d => d.RESASON_NAME.Equals(resason));
        }

        public async Task<IEnumerable<F_BAS_HRD_OUT_REASON>> GetAllFBasHrdOutReasonAsync()
        {
            return await HrDbContext.F_BAS_HRD_OUT_REASON
                .Select(d => new F_BAS_HRD_OUT_REASON
                {
                    RESASON_ID = d.RESASON_ID,
                    EncryptedId = _protector.Protect(d.RESASON_ID.ToString()),
                    RESASON_NAME = d.RESASON_NAME,
                    REMARKS = d.REMARKS
                }).ToListAsync();
        }
    }
}
