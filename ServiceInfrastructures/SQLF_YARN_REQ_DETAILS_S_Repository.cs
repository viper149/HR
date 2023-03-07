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
    public class SQLF_YARN_REQ_DETAILS_S_Repository : BaseRepository<F_YARN_REQ_DETAILS_S>, IF_YARN_REQ_DETAILS_S
    {
        public SQLF_YARN_REQ_DETAILS_S_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }

        public async Task<IEnumerable<POSOViewModel>> GetSoList()
        {
            return await DenimDbContext.RND_PRODUCTION_ORDER
                .GroupJoin(DenimDbContext.COM_EX_PI_DETAILS,
                    f1 => f1.ORDERNO,
                    f2 => f2.TRNSID,
                    (f1, f2) => new POSOViewModel
                    {
                        Poid = f1.POID,
                        SO_NO = f2.FirstOrDefault().SO_NO
                    }).ToListAsync();
        }

        public async Task<FYarnReqSViewModel> GetCountIdList(int poId, int sec)
        {
            FYarnReqSViewModel result;
            if (poId != 0 && sec == 2)
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
                .ThenInclude(c => c.F_YARN_REQ_DETAILS_S)
                .ThenInclude(c => c.F_YS_YARN_ISSUE_DETAILS_S)
                .Include(c => c.SO.PIMASTER.BUYER)
                    .Include(c => c.PL_BULK_PROG_SETUP_M)
                    .ThenInclude(c => c.PL_BULK_PROG_SETUP_D)
                    .ThenInclude(c => c.PL_PRODUCTION_SETDISTRIBUTION)
                .ThenInclude(c => c.F_YARN_REQ_DETAILS_S)
                .ThenInclude(c => c.COUNT.COUNT)
                    .Where(c => c.POID.Equals(poId))

                .Select(c => new FYarnReqSViewModel
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
                            COUNTNAME = e.COUNT.COUNTNAME,
                            COUNTID = e.COUNT.COUNTID
                        },
                        Remaining = (c.ORDER_QTY_YDS * e.FABCODENavigation.RND_YARNCONSUMPTION.FirstOrDefault(f => f.COUNTID.Equals(e.COUNT.COUNTID)).AMOUNT) - c.PL_BULK_PROG_SETUP_M.Select(m => m.PL_BULK_PROG_SETUP_D.Select(d => d.PL_PRODUCTION_SETDISTRIBUTION.Sum(s => s.F_YARN_REQ_DETAILS_S.Where(r => r.F_YS_YARN_ISSUE_DETAILS_S.MAIN_COUNTID.Equals(e.COUNTID)).Sum(r => r.F_YS_YARN_ISSUE_DETAILS_S.ISSUE_QTY))).ToList()).ToList()[0][0],
                            //issue = c.PL_BULK_PROG_SETUP_M.Sum(d => d.PL_BULK_PROG_SETUP_D.Sum(m => m.PL_PRODUCTION_SETDISTRIBUTION.Sum(a => a.F_YARN_REQ_DETAILS_S.Where(q=>q.COUNTID.Equals(e.COUNT.COUNTID)).Sum(v=>v.F_YS_YARN_ISSUE_DETAILS_S.ISSUE_QTY))))
                            //,
                            //Remaining = (c.ORDER_QTY_YDS * e.FABCODENavigation.RND_YARNCONSUMPTION.FirstOrDefault(d => d.COUNTID.Equals(e.COUNTID) && d.YARNFOR.Equals(e.YARNFOR) && d.COLOR.Equals(e.COLORCODE)).AMOUNT) - (c.F_YARN_REQ_DETAILS_S.Any()?c.PROG_.PL_PRODUCTION_SETDISTRIBUTION.Sum(b=>b.F_YARN_REQ_DETAILS_S.Sum(m => m.F_YS_YARN_ISSUE_DETAILS_S.ISSUE_QTY ?? 0)) :0)??0,

                            QTY = c.ORDER_QTY_YDS ?? 0,

                        AMOUNT = $"{c.ORDER_QTY_YDS * e.FABCODENavigation.RND_YARNCONSUMPTION.FirstOrDefault(f => f.COUNTID.Equals(e.COUNT.COUNTID)).AMOUNT:N} ({e.YarnFor.YARNNAME})"
                    }).ToList()
                }).FirstOrDefaultAsync();
            }
            else if (poId != 0 && sec == 1)
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
                .ThenInclude(c => c.F_YARN_REQ_DETAILS_S)
                .ThenInclude(c => c.F_YS_YARN_ISSUE_DETAILS_S)
                .Include(c => c.SO.PIMASTER.BUYER)
                    .Include(c => c.PL_BULK_PROG_SETUP_M)
                    .ThenInclude(c => c.PL_BULK_PROG_SETUP_D)
                    .ThenInclude(c => c.PL_PRODUCTION_SETDISTRIBUTION)
                .ThenInclude(c => c.F_YARN_REQ_DETAILS_S)
                .ThenInclude(c => c.COUNT.COUNT)
                    .Where(c => c.POID.Equals(poId))

                .Select(c => new FYarnReqSViewModel
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
                            COUNTNAME = e.COUNT.COUNTNAME,
                            COUNTID = e.COUNT.COUNTID
                        },
                            //Remaining = (c.ORDER_QTY_YDS * e.FABCODENavigation.RND_YARNCONSUMPTION.FirstOrDefault(f => f.COUNTID.Equals(e.COUNT.COUNTID)).AMOUNT) - c.PL_BULK_PROG_SETUP_M.Select(m => m.PL_BULK_PROG_SETUP_D.Select(d => d.PL_PRODUCTION_SETDISTRIBUTION.Sum(s => s.F_YARN_REQ_DETAILS_S.Where(r => r.F_YS_YARN_ISSUE_DETAILS_S.MAIN_COUNTID.Equals(e.COUNTID)).Sum(r => r.F_YS_YARN_ISSUE_DETAILS_S.ISSUE_QTY))).ToList()).ToList()[0][0],
                            //issue = c.PL_BULK_PROG_SETUP_M.Sum(d => d.PL_BULK_PROG_SETUP_D.Sum(m => m.PL_PRODUCTION_SETDISTRIBUTION.Sum(a => a.F_YARN_REQ_DETAILS_S.Where(q=>q.COUNTID.Equals(e.COUNT.COUNTID)).Sum(v=>v.F_YS_YARN_ISSUE_DETAILS_S.ISSUE_QTY))))
                            //,
                            //Remaining = (c.ORDER_QTY_YDS * e.FABCODENavigation.RND_YARNCONSUMPTION.FirstOrDefault(d => d.COUNTID.Equals(e.COUNTID) && d.YARNFOR.Equals(e.YARNFOR) && d.COLOR.Equals(e.COLORCODE)).AMOUNT) - (c.F_YARN_REQ_DETAILS_S.Any()?c.PROG_.PL_PRODUCTION_SETDISTRIBUTION.Sum(b=>b.F_YARN_REQ_DETAILS_S.Sum(m => m.F_YS_YARN_ISSUE_DETAILS_S.ISSUE_QTY ?? 0)) :0)??0,

                            QTY = c.ORDER_QTY_YDS ?? 0,

                        AMOUNT = $"{c.ORDER_QTY_YDS * e.FABCODENavigation.RND_YARNCONSUMPTION.FirstOrDefault(f => f.COUNTID.Equals(e.COUNT.COUNTID)).AMOUNT:N} ({e.YarnFor.YARNNAME})"
                    }).ToList()
                }).FirstOrDefaultAsync();
            }
            else
            {
                var x = DenimDbContext.BAS_YARN_COUNTINFO
                    .ToList();

                result = new FYarnReqSViewModel
                {
                    BasCount = x.ToList()
                };
            }

            return result;
        }

        public FYarnReqSViewModel GetCountConsumpList(int setId, int sec)
        {
            FYarnReqSViewModel result;
            if (setId != 0 && sec == 2)
            {
                result = DenimDbContext.PL_PRODUCTION_SETDISTRIBUTION
                .Where(c => c.SETID.Equals(setId))
                    .Include(c => c.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.RND_FABRIC_COUNTINFO)
                .ThenInclude(c => c.COUNT).Include(c => c.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.RND_FABRIC_COUNTINFO)
                .ThenInclude(c => c.YarnFor).Include(c => c.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.RND_FABRIC_COUNTINFO)
                .ThenInclude(c => c.Color).Include(c => c.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.RND_YARNCONSUMPTION)
                .Include(c => c.PROG_.BLK_PROG_.RndProductionOrder.SO.PIMASTER.BUYER)
                .Include(c => c.F_PR_SIZING_PROCESS_ROPE_MASTER)
                .ThenInclude(c => c.F_PR_SIZING_PROCESS_ROPE_DETAILS)
                .Include(c => c.F_PR_SLASHER_DYEING_MASTER)
                .ThenInclude(c => c.F_PR_SLASHER_DYEING_DETAILS)
                .Include(c => c.F_YARN_REQ_DETAILS_S)
                .ThenInclude(c => c.F_YS_YARN_ISSUE_DETAILS_S)
                .Select(c => new FYarnReqSViewModel
                {
                    Dynamic = c.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.RND_FABRIC_COUNTINFO.Select(e => new
                    {
                        TRNSID = e.TRNSID,
                        COUNT = new BAS_YARN_COUNTINFO
                        {
                            COUNTNAME = e.COUNT.COUNTNAME,
                            COUNTID = e.COUNT.COUNTID
                        },
                        Remaining = ((c.F_PR_SIZING_PROCESS_ROPE_MASTER != null ? c.F_PR_SIZING_PROCESS_ROPE_MASTER.Sum(m => m.F_PR_SIZING_PROCESS_ROPE_DETAILS.Sum(d => d.LENGTH_PER_BEAM)) : c.F_PR_SLASHER_DYEING_MASTER != null ? c.F_PR_SLASHER_DYEING_MASTER.Sum(d => d.F_PR_SLASHER_DYEING_DETAILS.Sum(m => m.LENGTH_PER_BEAM)) : 0) * e.FABCODENavigation.RND_YARNCONSUMPTION.FirstOrDefault(d => d.COUNTID.Equals(e.COUNTID) && d.YARNFOR.Equals(e.YARNFOR) && d.COLOR.Equals(e.COLORCODE)).AMOUNT) - (c.F_YARN_REQ_DETAILS_S.Any() ? c.F_YARN_REQ_DETAILS_S.Where(r => r.F_YS_YARN_ISSUE_DETAILS_S.MAIN_COUNTID.Equals(e.COUNTID)).Sum(m => m.F_YS_YARN_ISSUE_DETAILS_S.ISSUE_QTY ?? 0) : 0) ?? 0,


                        QTY = c.F_PR_SIZING_PROCESS_ROPE_MASTER != null ? c.F_PR_SIZING_PROCESS_ROPE_MASTER.Sum(m => m.F_PR_SIZING_PROCESS_ROPE_DETAILS.Sum(d => d.LENGTH_PER_BEAM)) : c.F_PR_SLASHER_DYEING_MASTER != null ? c.F_PR_SLASHER_DYEING_MASTER.Sum(d => d.F_PR_SLASHER_DYEING_DETAILS.Sum(m => m.LENGTH_PER_BEAM)) : 0,
                        AMOUNT = $"{(c.F_PR_SIZING_PROCESS_ROPE_MASTER != null ? c.F_PR_SIZING_PROCESS_ROPE_MASTER.Sum(m => m.F_PR_SIZING_PROCESS_ROPE_DETAILS.Sum(d => d.LENGTH_PER_BEAM)) : c.F_PR_SLASHER_DYEING_MASTER != null ? c.F_PR_SLASHER_DYEING_MASTER.Sum(d => d.F_PR_SLASHER_DYEING_DETAILS.Sum(m => m.LENGTH_PER_BEAM)) : 0) * e.FABCODENavigation.RND_YARNCONSUMPTION.FirstOrDefault(d => d.COUNTID.Equals(e.COUNTID) && d.YARNFOR.Equals(e.YARNFOR) && d.COLOR.Equals(e.COLORCODE)).AMOUNT:N} ({e.YarnFor.YARNNAME})"
                    }).ToList()
                }).FirstOrDefault();
            }
            if (setId != 0 && sec == 1)
            {
                result = DenimDbContext.PL_PRODUCTION_SETDISTRIBUTION
                .Where(c => c.SETID.Equals(setId))
                    .Include(c => c.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.RND_FABRIC_COUNTINFO)
                .ThenInclude(c => c.COUNT).Include(c => c.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.RND_FABRIC_COUNTINFO)
                .ThenInclude(c => c.YarnFor).Include(c => c.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.RND_FABRIC_COUNTINFO)
                .ThenInclude(c => c.Color).Include(c => c.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.RND_YARNCONSUMPTION)
                .Include(c => c.PROG_.BLK_PROG_.RndProductionOrder.SO.PIMASTER.BUYER)
                .Include(c => c.F_PR_SIZING_PROCESS_ROPE_MASTER)
                .ThenInclude(c => c.F_PR_SIZING_PROCESS_ROPE_DETAILS)
                .Include(c => c.F_PR_SLASHER_DYEING_MASTER)
                .ThenInclude(c => c.F_PR_SLASHER_DYEING_DETAILS)
                .Include(c => c.F_YARN_REQ_DETAILS_S)
                .ThenInclude(c => c.F_YS_YARN_ISSUE_DETAILS_S)
                .Select(c => new FYarnReqSViewModel
                {
                    Dynamic = c.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.RND_FABRIC_COUNTINFO.Select(e => new
                    {
                        TRNSID = e.TRNSID,
                        COUNT = new BAS_YARN_COUNTINFO
                        {
                            COUNTNAME = e.COUNT.COUNTNAME,
                            COUNTID = e.COUNT.COUNTID
                        },
                        Remaining = (c.PROG_.SET_QTY * e.FABCODENavigation.RND_YARNCONSUMPTION.FirstOrDefault(d => d.COUNTID.Equals(e.COUNTID) && d.YARNFOR.Equals(e.YARNFOR) && d.COLOR.Equals(e.COLORCODE)).AMOUNT) - (c.F_YARN_REQ_DETAILS_S.Any() ? c.F_YARN_REQ_DETAILS_S.Where(r => r.F_YS_YARN_ISSUE_DETAILS_S.MAIN_COUNTID.Equals(e.COUNTID)).Sum(m => m.F_YS_YARN_ISSUE_DETAILS_S.ISSUE_QTY ?? 0) : 0) ?? 0,


                        QTY = c.PROG_.SET_QTY,
                        AMOUNT = $"{c.PROG_.SET_QTY * e.FABCODENavigation.RND_YARNCONSUMPTION.FirstOrDefault(d => d.COUNTID.Equals(e.COUNTID) && d.YARNFOR.Equals(e.YARNFOR) && d.COLOR.Equals(e.COLORCODE)).AMOUNT:N} ({e.YarnFor.YARNNAME})"
                    }).ToList()
                }).FirstOrDefault();
            }
            else
            {
                var x = DenimDbContext.BAS_YARN_COUNTINFO
                    .ToList();

                result = new FYarnReqSViewModel
                {
                    BasCount = x.ToList()
                };
            }

            return result;
        }

        public dynamic GetSetList(int poId)
        {
            return DenimDbContext.PL_PRODUCTION_SETDISTRIBUTION
                .Include(c => c.PROG_.BLK_PROG_.RndProductionOrder)
                .Where(c => c.PROG_.BLK_PROG_.RndProductionOrder.POID.Equals(poId))
                .Select(c => new TypeTableViewModel
                {
                    ID = c.SETID,
                    Name = c.PROG_.PROG_NO
                }).ToList();
        }

        public async Task<IEnumerable<F_YARN_REQ_DETAILS_S>> GetYarnReqCountList(int orderno, int ysrid = 0)
        {
            return await DenimDbContext.F_YARN_REQ_DETAILS_S
                .Include(e => e.COUNT)
                .ThenInclude(c => c.COUNT)
                .Where(f => f.ORDERNO.Equals(orderno) && f.YSRID.Equals(ysrid))
                .ToListAsync();
        }

        public async Task<F_YARN_REQ_DETAILS_S> GetSingleYarnReqDetails(int countId, double qty)
        {
            return await DenimDbContext.F_YARN_REQ_DETAILS_S
                .Include(e => e.COUNT.COUNT)
                .ThenInclude(c => c.F_YARN_TRANSACTION_S)
                .ThenInclude(c => c.LOT)
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.COUNT.COUNT.COUNTID.Equals(countId) && e.REQ_QTY.Equals(qty));
        }

        public async Task<IEnumerable<F_YARN_TRANSACTION_S>> GetYarnLotDetails(int countId)
        {
            return await DenimDbContext.F_YARN_TRANSACTION_S
                .Include(c => c.LOT)
                .Where(e => e.COUNTID.Equals(countId))
                .GroupBy(l => new { l.COUNTID, l.LOTID })
                .Select(g => g.OrderByDescending(c => c.YTRNID).FirstOrDefault())
                .ToListAsync();
        }

        public async Task<FYarnReqSViewModel> GetInitObjectsOfSelectedItems(FYarnReqSViewModel fYarnReqSViewModel)
        {
            foreach (var item in fYarnReqSViewModel.FYarnRequirementDetailsSList)
            {
                item.COUNT = await DenimDbContext.RND_FABRIC_COUNTINFO
                    .Include(e => e.COUNT).FirstOrDefaultAsync(e => e.TRNSID.Equals(item.COUNTID));
                item.LOT = await DenimDbContext.BAS_YARN_LOTINFO.FirstOrDefaultAsync(e => e.LOTID.Equals(item.LOTID));
                item.UNITNavigation = await DenimDbContext.F_BAS_UNITS.FirstOrDefaultAsync(e => e.UID.Equals(item.UNIT));
                item.SET = await DenimDbContext.PL_PRODUCTION_SETDISTRIBUTION
                    .Include(c => c.PROG_)
                    .FirstOrDefaultAsync(e => e.SETID.Equals(item.SETID));
                item.ORDERNONavigation = await DenimDbContext.RND_PRODUCTION_ORDER
                    .Include(c => c.SO)
                    .FirstOrDefaultAsync(e => e.POID.Equals(item.ORDERNO));
            }

            return fYarnReqSViewModel;
        }
    }
}
