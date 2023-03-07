﻿using System.Collections.Generic;
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
    public class SQLF_BAS_HRD_SUB_SECTION_Repository : BaseRepository<F_BAS_HRD_SUB_SECTION>, IF_BAS_HRD_SUB_SECTION
    {
        private readonly IDataProtector _protector;

        public SQLF_BAS_HRD_SUB_SECTION_Repository(DenimDbContext denimDbContext,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings) : base(denimDbContext)
        {
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        public async Task<IEnumerable<F_BAS_HRD_SUB_SECTION>> GetAllFBasHrdSubSectionAsync()
        {
            return await DenimDbContext.F_BAS_HRD_SUB_SECTION
                .Select(d => new F_BAS_HRD_SUB_SECTION
                {
                    SUBSECID = d.SUBSECID,
                    EncryptedId = _protector.Protect(d.SUBSECID.ToString()),
                    SUBSEC_NAME = d.SUBSEC_NAME,
                    SUBSEC_NAME_BN = d.SUBSEC_NAME_BN,
                    SUBSEC_SNAME = d.SUBSEC_SNAME,
                    SUBSEC_SNAME_BN = d.SUBSEC_SNAME_BN,
                    DESCRIPTION = d.DESCRIPTION,
                    REMARKS = d.REMARKS
                }).ToListAsync();
        }

        public async Task<bool> FindBySubSecNameAsync(string subSecName, bool isBn = false)
        {
            return !isBn ? !await DenimDbContext.F_BAS_HRD_SUB_SECTION.AnyAsync(d => d.SUBSEC_NAME.Equals(subSecName))
                : !await DenimDbContext.F_BAS_HRD_SUB_SECTION.AnyAsync(d => d.SUBSEC_NAME_BN.Equals(subSecName));
        }

        public async Task<List<F_BAS_HRD_SUB_SECTION>> GetAllSubSectionsAsync()
        {
            return await DenimDbContext.F_BAS_HRD_SUB_SECTION
                .Select(d => new F_BAS_HRD_SUB_SECTION
                {
                    SUBSECID=d.SUBSECID,
                    SUBSEC_NAME=d.SUBSEC_NAME
                }).ToListAsync();
        }
    }
}
