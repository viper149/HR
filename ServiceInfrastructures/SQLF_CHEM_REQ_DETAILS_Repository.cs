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
    public class SQLF_CHEM_REQ_DETAILS_Repository : BaseRepository<F_CHEM_REQ_DETAILS>, IF_CHEM_REQ_DETAILS
    {
        public SQLF_CHEM_REQ_DETAILS_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }

        public async Task<dynamic> GetSingleChemReqDetails(int csrId, int productId)
        {
            if (csrId is not (0 and <= 0))
            {
                var result = await DenimDbContext.F_CHEM_REQ_DETAILS
                    .Include(c => c.PRODUCT.UNITNAVIGATION)
                    .GroupJoin(DenimDbContext.F_CHEM_STORE_RECEIVE_DETAILS,
                        f1 => f1.PRODUCTID,
                        f2 => f2.PRODUCTID,
                        (f1, f2) => new
                        {
                            CSRID = f1.CSRID,
                            PRODUCTID = f1.PRODUCTID,
                            UNAME = f1.PRODUCT.UNITNAVIGATION.UNAME,
                            REQ_QTY = f1.REQ_QTY,
                            F2 = f2.Select(e => new F_CHEM_STORE_RECEIVE_DETAILS
                            {
                                TRNSID = e.TRNSID,
                                BATCHNO = e.BATCHNO
                            }).ToList()
                        }).FirstOrDefaultAsync(e => e.CSRID.Equals(csrId) && e.PRODUCTID.Equals(productId));
                
                return result;
            }
            else
            {
                return new
                {
                    F2 = await DenimDbContext.F_CHEM_STORE_RECEIVE_DETAILS
                        .Where(e => e.TRNSID.Equals(productId))
                        .Select(e => new F_CHEM_STORE_RECEIVE_DETAILS
                        {
                            TRNSID = e.TRNSID,
                            BATCHNO = e.BATCHNO
                        }).ToListAsync()
                };
            }
        }
        public async Task<IEnumerable<F_CHEM_STORE_RECEIVE_DETAILS>> GetSingleChemReqDetailsAsync(int csrId, int productId)
        {
            if (csrId > 0)
            {
                 return await DenimDbContext.F_CHEM_STORE_RECEIVE_DETAILS
                    .Include(c => c.FChemStoreProductinfo.F_CHEM_REQ_DETAILS)
                    .Include(c => c.FChemStoreProductinfo.UNITNAVIGATION)
                    .Where(d => DenimDbContext.F_CHEM_STORE_RECEIVE_DETAILS.Where(f => f.PRODUCTID.Equals(d.PRODUCTID)).Sum(f => f.FRESH_QTY ?? 0) - DenimDbContext.F_CHEM_ISSUE_DETAILS.Where(f => f.PRODUCTID.Equals(d.PRODUCTID)).Sum(f => f.ISSUE_QTY ?? 0) > 0 && d.PRODUCTID.Equals(productId))
                    .Select(d=> new F_CHEM_STORE_RECEIVE_DETAILS
                    {
                        FChemStoreProductinfo = new F_CHEM_STORE_PRODUCTINFO
                        {
                            F_CHEM_REQ_DETAILS = d.FChemStoreProductinfo.F_CHEM_REQ_DETAILS.Select(x=>new F_CHEM_REQ_DETAILS
                            {
                                CSRID = x.CSRID,
                                REQ_QTY = x.REQ_QTY,
                                CRQID = x.CRQID
                            }).ToList(),
                            UNITNAVIGATION = new F_BAS_UNITS
                            {
                                UNAME = d.FChemStoreProductinfo.UNITNAVIGATION.UNAME
                            }
                        },
                        TRNSID = d.TRNSID,
                        BATCHNO = d.BATCHNO
                    }).ToListAsync();
                //
                //.Where(c => c.PRODUCTID.Equals(productId)
                //            && _denimDbContext.F_CHEM_TRANSECTION.GroupBy(x => x.CRCVID, (key, g) => g.OrderByDescending(e => e.CTRID).First()).Any(d=>d.CRCVID.Equals(c.TRNSID) && d.PRODUCTID.Equals(productId) && d.BALANCE>0)

                //            &&
                //            c.FChemStoreProductinfo.F_CHEM_REQ_DETAILS.Any(e => e.CSRID.Equals(csrId)))
                //.ToListAsync();

                //var x = await _denimDbContext.F_CHEM_REQ_DETAILS
                //    .Include(c => c.PRODUCT.FChemStoreReceiveDetailsesFromProductInfo).ToListAsync();
            }

            return await DenimDbContext.F_CHEM_STORE_RECEIVE_DETAILS
                .Where(e => e.TRNSID.Equals(productId))
                .Select(e => new F_CHEM_STORE_RECEIVE_DETAILS
                {
                    TRNSID = e.TRNSID,
                    BATCHNO = e.BATCHNO
                }).ToListAsync();
        }
    }
}
