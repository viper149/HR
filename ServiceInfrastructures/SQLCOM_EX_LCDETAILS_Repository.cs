using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;
using Microsoft.EntityFrameworkCore;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLCOM_EX_LCDETAILS_Repository : BaseRepository<COM_EX_LCDETAILS>, ICOM_EX_LCDETAILS
    {
        public SQLCOM_EX_LCDETAILS_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }

        public async Task<COM_EX_LCDETAILS> FindByLcNoAndPiIdIsDeleteAsync(int piId, int lcId)
        {
            try
            {
                return await DenimDbContext.COM_EX_LCDETAILS
                    .FirstOrDefaultAsync(e => e.PIID.Equals(piId) && e.LCID.Equals(lcId) && e.ISDELETE.Equals(true));
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<bool> IsAdvisingBankMatched(int? piId, int? bankId)
        {
            var result = await DenimDbContext.COM_EX_PIMASTER
                .Include(e => e.BANK)
                .FirstOrDefaultAsync(e => e.PIID.Equals(piId));

            return result.BANK != null && result.BANK.BANKID.Equals(bankId) ? true : false;
        }

        public async Task<IEnumerable<COM_EX_LCDETAILS>> FindByLcIdIsDeleteAsync(int lcid)
        {
            return await DenimDbContext.COM_EX_LCDETAILS
                    .Include(e => e.LC.COM_EX_LCDETAILS)
                    .ThenInclude(e => e.BANK)
                    .Include(e => e.LC.COM_EX_LCDETAILS)
                    .ThenInclude(e => e.BANK_)
                    .Where(ld => ld.LCID.Equals(lcid) && ld.ISDELETE.Equals(false))
                    .ToListAsync();
        }
    }
}
