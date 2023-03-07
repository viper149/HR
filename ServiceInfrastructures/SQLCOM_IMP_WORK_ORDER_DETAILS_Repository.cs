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
    public class SQLCOM_IMP_WORK_ORDER_DETAILS_Repository : BaseRepository<COM_IMP_WORK_ORDER_DETAILS>, ICOM_IMP_WORK_ORDER_DETAILS
    {
        public SQLCOM_IMP_WORK_ORDER_DETAILS_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }

        public async Task<IEnumerable<F_YS_INDENT_DETAILS>> GetCountInfoByIndentIdAsync(int indId)
        {
            return await DenimDbContext.F_YS_INDENT_DETAILS
                .Include(d => d.IND.INDSL.ORDER_NONavigation.SO.STYLE)
                .Include(d => d.BASCOUNTINFO)
                .Include(d => d.LOT)
                .Include(d => d.RAWNavigation)
                .Where(d => d.INDID.Equals(indId) &&
                            !DenimDbContext.COM_IMP_WORK_ORDER_DETAILS.Include(f => f.WO).Any(f => f.WO.INDID.Equals(indId) && f.COUNTID.Equals(d.PRODID)))
                .Select(d => new F_YS_INDENT_DETAILS
                {
                    TRNSID = d.TRNSID,
                    BASCOUNTINFO = new BAS_YARN_COUNTINFO
                    {
                        //RND_COUNTNAME = (d.BASCOUNTINFO.RND_COUNTNAME != null) ? (d.RAWNavigation.RAWPER != null) ? $"{d.BASCOUNTINFO.RND_COUNTNAME} {(_denimDbContext.RND_FABRIC_COUNTINFO.Where(g => g.FABCODE.Equals(d.IND.INDSL.ORDER_NONavigation.SO.STYLE.FABCODE) && g.COUNTID.Equals(d.BASCOUNTINFO.COUNTID) && g.YARNFOR.Equals(d.YARN_FOR)).Select(g => g.YARNTYPE).FirstOrDefault()) ?? ""} ({d.RAWNavigation.RAWPER})" : d.BASCOUNTINFO.RND_COUNTNAME : d.BASCOUNTINFO.COUNTNAME,

                        RND_COUNTNAME = (d.BASCOUNTINFO.RND_COUNTNAME != null) ? (d.RAWNavigation.RAWPER != null) ? $"{d.BASCOUNTINFO.RND_COUNTNAME} ({d.RAWNavigation.RAWPER})" : d.BASCOUNTINFO.RND_COUNTNAME : d.BASCOUNTINFO.COUNTNAME
                    }
                }).ToListAsync();
        }

        public async Task<F_YS_INDENT_DETAILS> GetAllByCountIdAsync(int transId)
        {
            return await DenimDbContext.F_YS_INDENT_DETAILS
                .Include(d => d.BASCOUNTINFO)
                .Include(d => d.LOT)
                .Include(d => d.SLUB_CODENavigation)
                .Where(d => d.TRNSID.Equals(transId))
                .Select(d => new F_YS_INDENT_DETAILS
                {
                    ORDER_QTY = d.ORDER_QTY,
                    NO_OF_CONE = d.NO_OF_CONE,
                    REMARKS = d.REMARKS,
                    LOT = new BAS_YARN_LOTINFO
                    {
                        LOTID = (int?)d.LOT.LOTID ?? 0,
                        LOTNO = $"{d.LOT.LOTNO}"
                    },
                    BASCOUNTINFO = new BAS_YARN_COUNTINFO
                    {
                        UNIT = d.BASCOUNTINFO.UNIT
                    },
                    SLUB_CODENavigation = new F_YS_SLUB_CODE
                    {
                        NAME = d.SLUB_CODENavigation.NAME
                    }
                }).FirstOrDefaultAsync();
        }
    }
}
