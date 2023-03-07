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
    public class SQLF_HRD_EMP_EDU_DEGREE_Repository : BaseRepository<F_HRD_EMP_EDU_DEGREE>, IF_HRD_EMP_EDU_DEGREE
    {
        private readonly IDataProtector _protector;

        public SQLF_HRD_EMP_EDU_DEGREE_Repository(DenimDbContext denimDbContext,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings) : base(denimDbContext)
        {
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        public async Task<IEnumerable<F_HRD_EMP_EDU_DEGREE>> GetAllFHrdEmpEduDegreeAsync()
        {
            return await DenimDbContext.F_HRD_EMP_EDU_DEGREE
                .Select(d => new F_HRD_EMP_EDU_DEGREE
                {
                    DEGID = d.DEGID,
                    EncryptedId = _protector.Protect(d.DEGID.ToString()),
                    DEGNAME = d.DEGNAME,
                    DESCRIPTION = d.DESCRIPTION
                }).ToListAsync();
        }

        public async Task<bool> FindByDegreeAsync(string degree)
        {
            return !await DenimDbContext.F_HRD_EMP_EDU_DEGREE.AnyAsync(d => d.DEGNAME.Equals(degree));
        }

        public async Task<List<F_HRD_EMP_EDU_DEGREE>> GetAllEduDegreesAsync()
        {
            return await DenimDbContext.F_HRD_EMP_EDU_DEGREE
                .Select(d => new F_HRD_EMP_EDU_DEGREE
                {
                    DEGID = d.DEGID,
                    DEGNAME = d.DEGNAME
                }).ToListAsync();
        }
    }
}
