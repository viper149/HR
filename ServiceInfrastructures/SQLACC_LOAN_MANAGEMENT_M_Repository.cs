using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Data;
using DenimERP.Models;
using DenimERP.Security;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLACC_LOAN_MANAGEMENT_M_Repository : BaseRepository<ACC_LOAN_MANAGEMENT_M>, IACC_LOAN_MANAGEMENT_M
    {
        private readonly IDataProtector _protector;

        public SQLACC_LOAN_MANAGEMENT_M_Repository(DenimDbContext denimDbContext,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings) : base(denimDbContext)
        {
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        public async Task<IEnumerable<ACC_LOAN_MANAGEMENT_M>> GetForDataTableByAsync()
        {
            return await DenimDbContext.ACC_LOAN_MANAGEMENT_M
                .Include(e => e.BANK)
                .Include(e => e.LC)
                .Select(e => new ACC_LOAN_MANAGEMENT_M
                {
                    EncryptedId = _protector.Protect(e.LOANID.ToString()),
                    LOANDATE = e.LOANDATE,
                    LOAN_AMT = e.LOAN_AMT,
                    EXP_DATE = e.EXP_DATE,
                    INTEREST_RATE = e.INTEREST_RATE,
                    PAID_AMT = e.PAID_AMT,
                    PAID_DATE = e.PAID_DATE,
                    BANK = new BAS_BEN_BANK_MASTER
                    {
                        BEN_BANK = $"{e.BANK.BEN_BANK}"
                    },
                    LC = new COM_IMP_LCINFORMATION
                    {
                        LCNO = $"{e.LC.LCNO}"
                    },
                    REMARKS = e.REMARKS
                }).ToListAsync();
        }
    }
}
