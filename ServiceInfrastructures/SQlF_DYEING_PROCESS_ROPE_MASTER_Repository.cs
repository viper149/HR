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
using DenimERP.ViewModels.Factory.Production;
using DenimERP.ViewModels.Home;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using Exception = System.Exception;

namespace DenimERP.ServiceInfrastructures
{
    public class SQlF_DYEING_PROCESS_ROPE_MASTER_Repository : BaseRepository<F_DYEING_PROCESS_ROPE_MASTER>, IF_DYEING_PROCESS_ROPE_MASTER
    {
        private readonly IDataProtector _protector;

        public SQlF_DYEING_PROCESS_ROPE_MASTER_Repository(DenimDbContext denimDbContext,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings) : base(denimDbContext)
        {
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        public async Task<IEnumerable<F_DYEING_PROCESS_ROPE_MASTER>> GetAllAsync()
        {
            try
            {

                var fDyeingProcessRopeMasters = await DenimDbContext.F_DYEING_PROCESS_ROPE_MASTER
                    .Include(c => c.GROUP.PL_PRODUCTION_PLAN_DETAILS)
                    .ThenInclude(c => c.PL_PRODUCTION_SETDISTRIBUTION)
                    .ThenInclude(c => c.PROG_.BLK_PROG_.RndProductionOrder.SO)
                    .Include(c => c.GROUP.PL_PRODUCTION_PLAN_DETAILS)
                    .ThenInclude(c => c.PL_PRODUCTION_SETDISTRIBUTION)
                    .ThenInclude(c => c.PROG_.BLK_PROG_.RndProductionOrder.RS)
                    .Include(c => c.F_DYEING_PROCESS_ROPE_CHEM)
                    .Select(e => new F_DYEING_PROCESS_ROPE_MASTER
                    {
                        ROPE_DID = e.ROPE_DID,
                        TRNSDATE = e.TRNSDATE,
                        EncryptedId = _protector.Protect(e.ROPE_DID.ToString()),
                        DYEING_CODE = e.DYEING_CODE,
                        DYEING_LENGTH = e.DYEING_LENGTH,
                        GROUP_LENGTH = e.GROUP_LENGTH,
                        REMARKS = e.REMARKS,
                        GROUP = new PL_PRODUCTION_PLAN_MASTER
                        {
                            GROUP_NO = e.GROUP.GROUP_NO,
                            OPTION1 = string.Join(" + ", e.GROUP.PL_PRODUCTION_PLAN_DETAILS
                                .SelectMany(f => f.PL_PRODUCTION_SETDISTRIBUTION
                                    .Select(g => (g.PROG_.BLK_PROG_.RndProductionOrder.SO.SO_NO != null) ? g.PROG_.PROG_NO + $" ({g.PROG_.BLK_PROG_.RndProductionOrder.SO.SO_NO}) " : g.PROG_.PROG_NO + $" ({g.PROG_.BLK_PROG_.RndProductionOrder.RS.RSOrder}) ")).ToArray())
                        },
                        F_DYEING_PROCESS_ROPE_CHEM = e.F_DYEING_PROCESS_ROPE_CHEM.ToList()
                    }).ToListAsync();

                //foreach (var item in fDyeingProcessRopeMasters)
                //{
                //    item.GROUP.OPTION1 = $"({item.GROUP.PL_PRODUCTION_PLAN_DETAILS.FirstOrDefault()?.PL_PRODUCTION_SETDISTRIBUTION.FirstOrDefault()?.PROG_.BLK_PROG_.RndProductionOrder.SO.SO_NO}) " + string.Join('+', item.GROUP.PL_PRODUCTION_PLAN_DETAILS.Select(i => string.Join('+', i.PL_PRODUCTION_SETDISTRIBUTION.Select(c => $"{c.PROG_.PROG_NO}"))));
                //}

                return fDyeingProcessRopeMasters;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<FDyeingProcessRopeViewModel> GetInitObjectsByAsync(FDyeingProcessRopeViewModel dyeingProcessRopeViewModel)
        {
            if (dyeingProcessRopeViewModel.FDyeingProcessRopeMaster is { GROUPID: > 0 })
            {
                dyeingProcessRopeViewModel.PlProductionPlanMasterList = await DenimDbContext.PL_PRODUCTION_PLAN_MASTER
                .Include(c => c.PL_PRODUCTION_PLAN_DETAILS)
                .ThenInclude(c => c.F_PR_WARPING_PROCESS_ROPE_MASTER)
                .Include(c => c.RND_DYEING_TYPE)
                .Where(c => c.GROUPID.Equals(dyeingProcessRopeViewModel.FDyeingProcessRopeMaster.GROUPID) &&
                            c.PL_PRODUCTION_PLAN_DETAILS.All(e => e.F_PR_WARPING_PROCESS_ROPE_MASTER.All(f => f.IS_DECLARE)) && c.RND_DYEING_TYPE.DTYPE.Equals("Rope"))
                .Select(c => new PL_PRODUCTION_PLAN_MASTER
                {
                    GROUPID = c.GROUPID,
                    GROUP_NO = c.GROUP_NO
                }).ToListAsync();
            }

            dyeingProcessRopeViewModel.ChemStoreProductInfoList = await DenimDbContext.F_CHEM_STORE_PRODUCTINFO
                .Select(c => new F_CHEM_STORE_PRODUCTINFO
                {
                    PRODUCTID = c.PRODUCTID,
                    PRODUCTNAME = c.PRODUCTNAME
                }).ToListAsync();

            dyeingProcessRopeViewModel.PrWarpingProcessRopeBallDetailsList = await DenimDbContext.F_PR_WARPING_PROCESS_ROPE_BALL_DETAILS
                .Include(c => c.BALL_ID_FKNavigation)
                .Select(c => new F_PR_WARPING_PROCESS_ROPE_BALL_DETAILS
                {
                    BALLID = c.BALLID,
                    OPT1 = c.BALL_ID_FKNavigation.BALL_NO
                }).ToListAsync();

            dyeingProcessRopeViewModel.PlProductionPlanDetailsList = await DenimDbContext.PL_PRODUCTION_PLAN_DETAILS
                .Include(c => c.PL_PRODUCTION_SETDISTRIBUTION)
                .ThenInclude(c => c.PROG_.PROG_NO)
                .Select(c => new PL_PRODUCTION_PLAN_DETAILS
                {
                    SUBGROUPID = c.SUBGROUPID,
                    OPT1 = $"{c.SUBGROUPNO} - {c.PL_PRODUCTION_SETDISTRIBUTION.FirstOrDefault().PROG_.PROG_NO}"
                }).ToListAsync();

            dyeingProcessRopeViewModel.FPrRopeInfos = await DenimDbContext.F_PR_ROPE_INFO
                .Select(c => new F_PR_ROPE_INFO
                {
                    ID = c.ID,
                    ROPE_NO = c.ROPE_NO
                }).ToListAsync();

            dyeingProcessRopeViewModel.FPrRopeMachineInfos = await DenimDbContext.F_PR_ROPE_MACHINE_INFO
                .Select(c => new F_PR_ROPE_MACHINE_INFO
                {
                    ID = c.ID,
                    ROPE_MC_NO = c.ROPE_MC_NO
                }).ToListAsync();

            dyeingProcessRopeViewModel.FPrTubeInfos = await DenimDbContext.F_PR_TUBE_INFO
                .Select(c => new F_PR_TUBE_INFO
                {
                    ID = c.ID,
                    TUBE_NO = c.TUBE_NO
                }).ToListAsync();

            return dyeingProcessRopeViewModel;
        }

        public async Task<PL_PRODUCTION_PLAN_MASTER> GetGroupDetails(int groupId)
        {
            try
            {
                var result = await DenimDbContext.PL_PRODUCTION_PLAN_MASTER
                    .Include(c => c.PL_PRODUCTION_PLAN_DETAILS)
                    .ThenInclude(c => c.PL_PRODUCTION_SETDISTRIBUTION)
                    .ThenInclude(c => c.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation)
                    .Include(c => c.PL_PRODUCTION_PLAN_DETAILS)
                    .ThenInclude(c => c.PL_PRODUCTION_SETDISTRIBUTION)
                    .ThenInclude(c => c.PROG_.BLK_PROG_.RndProductionOrder.RS)
                    .Include(c => c.PL_PRODUCTION_PLAN_DETAILS)
                    .ThenInclude(c => c.F_PR_WARPING_PROCESS_ROPE_MASTER)
                    .ThenInclude(c => c.F_PR_WARPING_PROCESS_ROPE_DETAILS)
                    //.ThenInclude(c=>c.F_PR_WARPING_PROCESS_ROPE_BALL_DETAILS)
                    //.ThenInclude(c=>c.BALL_ID_FKNavigation)
                    .FirstOrDefaultAsync(c => c.GROUPID.Equals(groupId));
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<PL_PRODUCTION_PLAN_DETAILS> GetProgramNoDetails(int subGroupId)
        {
            return await DenimDbContext.PL_PRODUCTION_PLAN_DETAILS
                .Include(c => c.PL_PRODUCTION_SETDISTRIBUTION)
                .ThenInclude(c => c.PROG_)
                .Include(c => c.F_PR_WARPING_PROCESS_ROPE_MASTER)
                .ThenInclude(c => c.F_PR_WARPING_PROCESS_ROPE_BALL_DETAILS)
                .ThenInclude(c => c.BALL_ID_FKNavigation)
                .Include(c => c.F_PR_WARPING_PROCESS_ROPE_MASTER)
                .ThenInclude(c => c.F_PR_WARPING_PROCESS_ROPE_DETAILS)
                .Include(c => c.F_DYEING_PROCESS_ROPE_DETAILS)
                .ThenInclude(c => c.CAN_NONavigation)
                .FirstOrDefaultAsync(c => c.SUBGROUPID.Equals(subGroupId));
        }

        public async Task<float> GetBallNoDetails(int ballId)
        {
            try
            {
                var result = await DenimDbContext.F_PR_WARPING_PROCESS_ROPE_BALL_DETAILS
                        .FirstOrDefaultAsync(c => c.BALLID.Equals(ballId));

                return float.Parse(result.BALL_LENGTH.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<int> InsertAndGetIdAsync(F_DYEING_PROCESS_ROPE_MASTER fDyeingProcessRopeMaster)
        {
            try
            {
                await DenimDbContext.F_DYEING_PROCESS_ROPE_MASTER.AddAsync(fDyeingProcessRopeMaster);
                await SaveChanges();
                return fDyeingProcessRopeMaster.ROPE_DID;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<FDyeingProcessRopeViewModel> FindAllByIdAsync(int sId)
        {
            try
            {
                var fDyeingProcessRopeMaster = await DenimDbContext.F_DYEING_PROCESS_ROPE_MASTER
                    .Include(c => c.GROUP.PL_PRODUCTION_PLAN_DETAILS)
                    .Include(c => c.F_DYEING_PROCESS_ROPE_CHEM)
                    .ThenInclude(c => c.CHEM_PROD_)
                    .Include(c => c.F_DYEING_PROCESS_ROPE_DETAILS)
                    .ThenInclude(c => c.R_MACHINE_NONavigation)
                    .Include(c => c.F_DYEING_PROCESS_ROPE_DETAILS)
                    .ThenInclude(c => c.BALL.BALL_ID_FKNavigation)
                    .Include(c => c.F_DYEING_PROCESS_ROPE_DETAILS)
                    .ThenInclude(c => c.BALL.BALL_ID_FK_Link)
                    .Include(c => c.F_DYEING_PROCESS_ROPE_DETAILS)
                    .ThenInclude(c => c.CAN_NONavigation)
                    .Include(c => c.F_DYEING_PROCESS_ROPE_DETAILS)
                    .ThenInclude(c => c.ROPE_NONavigation)
                    .Include(c => c.F_DYEING_PROCESS_ROPE_DETAILS)
                    .ThenInclude(c => c.SUBGROUP.PL_PRODUCTION_SETDISTRIBUTION)
                    .ThenInclude(c => c.PROG_)
                    .Where(c => c.ROPE_DID.Equals(sId))
                    .Select(e => new FDyeingProcessRopeViewModel
                    {
                        FDyeingProcessRopeMaster = new F_DYEING_PROCESS_ROPE_MASTER
                        {
                            ROPE_DID = e.ROPE_DID,
                            TRNSDATE = e.TRNSDATE,
                            GROUPID = e.GROUPID,
                            GROUP_LENGTH = e.GROUP_LENGTH,
                            DYEING_LENGTH = e.DYEING_LENGTH,
                            DYEING_CODE = e.DYEING_CODE,
                            REMARKS = e.REMARKS,
                            OPT1 = e.OPT1,
                            OPT2 = e.OPT2,
                            OPT3 = e.OPT3,
                            OPT4 = e.OPT4,
                            OPT5 = e.OPT5,
                            GROUP = e.GROUP
                        },
                        FDyeingProcessRopeChemList = e.F_DYEING_PROCESS_ROPE_CHEM.ToList(),
                        FDyeingProcessRopeDetailsList = e.F_DYEING_PROCESS_ROPE_DETAILS.Select(f => new F_DYEING_PROCESS_ROPE_DETAILS
                        {
                            ROPEID = f.ROPEID,
                            ROPE_DID = f.ROPE_DID,
                            SUBGROUPID = f.SUBGROUPID,
                            SUBGROUP = new PL_PRODUCTION_PLAN_DETAILS
                            {
                                OPT1 = $"{f.SUBGROUP.SUBGROUPNO} - {f.SUBGROUP.PL_PRODUCTION_SETDISTRIBUTION.FirstOrDefault(g => !g.PROG_.PROG_NO.Contains("-")).PROG_.PROG_NO}"
                            },
                            CLOSE_STATUS = f.CLOSE_STATUS,
                            BALLID = f.BALLID,
                            BALL = new F_PR_WARPING_PROCESS_ROPE_BALL_DETAILS
                            {
                                BALL_ID_FKNavigation = new F_BAS_BALL_INFO
                                {
                                    BALL_NO = $"{f.BALL.BALL_ID_FKNavigation.BALL_NO}"
                                }
                            },
                            ROPE_NO = f.ROPE_NO,
                            ROPE_NONavigation = new F_PR_ROPE_INFO
                            {
                                ROPE_NO = $"{f.ROPE_NONavigation.ROPE_NO}"
                            },
                            CAN_NO = f.CAN_NO,
                            CAN_NONavigation = new F_PR_TUBE_INFO
                            {
                                TUBE_NO = $"{f.CAN_NONavigation.TUBE_NO}"
                            },
                            BALL_LENGTH = f.BALL_LENGTH,
                            DYEING_LENGTH = e.DYEING_LENGTH,
                            REJECTION = f.REJECTION,
                            STOP_MARK = f.STOP_MARK,
                            R_MACHINE_NO = f.R_MACHINE_NO,
                            R_MACHINE_NONavigation = new F_PR_ROPE_MACHINE_INFO
                            {
                                ROPE_MC_NO = $"{f.R_MACHINE_NONavigation.ROPE_MC_NO}"
                            },
                            SHIFT = f.SHIFT,
                            REMARKS = f.REMARKS
                        }).ToList()
                    }).FirstOrDefaultAsync();

                return fDyeingProcessRopeMaster;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<FDyeingProcessRopeViewModel> GetGroupNumbersByAsync(FDyeingProcessRopeViewModel fDyeingProcessRopeViewModel, string search, int page)
        {
            // THE USER HAS TYPED IN THE SEARCH BOX
            if (!string.IsNullOrEmpty(search))
            {
                // IN EDIT MODE, WE HAVE TO SKIP THE SELECTED FIELD
                if (!string.IsNullOrEmpty(fDyeingProcessRopeViewModel.FDyeingProcessRopeMaster.EncryptedId))
                {
                    var fDyeingProcessRopeMaster = await DenimDbContext.F_DYEING_PROCESS_ROPE_MASTER.FirstOrDefaultAsync(e => e.ROPE_DID.Equals(int.Parse(_protector.Unprotect(fDyeingProcessRopeViewModel.FDyeingProcessRopeMaster.EncryptedId))));

                    fDyeingProcessRopeViewModel.PlProductionPlanMasterList = await DenimDbContext.PL_PRODUCTION_PLAN_MASTER
                        .Include(c => c.PL_PRODUCTION_PLAN_DETAILS)
                        .ThenInclude(c => c.F_PR_WARPING_PROCESS_ROPE_MASTER)
                        .Include(c => c.RND_DYEING_TYPE)
                        .Where(c => !DenimDbContext.F_DYEING_PROCESS_ROPE_MASTER
                                        .Any(e => !e.GROUPID.Equals(fDyeingProcessRopeMaster.GROUPID) && e.GROUPID.Equals(c.GROUPID)) &&
                                    c.GROUP_NO.ToString().ToLower().Contains(search.ToLower()) &&
                                    c.PL_PRODUCTION_PLAN_DETAILS.All(e => e.F_PR_WARPING_PROCESS_ROPE_MASTER.All(f => f.IS_DECLARE)) &&
                                    c.RND_DYEING_TYPE.DTYPE.ToLower().Equals("rope"))
                        .OrderByDescending(e => e.GROUP_NO)
                        .Select(c => new PL_PRODUCTION_PLAN_MASTER
                        {
                            GROUPID = c.GROUPID,
                            GROUP_NO = c.GROUP_NO
                        }).ToListAsync();
                }
                else // OTHERWISE DO BELOW
                {
                    fDyeingProcessRopeViewModel.PlProductionPlanMasterList = await DenimDbContext.PL_PRODUCTION_PLAN_MASTER
                        .Include(c => c.PL_PRODUCTION_PLAN_DETAILS)
                        .ThenInclude(c => c.F_PR_WARPING_PROCESS_ROPE_MASTER)
                        .Include(c => c.RND_DYEING_TYPE)
                        .Where(c => !DenimDbContext.F_DYEING_PROCESS_ROPE_MASTER.Any(e => e.GROUPID.Equals(c.GROUPID)) &&
                                    c.GROUP_NO.ToString().ToLower().Contains(search.ToLower()) &&
                                    c.PL_PRODUCTION_PLAN_DETAILS.All(e => e.F_PR_WARPING_PROCESS_ROPE_MASTER.All(f => f.IS_DECLARE)) &&
                                    c.RND_DYEING_TYPE.DTYPE.ToLower().Equals("rope"))
                        .OrderByDescending(e => e.GROUP_NO)
                        .Select(c => new PL_PRODUCTION_PLAN_MASTER
                        {
                            GROUPID = c.GROUPID,
                            GROUP_NO = c.GROUP_NO
                        }).ToListAsync();
                }

            }
            else
            {
                // IN EDIT MODE, USER JUST EXPANDED THE DROPDOWN BOX
                if (!string.IsNullOrEmpty(fDyeingProcessRopeViewModel.FDyeingProcessRopeMaster.EncryptedId))
                {
                    var fDyeingProcessRopeMaster = await DenimDbContext.F_DYEING_PROCESS_ROPE_MASTER.FirstOrDefaultAsync(e => e.ROPE_DID.Equals(int.Parse(_protector.Unprotect(fDyeingProcessRopeViewModel.FDyeingProcessRopeMaster.EncryptedId))));

                    fDyeingProcessRopeViewModel.PlProductionPlanMasterList = await DenimDbContext.PL_PRODUCTION_PLAN_MASTER
                        .Include(c => c.PL_PRODUCTION_PLAN_DETAILS)
                        .ThenInclude(c => c.F_PR_WARPING_PROCESS_ROPE_MASTER)
                        .Include(c => c.RND_DYEING_TYPE)
                        .Where(c => !DenimDbContext.F_DYEING_PROCESS_ROPE_MASTER
                                        .Any(e => !e.GROUPID.Equals(fDyeingProcessRopeMaster.GROUPID) && e.GROUPID.Equals(c.GROUPID)) &&
                                    c.PL_PRODUCTION_PLAN_DETAILS.All(e => e.F_PR_WARPING_PROCESS_ROPE_MASTER.All(f => f.IS_DECLARE)) &&
                                    c.RND_DYEING_TYPE.DTYPE.ToLower().Equals("rope"))
                        .OrderByDescending(e => e.GROUP_NO)
                        .Select(c => new PL_PRODUCTION_PLAN_MASTER
                        {
                            GROUPID = c.GROUPID,
                            GROUP_NO = c.GROUP_NO
                        }).ToListAsync();
                }
                else // USER JUST EXPANDED THE DROPDOWN BOX
                {
                    fDyeingProcessRopeViewModel.PlProductionPlanMasterList = await DenimDbContext.PL_PRODUCTION_PLAN_MASTER
                        .Include(c => c.PL_PRODUCTION_PLAN_DETAILS)
                        .ThenInclude(c => c.F_PR_WARPING_PROCESS_ROPE_MASTER)
                        .Include(c => c.RND_DYEING_TYPE)
                        .Where(c => !DenimDbContext.F_DYEING_PROCESS_ROPE_MASTER.Any(e => e.GROUPID.Equals(c.GROUPID)) &&
                                    c.PL_PRODUCTION_PLAN_DETAILS.All(e => e.F_PR_WARPING_PROCESS_ROPE_MASTER.All(f => f.IS_DECLARE)) &&
                                    c.RND_DYEING_TYPE.DTYPE.ToLower().Equals("rope"))
                        .OrderByDescending(e => e.GROUP_NO)
                        .Select(c => new PL_PRODUCTION_PLAN_MASTER
                        {
                            GROUPID = c.GROUPID,
                            GROUP_NO = c.GROUP_NO
                        }).ToListAsync();
                }

            }

            return fDyeingProcessRopeViewModel;
        }

        public async Task<DashboardViewModel> GetDyeingDateWiseLength()
        {
            try
            {
                var dashboardViewModel = new DashboardViewModel
                {
                    DyeingChartDataViewModel = new DyeingChartDataViewModel
                    {
                        RopeDyeing = await DenimDbContext.F_DYEING_PROCESS_ROPE_MASTER
                        .Where(c => c.TRNSDATE.Equals(Convert.ToDateTime("2022-05-07 00:00:00.000").Date))
                        .Select(d => new F_DYEING_PROCESS_ROPE_MASTER()
                        {
                            DYEING_LENGTH = d.DYEING_LENGTH
                        }).SumAsync(c => c.DYEING_LENGTH ?? 0),
                        SlasherDyeing = await DenimDbContext.F_PR_SLASHER_DYEING_MASTER
                        .Where(c => (c.TRNSDATE ?? default).ToString("yyyy-MM-dd").Equals("2022-05-07"))
                        .Select(d => new F_PR_SLASHER_DYEING_MASTER()
                        {
                            TOTAL_ENDS = d.TOTAL_ENDS
                        }).SumAsync(c => c.TOTAL_ENDS ?? 0)
                    }
                };
                return dashboardViewModel;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public async Task<List<ChartViewModel>> GetDyeingDateWiseLengthGraph()
        {
            try
            {
                var data = new List<ChartViewModel>();

                var date = Convert.ToDateTime("2022-05-09");


                for (var i = 0; i < 5; i++)
                {
                    data.Add(new ChartViewModel
                    {
                        type = date.AddDays(i).ToString("yyyy-MM-dd"),
                        visits = await DenimDbContext.F_DYEING_PROCESS_ROPE_MASTER
                            .Where(c => c.TRNSDATE.Equals(date.AddDays(i).Date))
                            .Select(d => new F_DYEING_PROCESS_ROPE_MASTER()
                            {
                                DYEING_LENGTH = d.DYEING_LENGTH
                            }).SumAsync(c => c.DYEING_LENGTH ?? 0) + await DenimDbContext.F_PR_SLASHER_DYEING_MASTER
                            .Where(c => (c.TRNSDATE ?? default).ToString("yyyy-MM-dd").Equals(date.AddDays(i)))
                            .Select(d => new F_PR_SLASHER_DYEING_MASTER()
                            {
                                TOTAL_ENDS = d.TOTAL_ENDS
                            }).SumAsync(c => c.TOTAL_ENDS ?? 0)
                    });
                }
                

                return data;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}