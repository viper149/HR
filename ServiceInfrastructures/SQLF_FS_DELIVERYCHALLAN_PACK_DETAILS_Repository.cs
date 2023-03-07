using System;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;
using DenimERP.ViewModels;
using DenimERP.ViewModels.Factory.Fabric_Store;
using DenimERP.ViewModels.Home;
using Microsoft.EntityFrameworkCore;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLF_FS_DELIVERYCHALLAN_PACK_DETAILS_Repository : BaseRepository<F_FS_DELIVERYCHALLAN_PACK_DETAILS>, IF_FS_DELIVERYCHALLAN_PACK_DETAILS
    {
        public SQLF_FS_DELIVERYCHALLAN_PACK_DETAILS_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }

        public async Task<FabDeliveryChallanViewModel> GetRollDetailsAsync(FabDeliveryChallanViewModel fabDeliveryChallanViewModel)
        {
            try
            {
                foreach (var item in fabDeliveryChallanViewModel.FFsDeliverychallanPackDetailsList)
                {
                    item.ROLL = await DenimDbContext.F_FS_FABRIC_RCV_DETAILS
                        .Include(c => c.ROLL_)
                        .Where(c => c.TRNSID.Equals(item.ROLL_NO)).FirstOrDefaultAsync();
                }
                return fabDeliveryChallanViewModel;

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<FabDeliveryChallanViewModel> GetRollsByScanAsync(FabDeliveryChallanViewModel fabDeliveryChallanViewModel)
        {
            try
            {
                if (!fabDeliveryChallanViewModel.FFsDeliverychallanPackDetailsList.Any(c =>
                    c.ROLL_NO.Equals(fabDeliveryChallanViewModel.FFsDeliverychallanPackDetails.ROLL_NO)))
                {
                    var getRollIDetails = await DenimDbContext
                        .F_FS_FABRIC_RCV_DETAILS
                        .Include(c => c.ROLL_.F_FS_FABRIC_CLEARANCE_DETAILS)
                        .ThenInclude(c => c.CL)
                        .Include(c => c.LOCATIONNavigation)
                        .Where(c =>
                            c.TRNSID.Equals(fabDeliveryChallanViewModel.FFsDeliverychallanPackDetails.ROLL_NO) &&
                            !DenimDbContext.F_FS_DELIVERYCHALLAN_PACK_DETAILS.Any(d => d.ROLL_NO.Equals(c.TRNSID)))
                        .Select(c => new F_FS_DELIVERYCHALLAN_PACK_DETAILS
                        {
                            ROLL_NO = c.TRNSID,
                            PACKING_LIST_ID = fabDeliveryChallanViewModel.FFsDeliverychallanPackDetails.PACKING_LIST_ID,
                            LENGTH1 = c.ROLL_.LENGTH_1,
                            LENGTH2 = c.ROLL_.LENGTH_2,
                            ROLL = new F_FS_FABRIC_RCV_DETAILS
                            {
                                TRNSID = c.TRNSID,
                                ROLL_ = new F_PR_INSPECTION_PROCESS_DETAILS
                                {
                                    ROLL_ID = c.ROLL_.ROLL_ID,
                                    FAB_GRADE = c.ROLL_.FAB_GRADE,
                                    OPT1 = c.ROLL_.F_FS_FABRIC_CLEARANCE_DETAILS
                                        .Where(cd => cd.ROLL_ID.Equals(c.ROLL_.ROLL_ID)).Select(ce => ce.SHADE_GROUP)
                                        .FirstOrDefault(),
                                    OPT2 = c.ROLL_.F_FS_FABRIC_CLEARANCE_DETAILS
                                        .Where(d => d.ROLL_ID.Equals(c.ROLL_.ROLL_ID))
                                        .Select(d => d.CL.WASH_CODE.ToString()).FirstOrDefault(),
                                },
                                LOCATIONNavigation = new F_FS_LOCATION
                                {
                                    LOCATION = c.LOCATIONNavigation.LOCATION
                                }
                            }

                        }).FirstOrDefaultAsync();
                    if (getRollIDetails != null)
                    {
                        fabDeliveryChallanViewModel.FFsDeliverychallanPackDetailsList.Add(getRollIDetails);
                    }
                }



                return fabDeliveryChallanViewModel;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }


        public async Task<FabDeliveryChallanViewModel> GetRollDetails(FabDeliveryChallanViewModel fabDeliveryChallanViewModel)
        {
            try
            {
                foreach (var item in fabDeliveryChallanViewModel.FFsDeliverychallanPackDetailsList)
                {
                    item.ROLL = await DenimDbContext.F_FS_FABRIC_RCV_DETAILS
                        .Include(c => c.ROLL_)
                        .Include(c => c.LOCATIONNavigation)
                        .Where(c => c.TRNSID.Equals(item.ROLL_NO))
                        .Select(c => new F_FS_FABRIC_RCV_DETAILS
                        {
                            TRNSID = c.TRNSID,
                            ROLL_ = new F_PR_INSPECTION_PROCESS_DETAILS
                            {
                                ROLL_ID = c.ROLL_.ROLL_ID,
                                FAB_GRADE = c.ROLL_.FAB_GRADE
                            },
                            LOCATIONNavigation = new F_FS_LOCATION
                            {
                                LOCATION = c.LOCATIONNavigation.LOCATION
                            }
                        })
                        .FirstOrDefaultAsync();
                }

                return fabDeliveryChallanViewModel;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<FabDeliveryChallanViewModel> GetRollDetailsList(FabDeliveryChallanViewModel fabDeliveryChallanViewModel)
        {
            try
            {
                foreach (var item in fabDeliveryChallanViewModel.FFsDeliverychallanPackDetailsList)
                {
                    var roll = await DenimDbContext.F_FS_FABRIC_RCV_DETAILS
                    .Include(c => c.FABCODENavigation)
                    .Include(c => c.SO_NONavigation)
                    .Include(c => c.PO_NONavigation)
                    .Include(c => c.ROLL_)
                    .Where(c => c.TRNSID.Equals(item.ROLL == null ? 0 : item.ROLL.TRNSID))
                    .FirstOrDefaultAsync();

                    if (roll != null)
                    {
                        item.ROLL.FABCODE = roll.FABCODENavigation.FABCODE;
                        item.ROLL.FABCODENavigation = roll.FABCODENavigation;
                        item.ROLL.SO_NO = roll.SO_NONavigation.TRNSID;
                        item.ROLL.SO_NONavigation = roll.SO_NONavigation;
                        item.ROLL.PO_NO = roll.PO_NONavigation.PIID;
                        item.ROLL.ROLL_ID = roll.ROLL_ID;
                        item.ROLL.QTY_YARDS = roll.QTY_YARDS;
                        item.ROLL.REMARKS = roll.REMARKS;
                        item.ROLL.ROLL_ = roll.ROLL_;
                    }

                    //fabDeliveryChallanViewModel =await GetRollDetails(fabDeliveryChallanViewModel);


                    //var roll = await _denimDbContext.F_PR_INSPECTION_PROCESS_DETAILS
                    //.Include(c => c.INSP.SET.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation)
                    //.Include(c => c.INSP.SET.PROG_.BLK_PROG_.RndProductionOrder.SO.PIMASTER)
                    //.Where(c => c.ROLL_ID.Equals(item.ROLL.ROLL_.ROLL_ID))
                    //.Select(c => new F_FS_DELIVERYCHALLAN_PACK_DETAILS()
                    //{
                    //    ROLL = new F_FS_FABRIC_RCV_DETAILS
                    //    {
                    //        FABCODE = c.INSP.SET.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.FABCODE,
                    //        FABCODENavigation = new RND_FABRICINFO
                    //        {
                    //            STYLE_NAME = c.INSP.SET.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation
                    //                .STYLE_NAME
                    //        },
                    //        SO_NO = c.INSP.SET.PROG_.BLK_PROG_.RndProductionOrder.SO.TRNSID,
                    //        SO_NONavigation = new COM_EX_PI_DETAILS
                    //        {
                    //            SO_NO = c.INSP.SET.PROG_.BLK_PROG_.RndProductionOrder.SO.SO_NO,
                    //            PIMASTER = new COM_EX_PIMASTER
                    //            {
                    //                PINO = c.INSP.SET.PROG_.BLK_PROG_.RndProductionOrder.SO.PIMASTER.PINO
                    //            }
                    //        },
                    //        PO_NO = c.INSP.SET.PROG_.BLK_PROG_.RndProductionOrder.SO.PIMASTER.PIID,
                    //        ROLL_ID = c.ROLL_ID,
                    //        QTY_YARDS = c.LENGTH_YDS,
                    //        REMARKS = c.REMARKS,
                    //        ROLL_ = new F_PR_INSPECTION_PROCESS_DETAILS
                    //        {
                    //            ROLLNO = c.ROLLNO,
                    //            LENGTH_YDS = c.LENGTH_YDS,
                    //            FAB_GRADE = c.FAB_GRADE,
                    //            REMARKS = c.REMARKS
                    //        }
                    //    }

                    //}).FirstOrDefaultAsync();

                }

                return fabDeliveryChallanViewModel;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<F_FS_FABRIC_RCV_DETAILS> GetRollIdByRollNo(string rollNo)
        {
            try
            {
                var getRollIDetails = await DenimDbContext.F_FS_FABRIC_RCV_DETAILS
                    .Include(c => c.ROLL_)
                    .Include(c => c.F_FS_DELIVERYCHALLAN_PACK_DETAILS)
                    .Where(c => c.ROLL_.ROLLNO.ToUpper().Equals(rollNo.ToUpper()))
                    .Select(c => new F_FS_FABRIC_RCV_DETAILS
                    {
                        TRNSID = c.TRNSID,
                        ROLL_ID = c.ROLL_.ROLL_ID,
                        OPT1 = c.ROLL_.LENGTH_YDS.ToString(),
                        F_FS_DELIVERYCHALLAN_PACK_DETAILS = c.F_FS_DELIVERYCHALLAN_PACK_DETAILS,
                        FABCODE = c.FABCODE
                    }).FirstOrDefaultAsync();

                return getRollIDetails;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<F_FS_FABRIC_CLEARANCE_DETAILS> GetRollShadeByRollNo(string rollNo)
        {
            try
            {
                var getRollIDetails = await DenimDbContext.F_FS_FABRIC_CLEARANCE_DETAILS
                    .Include(c => c.ROLL_)
                    .Where(c => c.ROLL_.ROLLNO.ToUpper().Equals(rollNo.ToUpper()) && c.SHADE_GROUP != null && c.STATUS.Equals(1))
                    .Select(c => new F_FS_FABRIC_CLEARANCE_DETAILS
                    {
                        CL_D_ID = c.CL_D_ID,
                        ROLL_ID = c.ROLL_.ROLL_ID
                    }).FirstOrDefaultAsync();

                return getRollIDetails;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }

        public async Task<F_FS_DELIVERYCHALLAN_PACK_DETAILS> GetRollDetailsByRcvid(int rcvId)
        {
            try
            {
                var getRollIDetails = await DenimDbContext.F_FS_DELIVERYCHALLAN_PACK_DETAILS
                    .Where(c => c.ROLL_NO.Equals(rcvId)).FirstOrDefaultAsync();

                return getRollIDetails;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }

        public async Task<DashboardViewModel> GetFabricDeliveryChallanLength()
        {
            try
            {
                var date = Convert.ToDateTime("2022-05-09");
                var dashboardViewModel = new DashboardViewModel
                {
                    FabricDeliveryChallanViewModel = new FabricDeliveryChallanViewModel()
                    {
                        TotalDelivery = await DenimDbContext.F_FS_DELIVERYCHALLAN_PACK_DETAILS
                            .Include(c => c.D_CHALLAN)
                            .Where(c => c.D_CHALLAN.D_CHALLANDATE.Equals(date))
                            .Select(d => new F_PR_INSPECTION_PROCESS_DETAILS()
                            {
                                LENGTH_1 = d.LENGTH1

                            }).SumAsync(c => Convert.ToDouble(c.LENGTH_1 ?? 0)) +
                                        await DenimDbContext.F_FS_DELIVERYCHALLAN_PACK_DETAILS
                            .Include(c => c.D_CHALLAN)
                            .Where(c => c.D_CHALLAN.D_CHALLANDATE.Equals(date))
                            .Select(d => new F_PR_INSPECTION_PROCESS_DETAILS()
                            {
                                LENGTH_2 = d.LENGTH2

                            }).SumAsync(c => Convert.ToDouble(c.LENGTH_2 ?? 0)),


                        TotalDeliveryY = await DenimDbContext.F_FS_DELIVERYCHALLAN_PACK_DETAILS
                                             .Include(c => c.D_CHALLAN)
                                             .Where(c => c.D_CHALLAN.D_CHALLANDATE.Equals(date.AddDays(-1)))
                                             .Select(d => new F_PR_INSPECTION_PROCESS_DETAILS()
                                             {
                                                 LENGTH_1 = d.LENGTH1

                                             }).SumAsync(c => Convert.ToDouble(c.LENGTH_1 ?? 0)) +
                                         await DenimDbContext.F_FS_DELIVERYCHALLAN_PACK_DETAILS
                                             .Include(c => c.D_CHALLAN)
                                             .Where(c => c.D_CHALLAN.D_CHALLANDATE.Equals(date.AddDays(-1)))
                                             .Select(d => new F_PR_INSPECTION_PROCESS_DETAILS()
                                             {
                                                 LENGTH_2 = d.LENGTH2

                                             }).SumAsync(c => Convert.ToDouble(c.LENGTH_2 ?? 0))
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
    }
}
