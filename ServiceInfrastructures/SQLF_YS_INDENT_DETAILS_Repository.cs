using System;
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
    public class SQLF_YS_INDENT_DETAILS_Repository : BaseRepository<F_YS_INDENT_DETAILS>, IF_YS_INDENT_DETAILS
    {
        public SQLF_YS_INDENT_DETAILS_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }

        public async Task<dynamic> GetIndentYarnListByIndidAsync(int indId)
        {
            try
            {
                if (indId != 4069)
                {
                    var x = await DenimDbContext.F_YS_INDENT_DETAILS
                        .Include(e => e.YARN_FOR)
                        .Include(e => e.LOT)
                        .Include(e => e.RAWNavigation)
                        .Include(e => e.SLUB_CODENavigation)
                        .Include(e => e.BASCOUNTINFO)
                        .Include(e => e.IND.F_YS_YARN_RECEIVE_MASTER)
                        .ThenInclude(e => e.F_YS_YARN_RECEIVE_DETAILS)
                        .Where(e => e.INDID.Equals(indId))
                        .Select(e => new
                        {
                            COUNTID = e.BASCOUNTINFO.COUNTID,
                            RND_COUNTNAME = $"{e.BASCOUNTINFO.RND_COUNTNAME} ({e.YARN_FORNavigation.YARNNAME})",
                            ORDER_QTY = e.ORDER_QTY,
                            CNSMP_AMOUNT = e.CNSMP_AMOUNT,
                            LOTNO = e.LOT.LOTNO,
                            RAW = e.RAWNavigation.RAWPER,
                            SLUB = e.SLUB_CODENavigation.NAME,
                            PREVCHALLANDETAILS = e.IND.F_YS_YARN_RECEIVE_MASTER.Select(f => new
                            {
                                f.F_YS_YARN_RECEIVE_DETAILS
                            }).ToList()
                        }).OrderBy(e => e.RND_COUNTNAME).ToListAsync();

                    return x;
                }

                return await DenimDbContext.BAS_YARN_COUNTINFO
                    .Select(e => new
                    {
                        COUNTID = e.COUNTID,
                        RND_COUNTNAME = e.RND_COUNTNAME
                    }).OrderBy(e => e.RND_COUNTNAME).ToListAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
        }

        public async Task<dynamic> GetInvoiceDetailsByINVIDAsync(int invId)
        {
            return await DenimDbContext.COM_IMP_INVOICEINFO
                .Include(e => e.LC.SUPP)
                .Where(e => e.INVID.Equals(invId))
                .Select(e => new
                {
                    LcNo = e.LC.LCNO,
                    SUPPID=e.LC.SUPP.SUPPID,
                    Supplier = e.LC.SUPP.SUPPNAME
                }).OrderBy(e => e.LcNo).FirstOrDefaultAsync();
        }

        public async Task<F_YS_INDENT_DETAILS> GetIndentQtyAsync(int prodId, int indId)
        {
            var indentQty = await DenimDbContext.F_YS_INDENT_DETAILS
                .Where(c => c.INDID.Equals(indId) && c.PRODID.Equals(prodId))
                .FirstOrDefaultAsync();

            return indentQty;
        }

        public async Task<F_YS_INDENT_DETAILS> FindIndentListByIdAsync(int id, int prdId)
        {
            var details = await DenimDbContext.F_YS_INDENT_DETAILS
                    .Include(e => e.SEC)
                    .Include(e => e.FBasUnits)
                    .Include(e => e.BASCOUNTINFO)
                    .Include(e => e.YARN_FORNavigation)
                    .Include(e => e.YARN_FROMNavigation)
                    .Include(e => e.LOT)
                    .Where(e => e.INDSLID.Equals(id) && e.PRODID.Equals(prdId))
                    .FirstOrDefaultAsync();
            return details;
        }

        public async Task<FYarnIndentComExPiViewModel> FindAllpRODUCTListAsync(int id)
        {
            //var details = await _denimDbContext.F_YS_INDENT_DETAILS
            //    .Include(e => e.SEC)
            //    .Include(e => e.BASCOUNTINFO)
            //    .Where(e => e.INDSLID.Equals(id))
            //    .Select(c => new BAS_YARN_COUNTINFO
            //    {
            //        COUNTID = c.BASCOUNTINFO.COUNTID,
            //        COUNTNAME = c.BASCOUNTINFO.COUNTNAME,
            //    }).ToListAsync();
            //return details;

            var fYarnIndentComExPiViewModel = new FYarnIndentComExPiViewModel
            {
                FYsIndentDetailsList = await DenimDbContext.F_YS_INDENT_DETAILS
                    .Include(e => e.BASCOUNTINFO)
                    .Include(e => e.SEC)
                    .Include(e => e.RND_PURCHASE_REQUISITION_MASTER)
                    .ThenInclude(e => e.EMP)
                    .Include(e => e.RND_PURCHASE_REQUISITION_MASTER)
                    .ThenInclude(e => e.ORDER_NONavigation)
                    .Include(e => e.RND_PURCHASE_REQUISITION_MASTER)
                    .ThenInclude(e => e.ORDERNO_SNavigation)
                    .ThenInclude(e => e.BUYER)
                    .Where(e => e.INDSLID.Equals(id)).ToListAsync()
            };

            fYarnIndentComExPiViewModel.ComExPiDetailseList = fYarnIndentComExPiViewModel.FYsIndentDetailsList.Where(c => c.RND_PURCHASE_REQUISITION_MASTER.ORDER_NONavigation != null)
                    .GroupJoin(DenimDbContext.COM_EX_PI_DETAILS
                        .Include(e => e.PIMASTER)
                        .ThenInclude(e => e.BUYER),
                    f1 => f1.RND_PURCHASE_REQUISITION_MASTER.ORDER_NONavigation.ORDERNO,
                    f2 => f2.TRNSID,
                    (f1, f2) => f2.ToList()).FirstOrDefault();



            return fYarnIndentComExPiViewModel;
        }



        public async Task<YarnRequirementViewModel> GetCountIdList(int poId)
        {
            try
            {
                YarnRequirementViewModel result = null;
                if (poId != 0)
                {
                    result = await DenimDbContext.RND_PRODUCTION_ORDER
                        .Include(c => c.SO.STYLE.FABCODENavigation.RND_FABRIC_COUNTINFO)
                    .ThenInclude(c => c.COUNT).Include(c => c.SO.STYLE.FABCODENavigation.RND_FABRIC_COUNTINFO)
                    .ThenInclude(c => c.YarnFor).Include(c => c.SO.STYLE.FABCODENavigation.RND_FABRIC_COUNTINFO)
                    .ThenInclude(c => c.Color)
                        .Include(c => c.SO.STYLE.FABCODENavigation.RND_YARNCONSUMPTION)
                    .Include(c => c.SO.PIMASTER.BUYER)
                        .Include(c => c.PL_BULK_PROG_SETUP_M)
                        .ThenInclude(c => c.PL_BULK_PROG_SETUP_D)
                        .ThenInclude(c => c.PL_PRODUCTION_SETDISTRIBUTION)
                    .ThenInclude(c => c.F_YARN_REQ_DETAILS)
                    .ThenInclude(c => c.F_YS_YARN_ISSUE_DETAILS)
                    .Include(c => c.SO.PIMASTER.BUYER)
                        .Include(c => c.PL_BULK_PROG_SETUP_M)
                        .ThenInclude(c => c.PL_BULK_PROG_SETUP_D)
                        .ThenInclude(c => c.PL_PRODUCTION_SETDISTRIBUTION)
                    .ThenInclude(c => c.F_YARN_REQ_DETAILS)
                    .ThenInclude(c => c.COUNT.COUNT)
                        .Where(c => c.POID.Equals(poId))

                    .Select(c => new YarnRequirementViewModel
                    {
                        RndFabricCountinfos = c.SO.STYLE.FABCODENavigation.RND_FABRIC_COUNTINFO.ToList(),
                        Count = c.SO.STYLE.FABCODENavigation.RND_FABRIC_COUNTINFO.Select(e => new RND_FABRIC_COUNTINFO
                        {
                            TRNSID = e.TRNSID,
                            COUNTID = e.COUNTID,
                            COUNT = new BAS_YARN_COUNTINFO
                            {
                                COUNTNAME = $"{e.COUNT.RND_COUNTNAME} - {e.Color.COLOR}",
                                COUNTID = e.COUNT.COUNTID
                            }
                        }).ToList(),
                        PiDetails = new COM_EX_PI_DETAILS
                        {
                            STYLE = new COM_EX_FABSTYLE
                            {
                                FABCODENavigation = new RND_FABRICINFO
                                {
                                    STYLE_NAME = c.SO.STYLE.FABCODENavigation.STYLE_NAME
                                }
                            },
                            PIMASTER = new COM_EX_PIMASTER
                            {
                                BUYER = new BAS_BUYERINFO
                                {
                                    BUYER_NAME = c.SO.PIMASTER.BUYER.BUYER_NAME
                                }
                            }
                        },
                        Dynamic = c.SO.STYLE.FABCODENavigation.RND_FABRIC_COUNTINFO.Select(e => new
                        {
                            TRNSID = e.TRNSID,
                            COUNT = new BAS_YARN_COUNTINFO
                            {
                                COUNTNAME = $"{e.COUNT.RND_COUNTNAME} - {e.Color.COLOR}",
                                COUNTID = e.COUNT.COUNTID
                            },
                            
                            QTY = c.ORDER_QTY_YDS ?? 0,

                            AMOUNT = $"{c.ORDER_QTY_YDS * e.FABCODENavigation.RND_YARNCONSUMPTION.FirstOrDefault(f => f.COUNTID.Equals(e.COUNT.COUNTID)).AMOUNT:N} ({e.YarnFor.YARNNAME})"
                        }).ToList()
                    }).FirstOrDefaultAsync();
                }
                else
                {
                    var x = DenimDbContext.BAS_YARN_COUNTINFO
                        .ToList();

                    result = new YarnRequirementViewModel
                    {
                        BasCount = x.ToList()
                    };
                }

                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<int> GetYarnForByIndProdAsync(int indId, int prodId)
        {
            return await DenimDbContext.F_YS_INDENT_DETAILS
                .Where(d=>d.INDID.Equals(indId) && d.PRODID.Equals(prodId))
                .Select(d => d.YARN_FOR ?? 0)
                .FirstOrDefaultAsync();
        }
    }
}
