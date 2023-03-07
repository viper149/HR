using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces.PostCosting;
using DenimERP.ViewModels.PostCostingMaster;
using Microsoft.EntityFrameworkCore;

namespace DenimERP.ServiceInfrastructures.PostCosting
{
    public class SQLCOS_POSTCOSTING_YARNDETAILS_Repository:BaseRepository<COS_POSTCOSTING_YARNDETAILS>, ICOS_POSTCOSTING_YARNDETAILS
    {
        public SQLCOS_POSTCOSTING_YARNDETAILS_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }

        public async Task<PostCostingViewModel> GetYarnDetailsBySo(PostCostingViewModel postCostingViewModel)
        {
            try
            {
                var warpRope = await DenimDbContext
                    .F_PR_WARPING_PROCESS_ROPE_YARN_CONSUM_DETAILS
                    .Include(c=>c.COUNT_)
                    .Include(c=>c.WARP.SUBGROUP.PL_PRODUCTION_SETDISTRIBUTION)
                    .ThenInclude(c=>c.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.RND_FABRIC_COUNTINFO)
                    .Where(c => c.WARP.SUBGROUP.PL_PRODUCTION_SETDISTRIBUTION.Any(e=>e.PROG_.BLK_PROG_.RndProductionOrder.POID.Equals(postCostingViewModel.CosPostcostingMaster.SO_NO)))
                    .Select(c=>new
                    {
                        Countid = c.COUNT_ID,
                        YarnFor = 1,
                        RndCountName = c.COUNT_.RND_COUNTNAME,
                        CountName = c.COUNT_.COUNTNAME,
                        Consumprion = c.CONSUM_TOTAL,
                        RND_Count = c.WARP.SUBGROUP.PL_PRODUCTION_SETDISTRIBUTION.Select(q=>q.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.RND_FABRIC_COUNTINFO.Where(r=>r.COUNTID.Equals(c.COUNT_ID) && r.YARNFOR==1).Select(r=>r.TRNSID).FirstOrDefault()).FirstOrDefault()
                    }).ToListAsync();

                var warpSlasher = await DenimDbContext
                    .F_PR_WARPING_PROCESS_DW_YARN_CONSUM_DETAILS
                    .Include(c=>c.COUNT_.COUNT)
                    .Include(c=>c.WARP_.SET)
                    .ThenInclude(c=>c.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.RND_FABRIC_COUNTINFO)
                    .Where(c => c.WARP_.SET.PROG_.BLK_PROG_.RndProductionOrder.POID.Equals(postCostingViewModel.CosPostcostingMaster.SO_NO))
                    .Select(c=>new
                    {
                        Countid = c.COUNT_.COUNTID,
                        YarnFor = 1,
                        RndCountName = c.COUNT_.COUNT.RND_COUNTNAME,
                        CountName = c.COUNT_.COUNT.COUNTNAME,
                        Consumprion = c.CONSM,
                        RND_Count = c.COUNT_ID
                    })
                    .ToListAsync();

                var warpSectional = await DenimDbContext
                    .F_PR_WARPING_PROCESS_SW_YARN_CONSUM_DETAILS
                    .Include(c => c.COUNT_.COUNT)
                    .Include(c => c.SW_.SET)
                    .ThenInclude(c => c.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.RND_FABRIC_COUNTINFO)
                    .Where(c => c.SW_.SET.PROG_.BLK_PROG_.RndProductionOrder.POID.Equals(postCostingViewModel.CosPostcostingMaster.SO_NO))
                    .Select(c => new
                    {
                        Countid = c.COUNT_.COUNTID,
                        YarnFor = 1,
                        RndCountName = c.COUNT_.COUNT.RND_COUNTNAME,
                        CountName = c.COUNT_.COUNT.COUNTNAME,
                        Consumprion = c.CONSM,
                        RND_Count = c.COUNT_ID
                    })
                    .ToListAsync();

                var warpEcru = await DenimDbContext
                    .F_PR_WARPING_PROCESS_ECRU_YARN_CONSUM_DETAILS
                    .Include(c => c.COUNT_.COUNT)
                    .Include(c => c.ECRU_.SET)
                    .ThenInclude(c => c.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.RND_FABRIC_COUNTINFO)
                    .Where(c => c.ECRU_.SET.PROG_.BLK_PROG_.RndProductionOrder.POID.Equals(postCostingViewModel.CosPostcostingMaster.SO_NO))
                    .Select(c => new
                    {
                        Countid = c.COUNT_.COUNTID,
                        YarnFor = 1,
                        RndCountName = c.COUNT_.COUNT.RND_COUNTNAME,
                        CountName = c.COUNT_.COUNT.COUNTNAME,
                        Consumprion = c.CONSM,
                        RND_Count = c.COUNT_ID
                    })
                    .ToListAsync();

                var weft = await DenimDbContext
                    .F_PR_WEAVING_WEFT_YARN_CONSUM_DETAILS
                    .Include(c => c.COUNT.COUNT)
                    .Include(c => c.WEAVING.SET.PROG_.BLK_PROG_.RndProductionOrder)
                    .Where(c => c.WEAVING.SET.PROG_.BLK_PROG_.RndProductionOrder.POID.Equals(postCostingViewModel.CosPostcostingMaster.SO_NO))
                    .Select(c => new
                    {
                        Countid = c.COUNT.COUNTID,
                        YarnFor = 2,
                        RndCountName = c.COUNT.COUNT.RND_COUNTNAME,
                        CountName = c.COUNT.COUNT.COUNTNAME,
                        Consumprion = c.CONSUMP,
                        RND_Count = c.COUNTID
                    }).ToListAsync();

                var yarnList = new List<COS_POSTCOSTING_YARNDETAILS>();

                foreach (var item in warpRope)
                {

                    var x = await DenimDbContext.F_YARN_REQ_DETAILS
                        .Include(c => c.F_YS_YARN_ISSUE_DETAILS.RCVD)
                        .Include(c=>c.PO)
                        .Include(c=>c.COUNT)
                        .Where(c => c.ORDER_TYPE.Equals("OrderNo") && c.COUNT.COUNTID.Equals(item.Countid) && c.ORDERNO.Equals(postCostingViewModel.CosPostcostingMaster.SO_NO)).FirstOrDefaultAsync();
                    
                    yarnList.Add(new COS_POSTCOSTING_YARNDETAILS
                    {
                        COUNTID = x.COUNT.COUNTID,
                        LOTID = x.F_YS_YARN_ISSUE_DETAILS?.RCVD?.LOT,
                        CONSUMPTION = warpRope.Where(c => c.Countid.Equals(item.Countid)).Sum(c=>c.Consumprion),
                        YARNFOR = item.YarnFor
                    });
                }

                foreach (var item in warpSlasher)
                {

                    var x = await DenimDbContext.F_YARN_REQ_DETAILS
                        .Include(c => c.F_YS_YARN_ISSUE_DETAILS.RCVD)
                        .Include(c=>c.PO)
                        .Include(c=>c.COUNT)
                        .Where(c => c.COUNTID.Equals(item.RND_Count) && c.ORDERNO.Equals(postCostingViewModel.CosPostcostingMaster.SO_NO)).FirstOrDefaultAsync();
                    
                    yarnList.Add(new COS_POSTCOSTING_YARNDETAILS
                    {
                        COUNTID = x.COUNT.COUNTID,
                        LOTID = x.F_YS_YARN_ISSUE_DETAILS?.RCVD?.LOT,
                        CONSUMPTION = warpSlasher.Where(c => c.Countid.Equals(item.Countid)).Sum(c=>c.Consumprion),
                        YARNFOR = item.YarnFor
                    });
                }

                foreach (var item in warpEcru)
                {

                    var x = await DenimDbContext.F_YARN_REQ_DETAILS
                        .Include(c => c.F_YS_YARN_ISSUE_DETAILS.RCVD)
                        .Include(c=>c.PO)
                        .Include(c=>c.COUNT)
                        .Where(c => c.COUNTID.Equals(item.RND_Count) && c.ORDERNO.Equals(postCostingViewModel.CosPostcostingMaster.SO_NO)).FirstOrDefaultAsync();
                    
                    yarnList.Add(new COS_POSTCOSTING_YARNDETAILS
                    {
                        COUNTID = x.COUNT.COUNTID,
                        LOTID = x.F_YS_YARN_ISSUE_DETAILS?.RCVD?.LOT,
                        CONSUMPTION = warpEcru.Where(c => c.Countid.Equals(item.Countid)).Sum(c=>c.Consumprion),
                        YARNFOR = item.YarnFor
                    });
                }

                foreach (var item in warpSectional)
                {

                    var x = await DenimDbContext.F_YARN_REQ_DETAILS
                        .Include(c => c.F_YS_YARN_ISSUE_DETAILS.RCVD)
                        .Include(c=>c.PO)
                        .Include(c=>c.COUNT)
                        .Where(c => c.COUNTID.Equals(item.RND_Count) && c.ORDERNO.Equals(postCostingViewModel.CosPostcostingMaster.SO_NO)).FirstOrDefaultAsync();

                    yarnList.Add(new COS_POSTCOSTING_YARNDETAILS
                    {
                        COUNTID = x.COUNT.COUNTID,
                        LOTID = x.F_YS_YARN_ISSUE_DETAILS?.RCVD?.LOT,
                        CONSUMPTION = warpSectional.Where(c => c.Countid.Equals(item.Countid)).Sum(c=>c.Consumprion),
                        YARNFOR = item.YarnFor
                    });
                }
                
                foreach (var item in weft)
                {
                    var x = await DenimDbContext.F_YARN_REQ_DETAILS
                        .Include(c => c.F_YS_YARN_ISSUE_DETAILS.RCVD)
                        .Include(c => c.PO)
                        .Include(c => c.COUNT)
                        .Where(c=> c.COUNTID.Equals(item.RND_Count) && c.ORDERNO.Equals(postCostingViewModel.CosPostcostingMaster.SO_NO)).FirstOrDefaultAsync();


                    yarnList.Add(new COS_POSTCOSTING_YARNDETAILS
                    {
                        COUNTID = x.COUNT.COUNTID,
                        LOTID = x.F_YS_YARN_ISSUE_DETAILS?.RCVD?.LOT,
                        CONSUMPTION = weft.Where(c=>c.Countid.Equals(item.Countid)).Sum(c=>double.Parse(c.Consumprion)),
                        YARNFOR = item.YarnFor
                    });
                }

                postCostingViewModel.CosPostCostingYarnDetailsList = new List<COS_POSTCOSTING_YARNDETAILS>();
                for (int i = 0; i < yarnList.Count; i++)
                {
                    if (i == yarnList.TakeWhile(t => !(t.COUNTID.Equals(yarnList[i].COUNTID) && t.LOTID == yarnList[i].LOTID && t.CONSUMPTION.Equals(yarnList[i].CONSUMPTION) && t.YARNFOR.Equals(yarnList[i].YARNFOR))).Count())
                    {
                        postCostingViewModel.CosPostCostingYarnDetailsList.Add(yarnList[i]);
                    }
                }

                return postCostingViewModel;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
