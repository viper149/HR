using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HRMS.Data;
using HRMS.Models;
using HRMS.Security;
using HRMS.ServiceInfrastructures.BaseInfrastructures;
using HRMS.ServiceInterfaces;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;

namespace HRMS.ServiceInfrastructures
{
    public class SQLBAS_BEN_BANK_MASTER_Repository: BaseRepository<BAS_BEN_BANK_MASTER>, IBAS_BEN_BANK_MASTER
    {
        private readonly IDataProtector _protector;

        public SQLBAS_BEN_BANK_MASTER_Repository(HRDbContext hrDbContext,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings) : base(hrDbContext)
        {
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        public async Task<IEnumerable<BAS_BEN_BANK_MASTER>> GetAllBasBenBankAsync()
        {
            return await HrDbContext.BAS_BEN_BANK_MASTER
                .Select(d => new BAS_BEN_BANK_MASTER
                {
                    BANKID = d.BANKID,
                    EncryptedId = _protector.Protect(d.BANKID.ToString()),
                    BEN_BANK = d.BEN_BANK,
                    ADDRESS = d.ADDRESS,
                    BRANCH = d.BRANCH,
                    REMARKS = d.REMARKS
                }).ToListAsync();
        }

        public async Task<bool> FindByBenBankAsync(string bank)
        {
            return !await HrDbContext.BAS_BEN_BANK_MASTER.AnyAsync(d => d.BEN_BANK.Equals(bank));
        }

        public async Task<List<BAS_BEN_BANK_MASTER>> GetAllBenBanksAsync()
        {
            return await HrDbContext.BAS_BEN_BANK_MASTER
                .Select(d => new BAS_BEN_BANK_MASTER
                {
                    BANKID = d.BANKID,
                    BEN_BANK = d.BEN_BANK
                }).ToListAsync();
        }
    }
}
