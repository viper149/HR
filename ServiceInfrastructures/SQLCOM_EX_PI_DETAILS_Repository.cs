using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;
using DenimERP.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLCOM_EX_PI_DETAILS_Repository : BaseRepository<COM_EX_PI_DETAILS>, ICOM_EX_PI_DETAILS
    {
        public SQLCOM_EX_PI_DETAILS_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }

        public async Task<double?> GetTotalSumByPiNoAsync(int piId)
        {
            return await DenimDbContext.COM_EX_PI_DETAILS
                .Where(cpi => cpi.PIID.Equals(piId))
                .SumAsync(e => e.TOTAL);
        }

        public async Task<IEnumerable<COM_EX_PI_DETAILS>> FindPIListByPINoAsync(int piId)
        {
            var result = await DenimDbContext.COM_EX_PI_DETAILS
                .Include(c => c.STYLE.FABCODENavigation.WV)
                .Include(c => c.PIMASTER.BUYER)
                .Where(c => c.PIID == piId && c.SO_STATUS)
                .ToListAsync();

            foreach (var item in result)
            {
                item.COSTREF = "CS-" + item.COSTID;
                item.SO_NO = $"{item.STYLE.FABCODENavigation.STYLE_NAME} ({item.PIMASTER.BUYER.BUYER_NAME}-{item.SO_NO})";
            }

            return result;
        }
        public async Task<IEnumerable<COM_EX_PI_DETAILS>> FindPIListByPINoAndTransIDAsync(int piId, int trnsId)
        {
            return await DenimDbContext.COM_EX_PI_DETAILS
                .Where(pi => pi.PIID == piId && pi.TRNSID == trnsId).ToListAsync();
        }

        public async Task<COM_EX_PI_DETAILS> FindSoInPOTableAsync(int trnsId)
        {
            return await DenimDbContext.COM_EX_PI_DETAILS
                .Include(c => c.RND_PRODUCTION_ORDER)
                .Include(c => c.STYLE.FABCODENavigation)
                .Where(c => c.RND_PRODUCTION_ORDER.Any(e => e.ORDERNO.Equals(trnsId))).FirstOrDefaultAsync();
        }

        public async Task<COM_EX_PI_DETAILS> FindSoDetailsAsync(int trnsId)
        {
            return await DenimDbContext.COM_EX_PI_DETAILS
                .Include(c => c.RND_PRODUCTION_ORDER)
                .Include(c => c.STYLE.FABCODENavigation)
                .Where(e => e.TRNSID.Equals(trnsId)).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<COM_EX_PI_DETAILS>> FindPiListByPiIdAndStyleIdAsync(int piId, int styleId)
        {
            return await DenimDbContext.COM_EX_PI_DETAILS
                .Where(pi => pi.PIID == piId && pi.STYLEID == styleId)
                .ToListAsync();
        }

        public async Task<string> GetLastSoNoAsync()
        {
            var soNo = "";
            var result = await DenimDbContext.COM_EX_PI_DETAILS.OrderByDescending(c => c.TRNSID).Select(c => c.SO_NO).FirstOrDefaultAsync();
            var year = DateTime.Now.Year - 2000;
            if (result.Contains('R'))
            {
                result = result.Split("R")[0];
            }
            if (result != null)
            {
                var resultArray = result.Split("-");
                if (int.Parse(resultArray[1]) < year)
                {
                    soNo = "SO-" + year + "-" + "1".PadLeft(4, '0');
                }
                else
                {
                    soNo = "SO-" + year + "-" + (int.Parse(resultArray[2]) + 1).ToString().PadLeft(4, '0');
                }
            }
            else
            {
                soNo = "SO-" + year + "-" + "1".PadLeft(4, '0');
            }

            return soNo;
        }

        public async Task<IEnumerable<TypeTableViewModel>> GetSoList()
        {
            return await DenimDbContext.RND_PRODUCTION_ORDER
                .GroupJoin(DenimDbContext.COM_EX_PI_DETAILS,
                    f1 => f1.ORDERNO,
                    f2 => f2.TRNSID,
                    (f1, f2) => new TypeTableViewModel
                    {
                        ID = f1.POID,
                        Name = f2.FirstOrDefault().SO_NO ?? "N/A"
                    })
                .ToListAsync();
        }

        public async Task<IEnumerable<TypeTableViewModel>> GetSoListWithProductionOrder()
        {
            var orderTypes = new[]
            {
                401, // Export
                402, // Local
                419, // Re-Production
                422  // Bulk
            };

            return await DenimDbContext.RND_PRODUCTION_ORDER
                .Include(e => e.SO)
                .Where(e => orderTypes.Any(f => f.Equals(e.OTYPEID)))
                .Select(e => new TypeTableViewModel
                {
                    ID = e.POID,
                    Name = e.SO.SO_NO
                }).OrderBy(e => e.Name).ToListAsync();
        }

        public async Task<IEnumerable<COM_EX_PI_DETAILS>> GetStyleByPiAsync(int piId)
        {
            return await DenimDbContext.COM_EX_PI_DETAILS
                .Include(d => d.STYLE.FABCODENavigation)
                .Where(d => d.PIID.Equals(piId))
                .Select(d => new COM_EX_PI_DETAILS
                {
                    TRNSID = d.TRNSID,
                    STYLE = new COM_EX_FABSTYLE
                    {
                        FABCODENavigation = new RND_FABRICINFO
                        {
                            STYLE_NAME = d.STYLE.FABCODENavigation.STYLE_NAME
                        }
                    }
                }).ToListAsync();
        }

        public async Task<string> GetUnitByPiAsync(int id)
        {
            return await DenimDbContext.COM_EX_PI_DETAILS
                .Include(d=>d.F_BAS_UNITS)
                .Where(d => d.TRNSID.Equals(id))
                .Select(d => d.F_BAS_UNITS.UNAME)
                .FirstOrDefaultAsync();
        }

        public async Task<F_YS_INDENT_DETAILS> GetYarnDetailsbyId(int trnsId)
        {
            return await DenimDbContext.F_YS_INDENT_DETAILS
                .Where(d => d.TRNSID.Equals(trnsId)).AsNoTracking().FirstOrDefaultAsync();
        }

        public Task<int> GetPoIdBySO(int id)
        {
            var x = DenimDbContext.RND_PRODUCTION_ORDER
                .Where(d => d.ORDERNO.Equals(id))
                .Select(d => d.POID).FirstOrDefaultAsync();
            return x;
        }
    }
}
