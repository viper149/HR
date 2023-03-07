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
    public class SQLBAS_SUPPLIERINFO_Repository : BaseRepository<BAS_SUPPLIERINFO>, IBAS_SUPPLIERINFO
    {
        public SQLBAS_SUPPLIERINFO_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }

        public async Task<string> FindSupplierNameByIdAsync(int id)
        {
            var supplierNames = await DenimDbContext.BAS_SUPPLIERINFO.Where(si => si.SUPPID == id).Select(e => e.SUPPNAME).ToListAsync();
            return supplierNames.FirstOrDefault();
        }

        public async Task<IEnumerable<BAS_SUPPLIERINFO>> GetForSelectItemsByAsync()
        {
            return await DenimDbContext.BAS_SUPPLIERINFO.Select(e => new BAS_SUPPLIERINFO
            {
                SUPPID = e.SUPPID,
                SUPPNAME = e.SUPPNAME
            }).ToListAsync();
        }

        public bool FindBySupplierInfoName(string supplierName)
        {
            var suppNames = DenimDbContext.BAS_SUPPLIERINFO.Where(si => si.SUPPNAME.Equals(supplierName)).Select(e => e.SUPPNAME);
            return !suppNames.Any();
        }

        public async Task<IEnumerable<BAS_SUPPLIERINFO>> GetBasSupplierInfoWithPaged(int pageNumber = 1, int pageSize = 5)
        {
            try
            {
                var excludeResult = (pageSize * pageNumber) - pageSize;

                var result = await DenimDbContext.BAS_SUPPLIERINFO
                    .Include(si=>si.SCAT).Where(s=>s.SCATID==s.SCAT.SCATID)
                    .OrderByDescending(sc => sc.SUPPID)
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

        public async Task<IEnumerable<BAS_SUPPLIERINFO>> GetBasSupplierInfoAllAsync()
        {
            try
            {
                var result = await DenimDbContext.BAS_SUPPLIERINFO
                    .Include(si=>si.SCAT).Where(s=>s.SCATID==s.SCAT.SCATID)
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
                var isFoundSupplierInfo = await DenimDbContext.BAS_SUPPLIERINFO.FindAsync(id);
                if (isFoundSupplierInfo != null)
                {
                    var supplierInfo = await DenimDbContext.BAS_SUPPLIERINFO.Where(si => si.SUPPID == isFoundSupplierInfo.SUPPID).ToListAsync();
                    if (supplierInfo.Count() > 0)
                    {
                        DenimDbContext.BAS_SUPPLIERINFO.RemoveRange(supplierInfo);
                        //SaveChanges();
                    }
                    DenimDbContext.BAS_SUPPLIERINFO.Remove(isFoundSupplierInfo);
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

        public async Task<BAS_SUPPLIERINFO> FindSupplierInfoByAsync(int id)
        {
            try
            {
                var sUPPLIERINFO = await DenimDbContext.BAS_SUPPLIERINFO.Include(si => si.SCAT).Where(si => si.SCATID == si.SCAT.SCATID & si.SUPPID == id).FirstOrDefaultAsync();
                return sUPPLIERINFO;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public async Task<bool> FindBySupplierInfoById(int id)
        {
            var suppNames = await DenimDbContext.BAS_SUPPLIERINFO.AnyAsync(si => si.SUPPID == id);
            return suppNames;
        }
    }
}
