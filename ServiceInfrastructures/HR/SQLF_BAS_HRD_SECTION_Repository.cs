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
    public class SQLF_BAS_HRD_SECTION_Repository : BaseRepository<F_BAS_HRD_SECTION>, IF_BAS_HRD_SECTION
    {
        private readonly IDataProtector _protector;

        public SQLF_BAS_HRD_SECTION_Repository(DenimDbContext denimDbContext,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings) : base(denimDbContext)
        {
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        public async Task<IEnumerable<F_BAS_HRD_SECTION>> GetAllFBasHrdSectionAsync()
        {
            return await DenimDbContext.F_BAS_HRD_SECTION
                .Select(d => new F_BAS_HRD_SECTION
                {
                    SECID = d.SECID,
                    EncryptedId = _protector.Protect(d.SECID.ToString()),
                    SEC_NAME = d.SEC_NAME,
                    SEC_NAME_BN = d.SEC_NAME_BN,
                    SHORT_NAME = d.SHORT_NAME,
                    SHORT_NAME_BN = d.SHORT_NAME_BN,
                    DESCRIPTION = d.DESCRIPTION,
                    REMARKS = d.REMARKS
                }).ToListAsync();
        }

        public async Task<bool> FindBySecNameAsync(string deptName, bool isBn)
        {
            return !isBn ? !await DenimDbContext.F_BAS_HRD_SECTION.AnyAsync(d => d.SEC_NAME.Equals(deptName))
                : !await DenimDbContext.F_BAS_HRD_SECTION.AnyAsync(d => d.SEC_NAME_BN.Equals(deptName));
        }

        public async Task<List<F_BAS_HRD_SECTION>> GetAllSectionsAsync()
        {
            return await DenimDbContext.F_BAS_HRD_SECTION
                .Select(d => new F_BAS_HRD_SECTION
                {
                    SECID=d.SECID,
                    SEC_NAME=d.SEC_NAME
                }).ToListAsync();
        }
    }
}
