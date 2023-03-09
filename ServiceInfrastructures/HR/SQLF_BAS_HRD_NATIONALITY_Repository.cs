using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HRMS.Data;
using HRMS.Models;
using HRMS.Security;
using HRMS.ServiceInfrastructures.BaseInfrastructures;
using HRMS.ServiceInterfaces.HR;
using HRMS.ViewModels.HR;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;

namespace HRMS.ServiceInfrastructures.HR
{
    public class SQLF_BAS_HRD_NATIONALITY_Repository : BaseRepository<F_BAS_HRD_NATIONALITY>, IF_BAS_HRD_NATIONALITY
    {
        private readonly IDataProtector _protector;

        public SQLF_BAS_HRD_NATIONALITY_Repository(HRDbContext hrDbContext,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings) : base(hrDbContext)
        {
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        public async Task<FBasHrdNationalityViewModel> GetInitObjByAsync(FBasHrdNationalityViewModel fBasHrdNationalityViewModel)
        {
            fBasHrdNationalityViewModel.CurrencyList = await HrDbContext.CURRENCY
                .Select(d => new CURRENCY
                {
                    Id = d.Id,
                    NAME = $"{d.CODE} ({d.NAME})"
                }).ToListAsync();

            return fBasHrdNationalityViewModel;
        }

        public async Task<IEnumerable<F_BAS_HRD_NATIONALITY>> GetAllFBasHrdNationalityAsync()
        {
            return await HrDbContext.F_BAS_HRD_NATIONALITY
                .Include(d => d.CUR)
                .Select(d => new F_BAS_HRD_NATIONALITY
                {
                    NATIONID = d.NATIONID,
                    EncryptedId = _protector.Protect(d.NATIONID.ToString()),
                    NATION_DESC = d.NATION_DESC,
                    NATION_DESC_BN = d.NATION_DESC_BN,
                    COUNTRY_NAME = d.COUNTRY_NAME,
                    COUNTRY_NAME_BN = d.COUNTRY_NAME_BN,
                    SHORT_NAME = d.SHORT_NAME,
                    REMARKS = d.REMARKS,
                    CUR = new CURRENCY
                    {
                        NAME = $"{d.CUR.CODE} ({d.CUR.NAME})"
                    }
                }).ToListAsync();
        }

        public async Task<bool> FindByNationalityAsync(string nationality, bool isBn = false)
        {
            return !isBn ? !await HrDbContext.F_BAS_HRD_NATIONALITY.AnyAsync(d => d.NATION_DESC.Equals(nationality))
                : !await HrDbContext.F_BAS_HRD_NATIONALITY.AnyAsync(d => d.NATION_DESC_BN.Equals(nationality));
        }

        public async Task<bool> FindByCountryAsync(string country, bool isBn = false)
        {
            return !isBn ? !await HrDbContext.F_BAS_HRD_NATIONALITY.AnyAsync(d => d.COUNTRY_NAME.Equals(country))
                : !await HrDbContext.F_BAS_HRD_NATIONALITY.AnyAsync(d => d.COUNTRY_NAME_BN.Equals(country));
        }
    }
}
