using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Data;
using DenimERP.Models;
using DenimERP.Security;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;
using DenimERP.ViewModels;
using DenimERP.ViewModels.Planning;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLPL_PRODUCTION_PLAN_MASTER_Repository : BaseRepository<PL_PRODUCTION_PLAN_MASTER>, IPL_PRODUCTION_PLAN_MASTER
    {
        private readonly IDataProtector _protector;

        public SQLPL_PRODUCTION_PLAN_MASTER_Repository(DenimDbContext denimDbContext,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings) : base(denimDbContext)
        {
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        public async Task<PlProductionGroupViewModel> GetInitObjects(PlProductionGroupViewModel plProductionGroupViewModel)
        {
            try
            {
                plProductionGroupViewModel.LotInfoList = await DenimDbContext.BAS_YARN_LOTINFO
                    .Select(c => new BAS_YARN_LOTINFO
                    {
                        LOTNO = c.LOTNO,
                        LOTID = c.LOTID
                    }).ToListAsync();

                plProductionGroupViewModel.PlBulkProgSetupDList = await DenimDbContext.PL_BULK_PROG_SETUP_D
                    .Where(c => !DenimDbContext.PL_PRODUCTION_SETDISTRIBUTION.Any(e => e.PROG_ID.Equals(c.PROG_ID)))
                    .Select(c => new PL_BULK_PROG_SETUP_D
                    {
                        PROG_ID = c.PROG_ID,
                        PROG_NO = c.PROG_NO
                    }).ToListAsync();

                plProductionGroupViewModel.DyeingTypes = await DenimDbContext.RND_DYEING_TYPE
                    .Select(c => new RND_DYEING_TYPE
                    {
                        DID = c.DID,
                        DTYPE = c.DTYPE
                    }).ToListAsync();

                plProductionGroupViewModel.ProductionOrderList =
                    await DenimDbContext.RND_PRODUCTION_ORDER.OrderByDescending(c => c.ORDERNO)
                        .GroupJoin(DenimDbContext.COM_EX_PI_DETAILS.OrderBy(c => c.TRNSID),
                            f1 => f1.ORDERNO,
                            f2 => f2.TRNSID,
                            (f1, f2) => new TypeTableViewModel
                            {
                                Name = f2.FirstOrDefault().SO_NO,
                                ID = f1.POID
                            }).ToListAsync();

                return plProductionGroupViewModel;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<int> InsertAndGetIdAsync(PL_PRODUCTION_PLAN_MASTER plProductionPlanMaster)
        {
            try
            {
                await DenimDbContext.PL_PRODUCTION_PLAN_MASTER.AddAsync(plProductionPlanMaster);
                await SaveChanges();
                return plProductionPlanMaster.GROUPID;
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                throw;
            }
        }

        public async Task<int> GetGroupNo()
        {
            try
            {
                var groupNo = await DenimDbContext.PL_PRODUCTION_PLAN_MASTER
                    .OrderByDescending(c => c.GROUP_NO)
                    .Select(c => c.GROUP_NO)
                    .FirstOrDefaultAsync();
                return groupNo == 0 ? 10000 : groupNo + 1;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<PL_BULK_PROG_SETUP_D> GetProgramLength(int progId)
        {
            try
            {
                var progLength = await DenimDbContext.PL_BULK_PROG_SETUP_D
                    .Include(c => c.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation)
                    .Where(c => c.PROG_ID.Equals(progId))
                    .Select(c => new PL_BULK_PROG_SETUP_D
                    {
                        OPT1 = c.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.REED_COUNT.ToString(),
                        OPT2 = c.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.TOTALROPE.ToString(),
                        OPT3 = c.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.PROGNO.ToString(),
                        OPT4 = c.BLK_PROG_.RndProductionOrder.DYENG_TYPE.ToString(),
                        OPT5 = c.PROCESS_TYPE.ToString(),
                        SET_QTY = c.SET_QTY
                    })
                    .FirstOrDefaultAsync();
                return progLength;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<RND_PRODUCTION_ORDER> GetPoDetails(int soNo)
        {
            try
            {

                var poDetails = await DenimDbContext.RND_PRODUCTION_ORDER
                    .Include(c => c.SO.STYLE.FABCODENavigation)
                    .Where(c => c.POID.Equals(soNo))
                    .FirstOrDefaultAsync();

                return poDetails;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }


        public async Task<IEnumerable<PL_PRODUCTION_PLAN_MASTER>> GetAllAsync(string type)
        {
            try
            {
                var result = await DenimDbContext.PL_PRODUCTION_PLAN_MASTER
                    //.Include(c => c.F_DYEING_PROCESS_ROPE_MASTER)
                    .Include(c => c.PL_PRODUCTION_PLAN_DETAILS)
                    .ThenInclude(c => c.PL_PRODUCTION_SETDISTRIBUTION)
                    .ThenInclude(c => c.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.COLORCODENavigation)
                    .Include(c => c.PL_PRODUCTION_PLAN_DETAILS)
                    .ThenInclude(c => c.PL_PRODUCTION_SETDISTRIBUTION)
                    .ThenInclude(c => c.PROG_.BLK_PROG_.RndProductionOrder.SO.PIMASTER.BUYER)
                    .Include(c => c.RND_DYEING_TYPE)
                    .Include(c => c.PL_PRODUCTION_PLAN_DETAILS)
                    .ThenInclude(c => c.PL_PRODUCTION_SETDISTRIBUTION)
                    .ThenInclude(c => c.F_PR_SLASHER_DYEING_MASTER)
                    .ThenInclude(c => c.F_PR_SLASHER_DYEING_DETAILS)
                    .Where(c => c.RND_DYEING_TYPE.DTYPE.Equals(type))
                    .Select(c => new PL_PRODUCTION_PLAN_MASTER
                    {
                        GROUPID = c.GROUPID,
                        EncryptedId = _protector.Protect(c.GROUPID.ToString()),
                        SERIAL_NO = c.SERIAL_NO,
                        PRODUCTION_DATE = c.PRODUCTION_DATE,
                        GROUP_NO = c.GROUP_NO,
                        GROUPDATE = c.GROUPDATE,
                        DYEING_REFERANCE = c.DYEING_REFERANCE,
                        DYEING_TYPE = c.DYEING_TYPE,
                        RND_DYEING_TYPE = c.RND_DYEING_TYPE,
                        REMARKS = c.REMARKS,
                        OPTION1 = c.PL_PRODUCTION_PLAN_DETAILS.Select(f => f.PL_PRODUCTION_SETDISTRIBUTION.Select(e => e.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.COLORCODENavigation.COLOR).FirstOrDefault()).FirstOrDefault(),
                        OPTION2 = c.RND_DYEING_TYPE.DTYPE == "Rope" ? c.F_DYEING_PROCESS_ROPE_MASTER.Count == 0 && c.F_DYEING_PROCESS_ROPE_MASTER.All(e => e.F_DYEING_PROCESS_ROPE_DETAILS.All(f => f.CLOSE_STATUS)) ? "No" : "Yes" : c.PL_PRODUCTION_PLAN_DETAILS.Select(e => e.PL_PRODUCTION_SETDISTRIBUTION.Select(f => f.F_PR_SLASHER_DYEING_MASTER.Select(g => g.CLOSE_STATUS).FirstOrDefault()).FirstOrDefault()).FirstOrDefault() ? "Yes" : "No",
                        OPTION3 = c.PL_PRODUCTION_PLAN_DETAILS.Select(f => f.PL_PRODUCTION_SETDISTRIBUTION.Select(e => e.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.STYLE_NAME).FirstOrDefault()).FirstOrDefault(),
                        OPTION4 = c.PL_PRODUCTION_PLAN_DETAILS.Select(e => string.Join(",", e.PL_PRODUCTION_SETDISTRIBUTION.Select(f => f.PROG_.BLK_PROG_.RndProductionOrder.SO.SO_NO))).FirstOrDefault(),
                        OPTION5 = c.PL_PRODUCTION_PLAN_DETAILS.Select(f => f.PL_PRODUCTION_SETDISTRIBUTION.Select(e => e.PROG_.BLK_PROG_.RndProductionOrder.SO.PIMASTER.BUYER.BUYER_NAME).FirstOrDefault()).FirstOrDefault(),
                    })
                    //.OrderBy(c => c.SERIAL_NO)
                    .ToListAsync();
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<bool> DateSerialCheck(PL_PRODUCTION_PLAN_MASTER plProductionPlanMaster)
        {
            try
            {
                var dateCheck = await DenimDbContext.PL_PRODUCTION_PLAN_MASTER.AnyAsync(c =>
                    c.PRODUCTION_DATE.Equals(plProductionPlanMaster.PRODUCTION_DATE) &&
                    c.SERIAL_NO.Equals(plProductionPlanMaster.SERIAL_NO));

                return dateCheck;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }


        public async Task<List<PL_PRODUCTION_PLAN_MASTER>> getPlanListSerial(int? serialNo, int groupId)
        {
            try
            {
                var group = await DenimDbContext.PL_PRODUCTION_PLAN_MASTER.FirstOrDefaultAsync(c => c.GROUPID.Equals(groupId));

                var planningList = await DenimDbContext.PL_PRODUCTION_PLAN_MASTER
                        .Where(c => c.SERIAL_NO != null && c.SERIAL_NO >= serialNo && c.DYEING_TYPE.Equals(group.DYEING_TYPE))
                        .OrderBy(c => c.SERIAL_NO)
                        .ToListAsync();

                return planningList;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<PlProductionGroupViewModel> GetPlanDetails(PL_BULK_PROG_SETUP_D plBulkProgSetupD)
        {
            try
            {
                var plProductionGroupViewModel = new PlProductionGroupViewModel();

                var poDetails = await DenimDbContext.PL_BULK_PROG_SETUP_M
                    .Include(c => c.RndProductionOrder.SO.STYLE.FABCODENavigation)
                    .Where(c => c.BLK_PROGID.Equals(plBulkProgSetupD.BLK_PROG_ID))
                    .FirstOrDefaultAsync();


                plProductionGroupViewModel.PlProductionSoDetailsList = new List<PL_PRODUCTION_SO_DETAILS>();
                plProductionGroupViewModel.PlProductionPlanDetailsList = new List<PL_PRODUCTION_PLAN_DETAILS>();


                plProductionGroupViewModel.PlProductionPlanMaster = new PL_PRODUCTION_PLAN_MASTER
                {
                    GROUP_NO = await GetGroupNo(),
                    GROUPDATE = DateTime.Now,
                    DYEING_REFERANCE = poDetails.RndProductionOrder.SO.STYLE.FABCODENavigation.PROGNO,
                    DYEING_TYPE = int.Parse(poDetails.RndProductionOrder.DYENG_TYPE),
                    REMARKS = ""
                };

                plProductionGroupViewModel.PlProductionSoDetailsList.Add(new PL_PRODUCTION_SO_DETAILS
                {
                    POID = poDetails.ORDERNO,
                    GROUPID = 0,
                    REMARKS = ""
                });

                plProductionGroupViewModel.PlProductionPlanDetailsList.Add(new PL_PRODUCTION_PLAN_DETAILS
                {
                    SUBGROUPNO = await GetSubGroupNo(plProductionGroupViewModel),
                    SUBGROUPDATE = DateTime.Now,
                    RATIO = poDetails.RndProductionOrder.SO.STYLE.FABCODENavigation.TOTALROPE.ToString(),
                    REED = poDetails.RndProductionOrder.SO.STYLE.FABCODENavigation.REED_COUNT.ToString(),
                    BEAM_NO = "1",
                    PlProductionSetDistributionList = new List<PL_PRODUCTION_SETDISTRIBUTION>
                    {
                        new PL_PRODUCTION_SETDISTRIBUTION
                        {
                            TRNSDATE = DateTime.Now,
                            SUBGROUPID = 0,
                            PROG_ID = plBulkProgSetupD.PROG_ID,
                            REMARKS = ""
                        }
                    }
                });


                return plProductionGroupViewModel;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<PlProductionGroupViewModel> FindAllByIdAsync(int id)
        {
            try
            {
                var result = await DenimDbContext.PL_PRODUCTION_PLAN_MASTER
                    .Include(c => c.PL_PRODUCTION_PLAN_DETAILS)
                    .ThenInclude(c => c.PL_PRODUCTION_SETDISTRIBUTION)
                    .ThenInclude(c => c.PROG_.BLK_PROG_)
                    .Include(c => c.PL_PRODUCTION_PLAN_DETAILS)
                    .ThenInclude(c => c.LOT)
                    .Include(c => c.PL_PRODUCTION_SO_DETAILS)
                    .ThenInclude(c => c.PO.SO.STYLE.FABCODENavigation)
                    .Include(c => c.RND_DYEING_TYPE)

                    .FirstOrDefaultAsync(c => c.GROUPID.Equals(id));

                var plProductionGroupViewModel = new PlProductionGroupViewModel
                {
                    PlProductionPlanMaster = result,
                    PlProductionPlanDetailsList = result.PL_PRODUCTION_PLAN_DETAILS.ToList(),
                    PlProductionSoDetailsList = result.PL_PRODUCTION_SO_DETAILS.ToList()
                };

                foreach (var item in plProductionGroupViewModel.PlProductionPlanDetailsList)
                {
                    item.PlProductionSetDistributionList = item.PL_PRODUCTION_SETDISTRIBUTION.ToList();
                }

                return plProductionGroupViewModel;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<int> GetSubGroupNo(PlProductionGroupViewModel plProductionGroupViewModel)
        {
            try
            {
                int subGroupNo;
                if (!plProductionGroupViewModel.PlProductionPlanDetailsList.Any())
                {
                    var masterData = await DenimDbContext.PL_PRODUCTION_PLAN_DETAILS.ToListAsync();
                    subGroupNo = masterData.Any() ? masterData.Select(c => c.SUBGROUPNO).Max() : 1000;
                }
                else
                {
                    subGroupNo = plProductionGroupViewModel.PlProductionPlanDetailsList.Select(c => c.SUBGROUPNO).Max();
                }
                return subGroupNo + 1;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<List<PL_PRODUCTION_SETDISTRIBUTION>> GetAllPendingAsync()
        {
            var result = await DenimDbContext.PL_PRODUCTION_SETDISTRIBUTION
                    .Include(c => c.SUBGROUP.GROUP)
                    .Include(c => c.PROG_)
                    .Select(c => new PL_PRODUCTION_SETDISTRIBUTION
                    {
                        TRNSDATE = c.TRNSDATE,
                        SETID = c.SETID,
                        PROG_ = new PL_BULK_PROG_SETUP_D
                        {
                            PROG_NO = c.PROG_.PROG_NO,
                            PROG_ID = c.PROG_.PROG_ID,
                            PROCESS_TYPE = c.PROG_.PROCESS_TYPE
                        },
                        SUBGROUPID = c.SUBGROUPID,
                        OPT2 = c.SUBGROUP.GROUP.GROUP_NO.ToString(),
                        OPT3 = c.SUBGROUP.SUBGROUPNO.ToString(),
                        OPT1 = "Warping Pending"
                    }).ToListAsync();


            foreach (var item in result.Where(c => c.PROG_.PROCESS_TYPE == "ROPE"))
            {

                item.OPT1 = await DenimDbContext.F_PR_SIZING_PROCESS_ROPE_MASTER
                    .Select(c => new F_PR_SIZING_PROCESS_ROPE_MASTER
                    {
                        SETID = c.SETID
                    })
                    .AnyAsync(c => c.SETID.Equals(item.SETID)) ? "Sizing Complete" : item.OPT1;

                if (item.OPT1 == "Sizing Complete") continue;
                {
                    item.OPT1 = await DenimDbContext.F_LCB_PRODUCTION_ROPE_MASTER
                        .Include(c => c.SUBGROUP.PL_PRODUCTION_SETDISTRIBUTION).AnyAsync(c =>
                            c.SUBGROUP.PL_PRODUCTION_SETDISTRIBUTION.Select(d => new PL_PRODUCTION_SETDISTRIBUTION
                            {
                                SETID = d.SETID
                            }).Any(e => e.SETID.Equals(item.SETID))) ? "LCB Complete" : item.OPT1;

                    if (item.OPT1 == "LCB Complete") continue;
                    {
                        item.OPT1 = await DenimDbContext.F_DYEING_PROCESS_ROPE_DETAILS
                            .Include(c => c.SUBGROUP.PL_PRODUCTION_SETDISTRIBUTION)
                            .AnyAsync(c =>
                                c.SUBGROUP.PL_PRODUCTION_SETDISTRIBUTION.Select(d => new PL_PRODUCTION_SETDISTRIBUTION
                                {
                                    SETID = d.SETID
                                }).Any(e => e.SETID.Equals(item.SETID))) ? "Dyeing Complete" : item.OPT1;

                        if (item.OPT1 == "Dyeing Complete") continue;
                        {
                            item.OPT1 =
                                await DenimDbContext.F_PR_WARPING_PROCESS_ROPE_MASTER.Include(c => c.SUBGROUP.PL_PRODUCTION_SETDISTRIBUTION).AnyAsync(c =>
                                    c.SUBGROUP.PL_PRODUCTION_SETDISTRIBUTION.Select(d => new PL_PRODUCTION_SETDISTRIBUTION
                                    {
                                        SETID = d.SETID
                                    }).Any(e => e.SETID.Equals(item.SETID))) ? "Warping Complete" : item.OPT1;
                        }
                    }
                }
            }

            foreach (var item in result.Where(c => c.PROG_.PROCESS_TYPE == "SLASHER"))
            {

                item.OPT1 = await DenimDbContext.F_PR_SLASHER_DYEING_MASTER
                    .Select(c => new F_PR_SLASHER_DYEING_MASTER
                    {
                        SETID = c.SETID
                    })
                    .AnyAsync(c =>
                        c.SETID.Equals(item.SETID)) ? "Dyeing Complete" : item.OPT1;

                if (item.OPT1 == "Dyeing Complete") continue;
                {
                    item.OPT1 = await DenimDbContext.F_PR_WARPING_PROCESS_DW_MASTER
                        .Select(c => new F_PR_WARPING_PROCESS_DW_MASTER
                        {
                            SETID = c.SETID
                        })
                        .AnyAsync(c => c.SETID.Equals(item.SETID)) ? "Warping Complete" : item.OPT1;
                }
            }

            return result;
        }

    }
}
