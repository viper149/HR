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
    public class SQLF_BAS_HRD_GRADE_Repository : BaseRepository<F_BAS_HRD_GRADE>, IF_BAS_HRD_GRADE
    {
        private readonly IDataProtector _protector;

        public SQLF_BAS_HRD_GRADE_Repository(HRDbContext hrDbContext,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings) : base(hrDbContext)
        {
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        public async Task<IEnumerable<F_BAS_HRD_GRADE>> GetAllFBasHrdGradeAsync()
        {
            return await HrDbContext.F_BAS_HRD_GRADE
                .Select(d => new F_BAS_HRD_GRADE
                {
                    GRADEID = d.GRADEID,
                    EncryptedId = _protector.Protect(d.GRADEID.ToString()),
                    GRADE_NAME = d.GRADE_NAME,
                    DESCRIPTION = d.DESCRIPTION,
                    REMARKS = d.REMARKS
                }).ToListAsync();
        }

        public async Task<bool> FindByGradeNameAsync(string gradeName)
        {
            return !await HrDbContext.F_BAS_HRD_GRADE.AnyAsync(d => d.GRADE_NAME.Equals(gradeName));
        }

        public async Task<List<F_BAS_HRD_GRADE>> GetAllGradesAsync()
        {
            return await HrDbContext.F_BAS_HRD_GRADE
                .Select(d => new F_BAS_HRD_GRADE
                {
                    GRADEID = d.GRADEID,
                    GRADE_NAME = d.GRADE_NAME
                }).ToListAsync();
        }
    }
}
