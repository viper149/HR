using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;
using DenimERP.ViewModels.Factory.Fabric_Store;
using Microsoft.EntityFrameworkCore;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLF_FS_FABRIC_RCV_DETAILS_Repository : BaseRepository<F_FS_FABRIC_RCV_DETAILS>, IF_FS_FABRIC_RCV_DETAILS
    {
        public SQLF_FS_FABRIC_RCV_DETAILS_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }

        //public async Task<FFsRollReceiveViewModel> GetRollsAsync(DateTime rcvDate)
        //{
        //    try
        //    {
        //        var fFsRollReceiveViewModel = new FFsRollReceiveViewModel
        //        {
        //            FFsFabricRcvDetailsList = await _denimDbContext.F_PR_INSPECTION_PROCESS_DETAILS
        //                .Include(c=>c.INSP.SET.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation)
        //                .Where(c => c.CREATED_AT.Equals(rcvDate.Date) && !_denimDbContext.F_FS_FABRIC_RCV_DETAILS.Any(e => e.ROLL_ID.Equals(c.ROLL_ID)))
        //                .Select(c=>new F_FS_FABRIC_RCV_DETAILS
        //                {
        //                    ROLL_ = c
        //                }).ToListAsync()
        //        };

        //        var piDetails =await _denimDbContext.COM_EX_PI_DETAILS
        //            .Include(c => c.PIMASTER)
        //            .ThenInclude(c => c.BUYER)
        //            .Include(c => c.STYLE)
        //            .ThenInclude(c => c.FABCODENavigation)
        //            .ThenInclude(c => c.WV).ToListAsync();

        //        fFsRollReceiveViewModel.FFsFabricRcvDetailsList = fFsRollReceiveViewModel.FFsFabricRcvDetailsList
        //            .GroupJoin(piDetails,
        //                f1 => f1.ROLL_.INSP.SET.PROG_.BLK_PROG_.RndProductionOrder.ORDERNO,
        //                f2 => f2.TRNSID,
        //                (f1, f2) => new F_FS_FABRIC_RCV_DETAILS
        //                {
        //                    FABCODE = f2.FirstOrDefault().STYLE.FABCODE,
        //                    FABCODENavigation = _denimDbContext.RND_FABRICINFO
        //                        .Include(c=>c.WV)
        //                        .FirstOrDefault(c => c.FABCODE.Equals(f2.FirstOrDefault().STYLE.FABCODE)),
        //                    SO_NO = f2.FirstOrDefault().TRNSID,
        //                    SO_NONavigation = _denimDbContext.COM_EX_PI_DETAILS
        //                        .Include(c=>c.PIMASTER)
        //                        .FirstOrDefault(c => c.TRNSID.Equals(f2.FirstOrDefault().TRNSID)),
        //                    PO_NO = f2.FirstOrDefault().PIID,
        //                    ROLL_ID = f1.ROLL_.ROLL_ID,
        //                    QTY_YARDS = f1.ROLL_.LENGTH_YDS,
        //                    REMARKS = f1.ROLL_.REMARKS,
        //                    ROLL_ = f1.ROLL_
        //                }).ToList();

        //        return fFsRollReceiveViewModel;
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e);
        //        throw;
        //    }
        //}



        public async Task<FFsRollReceiveViewModel> GetRollsAsync(DateTime rcvDate)
        {
            try
            {
                var fFsRollReceiveViewModel = new FFsRollReceiveViewModel
                {
                    FFsFabricRcvDetailsList = await DenimDbContext.F_PR_INSPECTION_PROCESS_DETAILS
                        .Include(c => c.INSP.SET.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation)
                        .Include(c => c.INSP.SET.PROG_.BLK_PROG_.RndProductionOrder.SO.PIMASTER)
                        .Include(c=>c.PROCESS_TYPENavigation)
                        .Where(c => c.ROLL_INSPDATE.Equals(rcvDate.Date) && c.PROCESS_TYPE!=11 && c.PROCESS_TYPE!=13 && c.ROLLNO.Length>7 && !DenimDbContext.F_FS_FABRIC_RCV_DETAILS.Any(e => e.ROLL_ID.Equals(c.ROLL_ID)))
                        .Select(c => new F_FS_FABRIC_RCV_DETAILS
                        {
                            FABCODE = c.INSP.SET.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.FABCODE,
                            FABCODENavigation = new RND_FABRICINFO
                            {
                                STYLE_NAME = c.INSP.SET.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.STYLE_NAME
                            },
                            SO_NO = c.INSP.SET.PROG_.BLK_PROG_.RndProductionOrder.SO.TRNSID,
                            SO_NONavigation = new COM_EX_PI_DETAILS
                            {
                                SO_NO = c.INSP.SET.PROG_.BLK_PROG_.RndProductionOrder.SO.SO_NO,
                                PIMASTER = new COM_EX_PIMASTER
                                {
                                    PINO = c.INSP.SET.PROG_.BLK_PROG_.RndProductionOrder.SO.PIMASTER.PINO
                                }
                            },
                            PO_NO = c.INSP.SET.PROG_.BLK_PROG_.RndProductionOrder.SO.PIMASTER.PIID,
                            ROLL_ID = c.ROLL_ID,
                            QTY_YARDS = c.LENGTH_YDS,
                            REMARKS = c.REMARKS,
                            ROLL_ = new F_PR_INSPECTION_PROCESS_DETAILS
                            {
                                ROLLNO = c.ROLLNO,
                                LENGTH_YDS = c.LENGTH_YDS,
                                FAB_GRADE = c.FAB_GRADE,
                                REMARKS = c.REMARKS,
                                OPT1 = c.PROCESS_TYPENavigation==null?"": c.PROCESS_TYPENavigation.NAME
                            }
                        }).ToListAsync()
                };

                //var piDetails = await _denimDbContext.COM_EX_PI_DETAILS
                //    .Include(c => c.PIMASTER)
                //    .ThenInclude(c => c.BUYER)
                //    .Include(c => c.STYLE)
                //    .ThenInclude(c => c.FABCODENavigation)
                //    .ThenInclude(c => c.WV).ToListAsync();

                //fFsRollReceiveViewModel.FFsFabricRcvDetailsList = fFsRollReceiveViewModel.FFsFabricRcvDetailsList
                //    .GroupJoin(piDetails,
                //        f1 => f1.ROLL_.INSP.SET.PROG_.BLK_PROG_.RndProductionOrder.ORDERNO,
                //        f2 => f2.TRNSID,
                //        (f1, f2) => new F_FS_FABRIC_RCV_DETAILS
                //        {
                //            FABCODE = f2.FirstOrDefault().STYLE.FABCODE,
                //            FABCODENavigation = _denimDbContext.RND_FABRICINFO
                //                .Include(c => c.WV)
                //                .FirstOrDefault(c => c.FABCODE.Equals(f2.FirstOrDefault().STYLE.FABCODE)),
                //            SO_NO = f2.FirstOrDefault().TRNSID,
                //            SO_NONavigation = _denimDbContext.COM_EX_PI_DETAILS
                //                .Include(c => c.PIMASTER)
                //                .FirstOrDefault(c => c.TRNSID.Equals(f2.FirstOrDefault().TRNSID)),
                //            PO_NO = f2.FirstOrDefault().PIID,
                //            ROLL_ID = f1.ROLL_.ROLL_ID,
                //            QTY_YARDS = f1.ROLL_.LENGTH_YDS,
                //            REMARKS = f1.ROLL_.REMARKS,
                //            ROLL_ = f1.ROLL_
                //        }).ToList();

                return fFsRollReceiveViewModel;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<F_FS_FABRIC_RCV_DETAILS> FindRollDetails(int rollId, DateTime rcvDate)
        {
            try
            {
                var getRollIDetails = await DenimDbContext.F_PR_INSPECTION_PROCESS_DETAILS
                    .Where(c => c.ROLL_INSPDATE.Equals(rcvDate) &&
                                c.ROLL_ID.Equals(rollId) &&
                                !DenimDbContext.F_FS_FABRIC_RCV_DETAILS.Any(e => e.ROLL_ID.Equals(c.ROLL_ID)))
                    .Select(c => new F_FS_FABRIC_RCV_DETAILS
                    {
                        ROLL_ID = c.ROLL_ID
                    }).FirstOrDefaultAsync();

                return getRollIDetails;
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
                var getRollIDetails = await DenimDbContext.F_PR_INSPECTION_PROCESS_DETAILS
                    .Where(c=>c.ROLLNO.Equals(rollNo))
                    .Select(c => new F_FS_FABRIC_RCV_DETAILS
                    {
                        ROLL_ID = c.ROLL_ID
                    }).FirstOrDefaultAsync();

                return getRollIDetails;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }

        public async Task<F_FS_FABRIC_RCV_DETAILS> GetRcvRollIdByRollNo(string rollNo)
        {
            try
            {
                var getRollIDetails = await DenimDbContext.F_FS_FABRIC_RCV_DETAILS
                    .Include(c=>c.ROLL_)
                    .Where(c => c.ROLL_.ROLLNO.Equals(rollNo))
                    .Select(c => new F_FS_FABRIC_RCV_DETAILS
                    {
                        TRNSID = c.TRNSID
                    }).FirstOrDefaultAsync();

                return getRollIDetails;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<FFsRollReceiveViewModel> GetRollDetailsList(FFsRollReceiveViewModel fFsRollReceiveViewModel)
        {
            try
            {
                foreach (var item in fFsRollReceiveViewModel.FFsFabricRcvDetailsList)
                {
                    var roll = await DenimDbContext.F_PR_INSPECTION_PROCESS_DETAILS
                    .Include(c => c.INSP.SET.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation)
                    .Include(c => c.INSP.SET.PROG_.BLK_PROG_.RndProductionOrder.SO.PIMASTER)
                    .Where(c => c.ROLL_ID.Equals(item.ROLL_ID))
                    .Select(c => new F_FS_FABRIC_RCV_DETAILS
                    {
                        FABCODE = c.INSP.SET.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.FABCODE,
                        FABCODENavigation = new RND_FABRICINFO
                        {
                            STYLE_NAME = c.INSP.SET.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation
                                .STYLE_NAME
                        },
                        SO_NO = c.INSP.SET.PROG_.BLK_PROG_.RndProductionOrder.SO.TRNSID,
                        SO_NONavigation = new COM_EX_PI_DETAILS
                        {
                            SO_NO = c.INSP.SET.PROG_.BLK_PROG_.RndProductionOrder.SO.SO_NO,
                            PIMASTER = new COM_EX_PIMASTER
                            {
                                PINO = c.INSP.SET.PROG_.BLK_PROG_.RndProductionOrder.SO.PIMASTER.PINO
                            }
                        },
                        PO_NO = c.INSP.SET.PROG_.BLK_PROG_.RndProductionOrder.SO.PIMASTER.PIID,
                        ROLL_ID = c.ROLL_ID,
                        QTY_YARDS = c.LENGTH_YDS,
                        REMARKS = c.REMARKS,
                        ROLL_ = new F_PR_INSPECTION_PROCESS_DETAILS
                        {
                            ROLLNO = c.ROLLNO,
                            LENGTH_YDS = c.LENGTH_YDS,
                            FAB_GRADE = c.FAB_GRADE,
                            REMARKS = c.REMARKS
                        }
                    }).FirstOrDefaultAsync();

                    item.FABCODE = roll.FABCODE;
                    item.FABCODENavigation = roll.FABCODENavigation;
                    item.SO_NO = roll.SO_NO;
                    item.SO_NONavigation = roll.SO_NONavigation;
                    item.PO_NO = roll.PO_NO;
                    item.ROLL_ID = roll.ROLL_ID;
                    item.QTY_YARDS = roll.QTY_YARDS;
                    item.REMARKS = roll.REMARKS;
                    item.ROLL_ = roll.ROLL_;
                    item.LOCATIONNavigation = await DenimDbContext.F_FS_LOCATION.Where(c => c.ID.Equals(item.LOCATION)).Select(c => new F_FS_LOCATION
                        {
                            LOCATION = c.LOCATION
                        })
                        .FirstOrDefaultAsync();
                }
                return fFsRollReceiveViewModel;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<FFsRollReceiveViewModel> GetRollsByScanAsync(FFsRollReceiveViewModel fFsRollReceiveViewModel)
        {
            try
            {
                if (!fFsRollReceiveViewModel.FFsFabricRcvDetailsList.Any(c =>
                    c.ROLL_ID.Equals(fFsRollReceiveViewModel.FFsFabricRcvDetails.ROLL_ID)))
                {
                    var getRollIDetails = await DenimDbContext.F_PR_INSPECTION_PROCESS_DETAILS
                        .Where(c => c.ROLL_INSPDATE.Equals(fFsRollReceiveViewModel.FFsFabricRcvMaster.RCVDATE) &&
                                    c.ROLL_ID.Equals(fFsRollReceiveViewModel.FFsFabricRcvDetails.ROLL_ID) &&
                                    !DenimDbContext.F_FS_FABRIC_RCV_DETAILS.Any(e => e.ROLL_ID.Equals(c.ROLL_ID)))
                        .Select(c => new F_FS_FABRIC_RCV_DETAILS
                        {
                            ROLL_ID = c.ROLL_ID,
                            LOCATION = fFsRollReceiveViewModel.FFsFabricRcvDetails.LOCATION
                        }).FirstOrDefaultAsync();
                    if (getRollIDetails != null)
                    {
                        fFsRollReceiveViewModel.FFsFabricRcvDetailsList.Add(getRollIDetails);
                    }
                }
                return fFsRollReceiveViewModel;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }


        public async Task<IEnumerable<F_FS_FABRIC_RCV_DETAILS>> GetRollListAsync()
        {
            try
            {
                var fFsRollReceiveViewModel = await DenimDbContext.F_FS_FABRIC_RCV_DETAILS
                    .Include(c => c.RCV)
                    .Include(c => c.FABCODENavigation)
                    .ThenInclude(c => c.WV)
                    .Include(c => c.PO_NONavigation)
                    .Include(c => c.SO_NONavigation)
                    .Include(c => c.ROLL_)
                    .ThenInclude(c => c.INSP)
                    .OrderBy(c => c.RCV.RCVDATE)
                    .ThenBy(c => c.IS_QC_APPROVE)
                    .ThenBy(c => c.IS_QC_REJECT)
                    .ToListAsync();

                foreach (var item in fFsRollReceiveViewModel)
                {
                    item.FABCODENavigation.WV.RND_FABRICINFO = null;
                    item.RCV.F_FS_FABRIC_RCV_DETAILS = null;
                    item.PO_NONavigation.F_FS_FABRIC_RCV_DETAILS = null;
                    item.SO_NONavigation.F_FS_FABRIC_RCV_DETAILS = null;
                    item.ROLL_.INSP.F_PR_INSPECTION_PROCESS_DETAILS = null;
                    item.ROLL_.F_FS_FABRIC_RCV_DETAILS = null;
                    if (item.IS_QC_APPROVE)
                    {
                        item.OPT1 = "Approved";
                    }
                    else if (item.IS_QC_REJECT)
                    {
                        item.OPT1 = "Rejected";
                    }
                    else
                    {
                        item.OPT1 = "Pending";
                    }
                }

                return fFsRollReceiveViewModel;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<F_FS_FABRIC_RCV_DETAILS> GetRollIDetails(int rollId)
        {
            try
            {
                var rollDetails = await DenimDbContext.F_FS_FABRIC_RCV_DETAILS
                    .Where(c => c.ROLL_ID.Equals(rollId))
                    .FirstOrDefaultAsync();
                return rollDetails;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<bool> GetRollBalance(int rollId, double fullLength)
        {
            try
            {

                var rollDetails = await DenimDbContext.F_FS_FABRIC_RCV_DETAILS.FirstOrDefaultAsync(c => c.ROLL_ID.Equals(rollId));

                return rollDetails.BALANCE_QTY > fullLength;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

    }
}
