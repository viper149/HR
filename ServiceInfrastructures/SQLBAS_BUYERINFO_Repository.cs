using System;
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
    public class SQLBAS_BUYERINFO_Repository : BaseRepository<BAS_BUYERINFO>, IBAS_BUYERINFO
    {
        private readonly IDataProtector _protector;

        public SQLBAS_BUYERINFO_Repository(DenimDbContext denimDbContext,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings)
            : base(denimDbContext)
        {
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        public bool FindByBuyerName(string buyerName)
        {
            var buyerNames = DenimDbContext.BAS_BUYERINFO.Where(si => si.BUYER_NAME.Equals(buyerName)).Select(e => e.BUYER_NAME);
            return !buyerNames.Any();
        }

        public async Task<IEnumerable<BAS_BUYERINFO>> GetBasBuyerInfoWithPaged(int pageNumber = 1, int pageSize = 5)
        {
            try
            {
                var excludeResult = (pageSize * pageNumber) - pageSize;

                var result = await DenimDbContext.BAS_BUYERINFO

                    .OrderByDescending(sc => sc.BUYERID)
                    .Skip(excludeResult)
                    .Take(pageSize)
                    .ToListAsync();
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<string> GetBuyerAddressByIdAsync(int buyerId)
        {
            return await DenimDbContext.BAS_BUYERINFO
                .Where(d => d.BUYERID.Equals(buyerId))
                .Select(d => d.ADDRESS)
                .FirstOrDefaultAsync();
        }

        public async Task<string> GetBuyerNameById(int id)
        {
            return await DenimDbContext.BAS_BUYERINFO
                .Where(d => d.BUYERID.Equals(id))
                .Select(d => d.BUYER_NAME)
                .FirstOrDefaultAsync();
        }
        
        public async Task<bool> DeleteInfo(int id)
        {
            try
            {
                var isFoundBuyerInfo = await DenimDbContext.BAS_BUYERINFO.FindAsync(id);
                if (isFoundBuyerInfo != null)
                {
                    var buyerInfo = await DenimDbContext.BAS_BUYERINFO.Where(si => si.BUYERID == isFoundBuyerInfo.BUYERID).ToListAsync();
                    if (buyerInfo.Count() > 0)
                    {
                        DenimDbContext.BAS_BUYERINFO.RemoveRange(buyerInfo);
                        //SaveChanges();
                    }
                    DenimDbContext.BAS_BUYERINFO.Remove(isFoundBuyerInfo);
                    await SaveChanges();
                    return true;
                }

                return false;
            }
            catch (Exception)
            {
                throw new System.InvalidOperationException("Failed!");
            }
        }

        public async Task<IQueryable<BAS_BUYERINFO>> GetDataForDataTableByAsync()
        {
            return DenimDbContext.BAS_BUYERINFO.Select(e => new BAS_BUYERINFO
            {
                EncryptedId = _protector.Protect(e.BUYERID.ToString()),
                BUYER_NAME = e.BUYER_NAME,
                ADDRESS = e.ADDRESS,
                DEL_ADDRESS = e.DEL_ADDRESS,
                BIN_NO = e.BIN_NO,
                REMARKS = e.REMARKS
            });
        }
    }
}
