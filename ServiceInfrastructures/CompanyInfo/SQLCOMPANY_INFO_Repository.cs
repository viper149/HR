using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HRMS.Data;
using HRMS.Models;
using HRMS.Security;
using HRMS.ServiceInfrastructures.BaseInfrastructures;
using HRMS.ServiceInterfaces.CompanyInfo;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;

namespace HRMS.ServiceInfrastructures.CompanyInfo
{
    public class SQLCOMPANY_INFO_Repository : BaseRepository<COMPANY_INFO>, ICOMPANY_INFO
    {
        private readonly IDataProtector _protector;
        public SQLCOMPANY_INFO_Repository(HRDbContext hrDbContext,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings) : base(hrDbContext)
        {
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        public async Task<bool> FindByCompanyName(string companyName)
        {
            return !await HrDbContext.COMPANY_INFO.AnyAsync(d => d.COMPANY_NAME.Equals(companyName));
        }
        
        public async Task<IEnumerable<COMPANY_INFO>> GetAllCompanyInfoAsync()
        {
            return await HrDbContext.COMPANY_INFO
                .Select(d => new COMPANY_INFO
                {
                    ID = d.ID,
                    EncryptedId = _protector.Protect(d.ID.ToString()),
                    LogoBase64 = d.LOGO != null ? "data:image/" + Common.Common.GetFileExtension(Convert.ToBase64String(d.LOGO)) + ";base64," + Convert.ToBase64String(d.LOGO) : null,
                    COMPANY_NAME = d.COMPANY_NAME,
                    //TAGLINE = d.TAGLINE,
                    //FACTORY_ADDRESS = d.FACTORY_ADDRESS,
                    //HEADOFFICE_ADDRESS = d.HEADOFFICE_ADDRESS,
                    //WEB_ADDRESS = d.WEB_ADDRESS,
                    EMAIL = d.EMAIL,
                    PHONE1 = d.PHONE1,
                    //PHONE2 = d.PHONE2,
                    //PHONE3 = d.PHONE3,
                    //BIN_NO = d.BIN_NO,
                    //ETIN_NO = d.ETIN_NO,
                    //DESCRIPTION = d.DESCRIPTION,
                    REMARKS = d.REMARKS
                }).ToListAsync();
        }
    }
}
