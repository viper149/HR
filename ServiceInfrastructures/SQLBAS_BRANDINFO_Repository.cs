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
    public class SQLBAS_BRANDINFO_Repository : BaseRepository<BAS_BRANDINFO>, IBAS_BRANDINFO
    {
        public SQLBAS_BRANDINFO_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }
        
        public bool FindByBrandName(string brandName)
        {
            var buyerNames = DenimDbContext.BAS_BRANDINFO.Where(si => si.BRANDNAME.Equals(brandName)).Select(e => e.BRANDNAME);

            if (buyerNames.Any())
                return false;
            else
                return true;
        }

        public async Task<IEnumerable<BAS_BRANDINFO>> GetBasBrandInfoWithPaged(int pageNumber = 1, int pageSize = 5)
        {
            try
            {
                int ExcludeResult = (pageSize * pageNumber) - pageSize;

                var result = await DenimDbContext.BAS_BRANDINFO

                    .OrderByDescending(sc => sc.BRANDID)
                    .Skip(ExcludeResult)
                    .Take(pageSize)
                    .ToListAsync();
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<bool> DeleteInfo(int id)
        {
            try
            {
                var isFoundBrandInfo = await DenimDbContext.BAS_BRANDINFO.FindAsync(id);
                if (isFoundBrandInfo != null)
                {
                    var brandInfo = await DenimDbContext.BAS_BRANDINFO.Where(si => si.BRANDID == isFoundBrandInfo.BRANDID).ToListAsync();
                    if (brandInfo.Count() > 0)
                    {
                        DenimDbContext.BAS_BRANDINFO.RemoveRange(brandInfo);
                        //SaveChanges();
                    }
                    DenimDbContext.BAS_BRANDINFO.Remove(isFoundBrandInfo);
                    await SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                throw new System.InvalidOperationException("Failed!");
            }
        }
    }
}
