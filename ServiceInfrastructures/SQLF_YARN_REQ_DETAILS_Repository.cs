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
    public class SQLF_YARN_REQ_DETAILS_Repository : BaseRepository<F_YARN_REQ_DETAILS>, IF_YARN_REQ_DETAILS
    {
        public SQLF_YARN_REQ_DETAILS_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {

        }

        public async Task<IEnumerable<POSOViewModel>> GetSoList()
        {
            try
            {
                var result = await DenimDbContext.RND_PRODUCTION_ORDER
                    .GroupJoin(DenimDbContext.COM_EX_PI_DETAILS,
                        f1 => f1.ORDERNO,
                        f2 => f2.TRNSID,
                        (f1, f2) => new POSOViewModel
                        {
                            Poid = f1.POID,
                            SO_NO = f2.FirstOrDefault().SO_NO
                        }).ToListAsync();

                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public dynamic GetSetList(int poId)
        {
            try
            {
                var result = DenimDbContext.PL_PRODUCTION_SETDISTRIBUTION
                    .Include(c => c.PROG_.BLK_PROG_.RndProductionOrder)
                    .Where(c => c.PROG_.BLK_PROG_.RndProductionOrder.POID.Equals(poId))
                    .Select(c => new TypeTableViewModel
                    {
                        ID = c.SETID,
                        Name = c.PROG_.PROG_NO
                    }).ToList();
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<YarnRequirementViewModel> GetCountIdList(int poId, int sec)
        {
            YarnRequirementViewModel result = null;
            List<RND_FABRIC_COUNTINFO> list = new List<RND_FABRIC_COUNTINFO>();

            if (poId == 304644)
            {
                var v = await DenimDbContext.RND_FABRIC_COUNTINFO
                    .Include(c => c.COUNT)
                    .ToListAsync();


                for (int i = 0; i < v.Count; i++)
                {
                    if (i == v.TakeWhile(t => !(t.COUNTID.Equals(v[i].COUNTID))).Count())
                    {
                        var x = v[i];
                        //x.COUNT.RND_FABRIC_COUNTINFO = null;
                        list.Add(x);
                    }
                }

                result = new YarnRequirementViewModel
                {
                    Count = list
                };

                return result;
            }


            if (poId != 0 && sec == 2 && poId != 304644)
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
                            COUNTID = e.COUNT.COUNTID,
                            RND_COUNTNAME = $"{e.COUNT.RND_COUNTNAME} - {e.Color.COLOR}"
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
                            RND_COUNTNAME = $"{e.COUNT.RND_COUNTNAME} - {e.Color.COLOR}",
                            COUNTID = e.COUNT.COUNTID
                        },
                        Remaining = ((c.ORDER_QTY_YDS * e.FABCODENavigation.RND_YARNCONSUMPTION.FirstOrDefault(f => f.COUNTID.Equals(e.COUNT.COUNTID)).AMOUNT) - c.PL_BULK_PROG_SETUP_M.Select(m => m.PL_BULK_PROG_SETUP_D.Select(d => d.PL_PRODUCTION_SETDISTRIBUTION.Sum(s => s.F_YARN_REQ_DETAILS.Where(r => r.F_YS_YARN_ISSUE_DETAILS.MAIN_COUNTID.Equals(e.COUNTID) && r.F_YS_YARN_ISSUE_DETAILS.REQ_DET_ID.Equals(r.TRNSID)).Sum(r => r.F_YS_YARN_ISSUE_DETAILS.ISSUE_QTY))).ToList()).ToList()[0][0]) ?? c.ORDER_QTY_YDS * e.FABCODENavigation.RND_YARNCONSUMPTION.FirstOrDefault(f => f.COUNTID.Equals(e.COUNT.COUNTID)).AMOUNT,
                            //issue = c.PL_BULK_PROG_SETUP_M.Sum(d => d.PL_BULK_PROG_SETUP_D.Sum(m => m.PL_PRODUCTION_SETDISTRIBUTION.Sum(a => a.F_YARN_REQ_DETAILS.Where(q=>q.COUNTID.Equals(e.COUNT.COUNTID)).Sum(v=>v.F_YS_YARN_ISSUE_DETAILS.ISSUE_QTY))))
                            //,
                            //Remaining = (c.ORDER_QTY_YDS * e.FABCODENavigation.RND_YARNCONSUMPTION.FirstOrDefault(d => d.COUNTID.Equals(e.COUNTID) && d.YARNFOR.Equals(e.YARNFOR) && d.COLOR.Equals(e.COLORCODE)).AMOUNT) - (c.F_YARN_REQ_DETAILS.Any()?c.PROG_.PL_PRODUCTION_SETDISTRIBUTION.Sum(b=>b.F_YARN_REQ_DETAILS.Sum(m => m.F_YS_YARN_ISSUE_DETAILS.ISSUE_QTY ?? 0)) :0)??0,

                            QTY = c.ORDER_QTY_YDS ?? 0,

                        AMOUNT = $"{c.ORDER_QTY_YDS * e.FABCODENavigation.RND_YARNCONSUMPTION.FirstOrDefault(f => f.COUNTID.Equals(e.COUNT.COUNTID)).AMOUNT:N} ({e.YarnFor.YARNNAME})"
                    }).ToList()
                }).FirstOrDefaultAsync();
            }
            else if (poId != 0 && sec == 1 && poId != 304644)
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
                            RND_COUNTNAME = $"{e.COUNT.RND_COUNTNAME} - {e.Color.COLOR}",
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
                            RND_COUNTNAME = $"{e.COUNT.RND_COUNTNAME} - {e.Color.COLOR}",
                            COUNTID = e.COUNT.COUNTID
                        },
                        Remaining = ((c.ORDER_QTY_YDS * e.FABCODENavigation.RND_YARNCONSUMPTION.FirstOrDefault(f => f.COUNTID.Equals(e.COUNT.COUNTID)).AMOUNT) - (c.PL_BULK_PROG_SETUP_M.Sum(m => m.PL_BULK_PROG_SETUP_D.Sum(d => d.PL_PRODUCTION_SETDISTRIBUTION.Sum(v => v.F_YARN_REQ_DETAILS.Where(p => p.F_YS_YARN_ISSUE_DETAILS.MAIN_COUNTID.Equals(e.COUNTID)).Sum(o => o.F_YS_YARN_ISSUE_DETAILS.ISSUE_QTY)))))) ?? c.ORDER_QTY_YDS * e.FABCODENavigation.RND_YARNCONSUMPTION.FirstOrDefault(f => f.COUNTID.Equals(e.COUNT.COUNTID)).AMOUNT,
                            //Remaining = (c.ORDER_QTY_YDS * e.FABCODENavigation.RND_YARNCONSUMPTION.FirstOrDefault(f => f.COUNTID.Equals(e.COUNT.COUNTID)).AMOUNT) - c.PL_BULK_PROG_SETUP_M.Select(m => m.PL_BULK_PROG_SETUP_D.Select(d => d.PL_PRODUCTION_SETDISTRIBUTION.Sum(s => s.F_YARN_REQ_DETAILS.Where(r => r.F_YS_YARN_ISSUE_DETAILS.MAIN_COUNTID.Equals(e.COUNTID)).Sum(r => r.F_YS_YARN_ISSUE_DETAILS.ISSUE_QTY))).ToList()).ToList()[0][0],
                            //issue = c.PL_BULK_PROG_SETUP_M.Sum(d => d.PL_BULK_PROG_SETUP_D.Sum(m => m.PL_PRODUCTION_SETDISTRIBUTION.Sum(a => a.F_YARN_REQ_DETAILS.Where(q=>q.COUNTID.Equals(e.COUNT.COUNTID)).Sum(v=>v.F_YS_YARN_ISSUE_DETAILS.ISSUE_QTY))))
                            //,
                            //Remaining = (c.ORDER_QTY_YDS * e.FABCODENavigation.RND_YARNCONSUMPTION.FirstOrDefault(d => d.COUNTID.Equals(e.COUNTID) && d.YARNFOR.Equals(e.YARNFOR) && d.COLOR.Equals(e.COLORCODE)).AMOUNT) - (c.F_YARN_REQ_DETAILS.Any()?c.PROG_.PL_PRODUCTION_SETDISTRIBUTION.Sum(b=>b.F_YARN_REQ_DETAILS.Sum(m => m.F_YS_YARN_ISSUE_DETAILS.ISSUE_QTY ?? 0)) :0)??0,
                            rcv = c.PL_BULK_PROG_SETUP_M.Sum(m => m.PL_BULK_PROG_SETUP_D.Sum(d => d.PL_PRODUCTION_SETDISTRIBUTION.Sum(v => v.F_YARN_REQ_DETAILS.Where(p => p.F_YS_YARN_ISSUE_DETAILS.MAIN_COUNTID.Equals(e.COUNTID)).Sum(o => o.F_YS_YARN_ISSUE_DETAILS.ISSUE_QTY)))),

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


        public async Task<IEnumerable<RND_FABRIC_COUNTINFO>> GetCountIdList2()
        {
            return await DenimDbContext.RND_FABRIC_COUNTINFO
                .Include(d => d.COUNT)
                .Include(d => d.Color)
                .Where(d => !d.COUNT.Equals(null))
                .Select(d => new RND_FABRIC_COUNTINFO
                {
                    COUNT = new BAS_YARN_COUNTINFO
                    {
                        COUNTID = d.COUNT.COUNTID,
                        RND_COUNTNAME = d.COLORCODE == null ? d.COUNT.RND_COUNTNAME : $"{d.COUNT.RND_COUNTNAME} - {d.Color.COLOR}"
                    }
                }).Distinct().ToListAsync();
        }

        public YarnRequirementViewModel GetCountConsumpList(int setId, int sec)
        {
            YarnRequirementViewModel result = null;
            if (setId != 0 && sec == 2)
            {
                return DenimDbContext.PL_PRODUCTION_SETDISTRIBUTION
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
                    .Include(c => c.F_YARN_REQ_DETAILS)
                    .ThenInclude(c => c.F_YS_YARN_ISSUE_DETAILS)
                    .Select(c => new YarnRequirementViewModel
                    {
                        Dynamic = c.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.RND_FABRIC_COUNTINFO.Select(e => new
                        {
                            TRNSID = e.TRNSID,
                            COUNT = new BAS_YARN_COUNTINFO
                            {
                                RND_COUNTNAME = $"{e.COUNT.RND_COUNTNAME} - {e.Color.COLOR}",
                                COUNTID = e.COUNT.COUNTID
                            },
                            Remaining = (((c.F_PR_SIZING_PROCESS_ROPE_MASTER != null ? c.F_PR_SIZING_PROCESS_ROPE_MASTER.Sum(m => m.F_PR_SIZING_PROCESS_ROPE_DETAILS.Sum(d => d.LENGTH_PER_BEAM)) : c.F_PR_SLASHER_DYEING_MASTER != null ? c.F_PR_SLASHER_DYEING_MASTER.Sum(d => d.F_PR_SLASHER_DYEING_DETAILS.Sum(m => m.LENGTH_PER_BEAM)) : 0) * e.FABCODENavigation.RND_YARNCONSUMPTION.FirstOrDefault(d => d.COUNTID.Equals(e.COUNTID) && d.YARNFOR.Equals(e.YARNFOR) && d.COLOR.Equals(e.COLORCODE)).AMOUNT) - (c.F_YARN_REQ_DETAILS.Any() ? c.F_YARN_REQ_DETAILS.Where(r => r.F_YS_YARN_ISSUE_DETAILS.MAIN_COUNTID.Equals(e.COUNTID) && r.F_YS_YARN_ISSUE_DETAILS.REQ_DET_ID.Equals(r.TRNSID)).Sum(m => m.F_YS_YARN_ISSUE_DETAILS.ISSUE_QTY ?? 0) : 0)) ?? (c.F_PR_SIZING_PROCESS_ROPE_MASTER != null ? c.F_PR_SIZING_PROCESS_ROPE_MASTER.Sum(m => m.F_PR_SIZING_PROCESS_ROPE_DETAILS.Sum(d => d.LENGTH_PER_BEAM)) : c.F_PR_SLASHER_DYEING_MASTER != null ? c.F_PR_SLASHER_DYEING_MASTER.Sum(d => d.F_PR_SLASHER_DYEING_DETAILS.Sum(m => m.LENGTH_PER_BEAM)) : 0) * e.FABCODENavigation.RND_YARNCONSUMPTION.FirstOrDefault(d => d.COUNTID.Equals(e.COUNTID) && d.YARNFOR.Equals(e.YARNFOR) && d.COLOR.Equals(e.COLORCODE)).AMOUNT,
                            QTY = c.F_PR_SIZING_PROCESS_ROPE_MASTER != null ? c.F_PR_SIZING_PROCESS_ROPE_MASTER.Sum(m => m.F_PR_SIZING_PROCESS_ROPE_DETAILS.Sum(d => d.LENGTH_PER_BEAM)) : c.F_PR_SLASHER_DYEING_MASTER != null ? c.F_PR_SLASHER_DYEING_MASTER.Sum(d => d.F_PR_SLASHER_DYEING_DETAILS.Sum(m => m.LENGTH_PER_BEAM)) : 0,
                            AMOUNT = $"{(c.F_PR_SIZING_PROCESS_ROPE_MASTER != null ? c.F_PR_SIZING_PROCESS_ROPE_MASTER.Sum(m => m.F_PR_SIZING_PROCESS_ROPE_DETAILS.Sum(d => d.LENGTH_PER_BEAM)) : c.F_PR_SLASHER_DYEING_MASTER != null ? c.F_PR_SLASHER_DYEING_MASTER.Sum(d => d.F_PR_SLASHER_DYEING_DETAILS.Sum(m => m.LENGTH_PER_BEAM)) : 0) * e.FABCODENavigation.RND_YARNCONSUMPTION.FirstOrDefault(d => d.COUNTID.Equals(e.COUNTID) && d.YARNFOR.Equals(e.YARNFOR) && d.COLOR.Equals(e.COLORCODE)).AMOUNT:N} ({e.YarnFor.YARNNAME})"
                        }).ToList()
                    }).FirstOrDefault();
            }

            if (setId != 0 && sec == 1)
            {
                return DenimDbContext.PL_PRODUCTION_SETDISTRIBUTION
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
                    .Include(c => c.F_YARN_REQ_DETAILS)
                    .ThenInclude(c => c.F_YS_YARN_ISSUE_DETAILS)
                    .Select(c => new YarnRequirementViewModel
                    {
                        Dynamic = c.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.RND_FABRIC_COUNTINFO.Select(e => new
                        {
                            TRNSID = e.TRNSID,
                            COUNT = new BAS_YARN_COUNTINFO
                            {
                                RND_COUNTNAME = $"{e.COUNT.RND_COUNTNAME} - {e.Color.COLOR}",
                                COUNTID = e.COUNT.COUNTID
                            },
                            Remaining = ((c.PROG_.SET_QTY * e.FABCODENavigation.RND_YARNCONSUMPTION.FirstOrDefault(d => d.COUNTID.Equals(e.COUNTID) && d.YARNFOR.Equals(e.YARNFOR) && d.COLOR.Equals(e.COLORCODE)).AMOUNT) - (c.F_YARN_REQ_DETAILS.Any() ? c.F_YARN_REQ_DETAILS.Where(r => r.F_YS_YARN_ISSUE_DETAILS.MAIN_COUNTID.Equals(e.COUNTID) && r.F_YS_YARN_ISSUE_DETAILS.REQ_DET_ID.Equals(r.TRNSID)).Sum(m => m.F_YS_YARN_ISSUE_DETAILS.ISSUE_QTY ?? 0) : 0)) ?? (c.PROG_.SET_QTY * e.FABCODENavigation.RND_YARNCONSUMPTION.FirstOrDefault(d => d.COUNTID.Equals(e.COUNTID) && d.YARNFOR.Equals(e.YARNFOR) && d.COLOR.Equals(e.COLORCODE)).AMOUNT),
                            QTY = c.PROG_.SET_QTY,
                            AMOUNT = $"{c.PROG_.SET_QTY * e.FABCODENavigation.RND_YARNCONSUMPTION.FirstOrDefault(d => d.COUNTID.Equals(e.COUNTID) && d.YARNFOR.Equals(e.YARNFOR) && d.COLOR.Equals(e.COLORCODE)).AMOUNT:N} ({e.YarnFor.YARNNAME})"
                        }).ToList()
                    }).FirstOrDefault();
            }

            var x = DenimDbContext.BAS_YARN_COUNTINFO
                .ToList();

            return new YarnRequirementViewModel
            {
                BasCount = x.ToList()
            };
        }

        public async Task<IEnumerable<F_YARN_REQ_DETAILS>> GetYarnReqCountList(int orderNo, int ysrId)
        {
            return await DenimDbContext.F_YARN_REQ_DETAILS
                .Include(e => e.COUNT)
                .ThenInclude(c => c.COUNT)
                .Where(f => f.ORDERNO.Equals(orderNo) && f.YSRID.Equals(ysrId))
                .ToListAsync();
        }

        public async Task<F_YARN_REQ_DETAILS> GetSingleYarnReqDetails(int countId, double qty)
        {
            return await DenimDbContext.F_YARN_REQ_DETAILS
                .Include(e => e.COUNT.COUNT)
                .Include(e => e.RSCOUNT)
                .ThenInclude(c => c.F_YARN_TRANSACTION)
                .ThenInclude(c => c.LOT)
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.RSID == null ? e.COUNT.COUNT.COUNTID.Equals(countId) : e.RSCOUNT.COUNTID.Equals(countId) && e.REQ_QTY.Equals(qty));
        }

        public async Task<IEnumerable<F_YARN_TRANSACTION>> GetYarnLotDetails(int countId)
        {
            return await DenimDbContext.F_YARN_TRANSACTION
                .Include(c => c.LOT)
                .Where(e => e.COUNTID.Equals(countId))
                .GroupBy(l => new { l.COUNTID, l.LOTID })
                .Select(g => g.OrderByDescending(c => c.YTRNID).FirstOrDefault())
                .ToListAsync();
        }

        public async Task<IEnumerable<F_YARN_TRANSACTION>> GetYarnLotDetailsByCount(int countId, int indentType)
        {
            try
            {
                var result = await DenimDbContext.F_YARN_TRANSACTION
                    .Include(c => c.LOT)
                    .Where(e => e.COUNTID.Equals(countId) && e.INDENT_TYPE.Equals(indentType))
                    .GroupBy(l => new { l.COUNTID, l.LOTID, l.INDENT_TYPE })
                    .Select(g => g.OrderByDescending(c => c.YTRNID).FirstOrDefault())
                    .ToListAsync();
                return result;
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                return null;
            }
        }

        public async Task<IEnumerable<F_YARN_TRANSACTION>> GetYarnLotDetailsByIndentType(int countId, int indentType)
        {
            return await DenimDbContext.F_YARN_TRANSACTION
                .Include(c => c.LOT)
                .Where(e => e.COUNTID.Equals(countId) && e.INDENT_TYPE.Equals(indentType))
                .GroupBy(l => new { l.COUNTID, l.LOTID, l.INDENT_TYPE })
                .Select(g => g.OrderByDescending(c => c.YTRNID).FirstOrDefault())
                .ToListAsync();
        }

        public async Task<YarnRequirementViewModel> GetInitObjectsOfSelectedItems(YarnRequirementViewModel yarnRequirement)
        {
            foreach (var item in yarnRequirement.FYarnRequirementDetailsList)
            {
                item.COUNT = await DenimDbContext.RND_FABRIC_COUNTINFO
                    .Include(e => e.COUNT).Include(d => d.Color).FirstOrDefaultAsync(e => e.TRNSID.Equals(item.COUNTID));

                item.LOT = await DenimDbContext.BAS_YARN_LOTINFO.FirstOrDefaultAsync(e => e.LOTID.Equals(item.LOTID));

                item.FBasUnits = await DenimDbContext.F_BAS_UNITS.FirstOrDefaultAsync(e => e.UID.Equals(item.UNIT));

                item.SET = await DenimDbContext.PL_PRODUCTION_SETDISTRIBUTION
                    .Include(c => c.PROG_)
                    .FirstOrDefaultAsync(e => e.SETID.Equals(item.SETID));

                item.PO = await DenimDbContext.RND_PRODUCTION_ORDER
                    .Include(c => c.SO)
                    .FirstOrDefaultAsync(e => e.POID.Equals(item.ORDERNO));

                item.RS = await DenimDbContext.RND_SAMPLE_INFO_DYEING
                    .Where(d => d.SDID.Equals(item.RSID))
                    .Select(d => new RND_SAMPLE_INFO_DYEING
                    {
                        RSOrder = d.RSOrder
                    }).FirstOrDefaultAsync();

                item.RSCOUNT = await DenimDbContext.BAS_YARN_COUNTINFO
                    .Where(d => d.COUNTID.Equals(item.COUNTID_RS))
                    .Select(d => new BAS_YARN_COUNTINFO
                    {
                        RND_COUNTNAME = d.RND_COUNTNAME
                    }).FirstOrDefaultAsync();
            }

            return yarnRequirement;
        }

        public async Task<RND_SAMPLE_INFO_DYEING> GetStyleByRSNO(int rsId)
        {
            return await DenimDbContext.RND_SAMPLE_INFO_DYEING

                .Where(d => d.SDID.Equals(rsId))
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<BAS_YARN_COUNTINFO>> GetCountIdList2T()
        {
            return await DenimDbContext.BAS_YARN_COUNTINFO
                .Select(d=> new BAS_YARN_COUNTINFO
                {
                    COUNTID = d.COUNTID,
                    RND_COUNTNAME = d.RND_COUNTNAME
                }).ToListAsync();
        }

        public async Task<int> GetCountIdByReqDId(int reqId)
        {
            return await DenimDbContext.F_YARN_REQ_DETAILS
                .Include(d => d.COUNT.COUNT)
                .Include(d => d.RSCOUNT)
                .Where(d => d.TRNSID.Equals(reqId))
                .Select(d => d.RSID == null ? d.COUNT.COUNT.COUNTID : d.RSCOUNT.COUNTID)
                .FirstOrDefaultAsync();
        }
    }
}
