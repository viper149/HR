using System.Linq;
using System.Threading.Tasks;
using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;
using Microsoft.EntityFrameworkCore;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLF_GEN_S_REQ_DETAILS_Repository : BaseRepository<F_GEN_S_REQ_DETAILS>, IF_GEN_S_REQ_DETAILS
    {
        public SQLF_GEN_S_REQ_DETAILS_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }

        public async Task<F_GEN_S_REQ_DETAILS> GetSingleGenSReqDetails(int gsrId, int productId)
        {
            return await DenimDbContext.F_GEN_S_REQ_DETAILS
                .Include(d => d.PRODUCT.UNITNavigation)
                .Where(e => e.GSRID.Equals(gsrId) && e.PRODUCTID.Equals(productId))
                .Select(d => new F_GEN_S_REQ_DETAILS
                {
                    GRQID = d.GRQID,
                    REQ_QTY = d.REQ_QTY,
                    PRODUCT = new F_GS_PRODUCT_INFORMATION
                    {
                        UNITNavigation = new F_BAS_UNITS
                        {
                            UNAME = d.PRODUCT.UNITNavigation.UNAME
                        }
                    }

                }).FirstOrDefaultAsync();
        }
    }
}
