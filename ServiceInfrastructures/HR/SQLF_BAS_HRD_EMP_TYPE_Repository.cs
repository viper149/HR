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
    public class SQLF_BAS_HRD_EMP_TYPE_Repository : BaseRepository<F_BAS_HRD_EMP_TYPE>, IF_BAS_HRD_EMP_TYPE
    {
        private readonly IDataProtector _protector;

        public SQLF_BAS_HRD_EMP_TYPE_Repository(HRDbContext hrDbContext,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings) : base(hrDbContext)
        {
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        public async Task<bool> FindByEmpTypeAsync(string typeName)
        {
            return !await HrDbContext.F_BAS_HRD_EMP_TYPE.AnyAsync(d => d.TYPE_NAME.Equals(typeName));
        }

        public async Task<IEnumerable<F_BAS_HRD_EMP_TYPE>> GetAllFBasHrdEmpTypeAsync()
        {
            return await HrDbContext.F_BAS_HRD_EMP_TYPE
                .Select(d => new F_BAS_HRD_EMP_TYPE
                {
                    TYPEID = d.TYPEID,
                    EncryptedId = _protector.Protect(d.TYPEID.ToString()),
                    TYPE_NAME = d.TYPE_NAME,
                    REMARKS = d.REMARKS
                }).ToListAsync();
        }

        public async Task<List<F_BAS_HRD_EMP_TYPE>> GetAllEmpTypesAsync()
        {
            return await HrDbContext.F_BAS_HRD_EMP_TYPE
                .Select(d => new F_BAS_HRD_EMP_TYPE
                {
                    TYPEID = d.TYPEID,
                    TYPE_NAME = d.TYPE_NAME
                }).ToListAsync();
        }
    }
}
