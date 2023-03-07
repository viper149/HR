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
    public class SQLF_PR_INSPECTION_FABRIC_D_DETAILS_Repository : BaseRepository<F_PR_INSPECTION_FABRIC_D_DETAILS>, IF_PR_INSPECTION_FABRIC_D_DETAILS
    {
        public SQLF_PR_INSPECTION_FABRIC_D_DETAILS_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }

        public async Task<F_PR_INSPECTION_FABRIC_D_DETAILS> FindRollDetails(int rollId, DateTime dDate)
        {
            try
            {
                var getRollIDetails = await DenimDbContext.F_PR_INSPECTION_PROCESS_DETAILS
                    .Where(c => c.ROLL_INSPDATE.Equals(dDate) &&
                                c.ROLL_ID.Equals(rollId) &&
                                !DenimDbContext.F_PR_INSPECTION_FABRIC_D_DETAILS.Any(e => e.ROLL_ID.Equals(c.ROLL_ID)))
                    .Select(c => new F_PR_INSPECTION_FABRIC_D_DETAILS
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

        public async Task<F_PR_INSPECTION_FABRIC_D_DETAILS> GetRcvRollIdByRollNo(string rollNo)
        {
            try
            {
                var getRollIDetails = await DenimDbContext.F_PR_INSPECTION_FABRIC_D_DETAILS
                    .Include(c => c.ROLL_)
                    .Where(c => c.ROLL_.ROLLNO.Equals(rollNo))
                    .Select(c => new F_PR_INSPECTION_FABRIC_D_DETAILS
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

        public async Task<bool> GetRollBalance(int rollId, double fullLength)
        {
            try
            {

                var rollDetails = await DenimDbContext.F_PR_INSPECTION_FABRIC_D_DETAILS.FirstOrDefaultAsync(c => c.ROLL_ID.Equals(rollId));

                return rollDetails.BALANCE_QTY > fullLength;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<FPrInspectionFabricDispatchViewModel> GetRollDetailsList(FPrInspectionFabricDispatchViewModel fPrInspectionFabricDispatchViewModel)
        {
            try
            {
                foreach (var item in fPrInspectionFabricDispatchViewModel.FPrInspectionFabricDDetailsList)
                {
                    var roll = await DenimDbContext.F_PR_INSPECTION_PROCESS_DETAILS
                    .Include(c => c.INSP.SET.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation)
                    .Include(c => c.INSP.SET.PROG_.BLK_PROG_.RndProductionOrder.SO.PIMASTER)
                    .Where(c => c.ROLL_ID.Equals(item.ROLL_ID))
                    .Select(c => new F_PR_INSPECTION_FABRIC_D_DETAILS
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
                            SHIFT = c.SHIFT,
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
                return fPrInspectionFabricDispatchViewModel;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<F_PR_INSPECTION_FABRIC_D_DETAILS> GetRollIdByRollNo(string rollNo)
        {
            try
            {
                var getRollIDetails = await DenimDbContext.F_PR_INSPECTION_PROCESS_DETAILS
                    .Where(c => c.ROLLNO.Equals(rollNo))
                    .Select(c => new F_PR_INSPECTION_FABRIC_D_DETAILS
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

        public async Task<F_PR_INSPECTION_FABRIC_D_DETAILS> GetRollIDetails(int rollId)
        {
            try
            {
                var rollDetails = await DenimDbContext.F_PR_INSPECTION_FABRIC_D_DETAILS
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

        public async Task<IEnumerable<F_PR_INSPECTION_FABRIC_D_DETAILS>> GetRollListAsync()
        {
            try
            {
                var fPrInspectionFabricDispatchViewModel = await DenimDbContext.F_PR_INSPECTION_FABRIC_D_DETAILS
                    .Include(c => c.D)
                    .Include(c => c.FABCODENavigation)
                    .ThenInclude(c => c.WV)
                    .Include(c => c.PO_NONavigation)
                    .Include(c => c.SO_NONavigation)
                    .Include(c => c.ROLL_)
                    .ThenInclude(c => c.INSP)
                    .OrderBy(c => c.D.DDATE)
                    .ThenBy(c => c.IS_QC_APPROVE)
                    .ThenBy(c => c.IS_QC_REJECT)
                    .ToListAsync();

                foreach (var item in fPrInspectionFabricDispatchViewModel)
                {
                    item.FABCODENavigation.WV.RND_FABRICINFO = null;
                    item.D.F_PR_INSPECTION_FABRIC_D_DETAILS = null;
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

                return fPrInspectionFabricDispatchViewModel;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<FPrInspectionFabricDispatchViewModel> GetRollsAsync(DateTime dDate)
        {
            try
            {
                var fPrInspectionFabricDispatchViewModel = new FPrInspectionFabricDispatchViewModel
                {
                    FPrInspectionFabricDDetailsList = await DenimDbContext.F_PR_INSPECTION_PROCESS_DETAILS
                        .Include(c => c.INSP.SET.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation)
                        .Include(c => c.INSP.SET.PROG_.BLK_PROG_.RndProductionOrder.SO.PIMASTER)
                        .Include(c => c.PROCESS_TYPENavigation)
                        .Where(c => c.ROLL_INSPDATE.Equals(dDate.Date) && c.PROCESS_TYPE != 11 && c.PROCESS_TYPE != 13 && c.ROLLNO.Length > 7 && !DenimDbContext.F_FS_FABRIC_RCV_DETAILS.Any(e => e.ROLL_ID.Equals(c.ROLL_ID)))
                        .Select(c => new F_PR_INSPECTION_FABRIC_D_DETAILS
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
                                OPT1 = c.PROCESS_TYPENavigation == null ? "" : c.PROCESS_TYPENavigation.NAME
                            }
                        }).ToListAsync()
                };

                return fPrInspectionFabricDispatchViewModel;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<FPrInspectionFabricDispatchViewModel> GetRollsByScanAsync(FPrInspectionFabricDispatchViewModel fPrInspectionFabricDispatchViewModel)
        {
            try
            {
                if (!fPrInspectionFabricDispatchViewModel.FPrInspectionFabricDDetailsList.Any(c =>
                    c.ROLL_ID.Equals(fPrInspectionFabricDispatchViewModel.FPrInspectionFabricDDetails.ROLL_ID)))
                {
                    var getRollIDetails = await DenimDbContext.F_PR_INSPECTION_PROCESS_DETAILS
                        .Where(c => c.ROLL_INSPDATE.Equals(fPrInspectionFabricDispatchViewModel.FPrInspectionFabricDMaster.DDATE) &&
                                    c.ROLL_ID.Equals(fPrInspectionFabricDispatchViewModel.FPrInspectionFabricDDetails.ROLL_ID) &&
                                    !DenimDbContext.F_PR_INSPECTION_FABRIC_D_DETAILS.Any(e => e.ROLL_ID.Equals(c.ROLL_ID)))
                        .Select(c => new F_PR_INSPECTION_FABRIC_D_DETAILS
                        {
                            ROLL_ID = c.ROLL_ID,
                            LOCATION = fPrInspectionFabricDispatchViewModel.FPrInspectionFabricDDetails.LOCATION
                        }).FirstOrDefaultAsync();
                    if (getRollIDetails != null)
                    {
                        fPrInspectionFabricDispatchViewModel.FPrInspectionFabricDDetailsList.Add(getRollIDetails);
                    }
                }
                return fPrInspectionFabricDispatchViewModel;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
