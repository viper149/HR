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
    public class SQLBAS_BUYER_BANK_MASTER_Repository : BaseRepository<BAS_BUYER_BANK_MASTER>, IBAS_BUYER_BANK_MASTER
    {
        public SQLBAS_BUYER_BANK_MASTER_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }
               
        public bool FindByBuyerBankName(string bankName)
        {
            try
            {
                var bankNames = DenimDbContext.BAS_BUYER_BANK_MASTER.Where(sc => sc.PARTY_BANK.Equals(bankName)).Select(e => e.PARTY_BANK);

                if (bankNames.Any())
                    return false;
                else
                    return true;
            }
            catch (Exception ex)
            {
                throw new System.InvalidOperationException(ex.Message);
            }
        }

        public async Task<IEnumerable<BAS_BUYER_BANK_MASTER>> GetBasBuyerBanksWithPaged(int pageNumber = 1, int pageSize = 5)
        {
            try
            {
                int ExcludeResult = (pageSize * pageNumber) - pageSize;

                var result = await DenimDbContext.BAS_BUYER_BANK_MASTER
                    .OrderByDescending(sc => sc.BANK_ID)
                    .Skip(ExcludeResult)
                    .Take(pageSize)
                    .ToListAsync();
                return result;
            }
            catch (Exception ex)
            {
                throw new System.InvalidOperationException(ex.Message);
            }
        }

        public async Task<bool> DeleteBank(int id)
        {
            try
            {
                var isFoundBank = await DenimDbContext.BAS_BUYER_BANK_MASTER.FindAsync(id);
                if (isFoundBank != null)
                {
                    var banks = await DenimDbContext.BAS_BUYER_BANK_MASTER.Where(pi => pi.BANK_ID == isFoundBank.BANK_ID).ToListAsync();
                    if (banks.Count() > 0)
                    {
                        DenimDbContext.BAS_BUYER_BANK_MASTER.RemoveRange(banks);
                        //SaveChanges();
                    }
                    DenimDbContext.BAS_BUYER_BANK_MASTER.Remove(isFoundBank);
                    await SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw new System.InvalidOperationException(ex.Message);
            }
        }

    }
}
