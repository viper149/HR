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
    public class SQLF_FS_FABRIC_CLEARANCE_DETAILS_Repository:BaseRepository<F_FS_FABRIC_CLEARANCE_DETAILS>, IF_FS_FABRIC_CLEARANCE_DETAILS
    {
        public SQLF_FS_FABRIC_CLEARANCE_DETAILS_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }
        
        public async Task<FFsFabricClearanceViewModel> SetRollStatus(
            FFsFabricClearanceViewModel fFsFabricClearanceViewModel)
        {
            try
            {
                foreach (var item in fFsFabricClearanceViewModel.FFsFabricClearanceDetailsList)
                {
                    item.ROLL_ = await DenimDbContext.F_PR_INSPECTION_PROCESS_DETAILS
                        .Where(c => c.ROLL_ID.Equals(item.ROLL_ID)).FirstOrDefaultAsync();
                    item.STATUS = fFsFabricClearanceViewModel.FFsFabricClearanceDetails.STATUS;
                }

                return fFsFabricClearanceViewModel;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<FFsFabricClearanceViewModel> GetRollDetailsAsync(
            FFsFabricClearanceViewModel fFsFabricClearanceViewModel)
        {
            try
            {
                var dateFrom = fFsFabricClearanceViewModel.FFsFabricClearanceMaster.DATE_FROM;
                var dateTo = fFsFabricClearanceViewModel.FFsFabricClearanceMaster.DATE_TO;

                fFsFabricClearanceViewModel.FFsFabricClearanceMaster.FABCODENavigation = await DenimDbContext
                    .RND_FABRICINFO
                    .Where(c => c.FABCODE.Equals(fFsFabricClearanceViewModel.FFsFabricClearanceMaster.FABCODE))
                    .FirstOrDefaultAsync();


                fFsFabricClearanceViewModel.FFsFabricClearanceMaster.PO = await DenimDbContext
                    .RND_PRODUCTION_ORDER
                    .Include(c => c.SO)
                    .Where(c => c.POID.Equals(fFsFabricClearanceViewModel.FFsFabricClearanceMaster.ORDER_NO))
                    .FirstOrDefaultAsync();

                var x = await DenimDbContext.F_PR_INSPECTION_PROCESS_DETAILS
                    .Include(c => c.INSP)
                    .ThenInclude(c => c.SET.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation)
                    .Where(c => c.ROLL_INSPDATE >= dateFrom && c.ROLL_INSPDATE <= dateTo && c.FAB_GRADE == "A" && !DenimDbContext.F_FS_FABRIC_CLEARANCE_DETAILS.Any(d => d.ROLL_ID.Equals(c.ROLL_ID)) && (c.INSP.SET.PROG_.BLK_PROG_.RndProductionOrder.POID.Equals(fFsFabricClearanceViewModel.FFsFabricClearanceMaster.ORDER_NO) || c.OPT3.Equals(fFsFabricClearanceViewModel.FFsFabricClearanceMaster.PO.SO.SO_NO)))
                    
                    .OrderBy(c=>c.ROLLNO)
                    .ToListAsync();


                if (fFsFabricClearanceViewModel.FFsFabricClearanceMaster.OPT1 != null &&
                    fFsFabricClearanceViewModel.FFsFabricClearanceMaster.OPT2 != null)
                {
                    x = x.Where(c => string.Compare(c.ROLLNO, fFsFabricClearanceViewModel.FFsFabricClearanceMaster.OPT1, StringComparison.Ordinal) >= 0 && string.Compare(c.ROLLNO, fFsFabricClearanceViewModel.FFsFabricClearanceMaster.OPT2, StringComparison.Ordinal) <= 0).ToList();
                }


                fFsFabricClearanceViewModel.FFsFabricClearanceDetailsList.RemoveAll(c=>true);

                foreach (var item in x)
                {
                    fFsFabricClearanceViewModel.FFsFabricClearanceDetailsList.Add(new F_FS_FABRIC_CLEARANCE_DETAILS
                    {
                        ROLL_ = item,
                        ROLL_ID = item.ROLL_ID,
                        PROD_DATE = item.ROLL_INSPDATE,
                        INSPECTION_REMARKS = item.REMARKS,
                        STATUS = fFsFabricClearanceViewModel.FFsFabricClearanceDetails.STATUS
                    });
                }

                return fFsFabricClearanceViewModel;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }


        public async Task<FFsFabricClearanceViewModel> GetRollDetailsEditAsync(
            FFsFabricClearanceViewModel fFsFabricClearanceViewModel)
        {
            try
            {
                var dateFrom = fFsFabricClearanceViewModel.FFsFabricClearanceMaster.DATE_FROM;
                var dateTo = fFsFabricClearanceViewModel.FFsFabricClearanceMaster.DATE_TO;

                fFsFabricClearanceViewModel.FFsFabricClearanceMaster.FABCODENavigation = await DenimDbContext
                    .RND_FABRICINFO
                    .Where(c => c.FABCODE.Equals(fFsFabricClearanceViewModel.FFsFabricClearanceMaster.FABCODE))
                    .FirstOrDefaultAsync();
                
                fFsFabricClearanceViewModel.FFsFabricClearanceMaster.PO = await DenimDbContext
                    .RND_PRODUCTION_ORDER
                    .Include(c=>c.SO)
                    .Where(c => c.POID.Equals(fFsFabricClearanceViewModel.FFsFabricClearanceMaster.ORDER_NO))
                    .FirstOrDefaultAsync();
                
                var result = await DenimDbContext
                    .F_PR_INSPECTION_PROCESS_DETAILS
                    .Include(c=>c.INSP.SET.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation)
                    .Where(c => c.ROLL_INSPDATE >= dateFrom && c.ROLL_INSPDATE <= dateTo && c.FAB_GRADE == "A" && (c.INSP.SET.PROG_.BLK_PROG_.RndProductionOrder.POID.Equals(fFsFabricClearanceViewModel.FFsFabricClearanceMaster.ORDER_NO) || c.OPT3.Equals(fFsFabricClearanceViewModel.FFsFabricClearanceMaster.PO.SO.SO_NO)) && !DenimDbContext.F_FS_FABRIC_CLEARANCE_DETAILS.Any(d=>d.ROLL_ID.Equals(c.ROLL_ID)))
                    //.Select(c => new F_FS_FABRIC_CLEARANCE_DETAILS
                    //{
                    //    ROLL_ID = c.ROLL_ID,
                    //    ROLL_ = c,
                    //    PROD_DATE = DateTime.Now,
                    //})
                    .OrderBy(c => c.ROLLNO)
                    .ToListAsync();

                var x = new List<F_PR_INSPECTION_PROCESS_DETAILS>();

                if (fFsFabricClearanceViewModel.FFsFabricClearanceMaster.OPT1 != null &&
                    fFsFabricClearanceViewModel.FFsFabricClearanceMaster.OPT2 != null)
                {
                    x = result.Where(c => string.Compare(c.ROLLNO, fFsFabricClearanceViewModel.FFsFabricClearanceMaster.OPT1, StringComparison.Ordinal) >= 0 && string.Compare(c.ROLLNO, fFsFabricClearanceViewModel.FFsFabricClearanceMaster.OPT2, StringComparison.Ordinal) <= 0).ToList();
                }
                
                foreach (var item in x.Where(d=>!fFsFabricClearanceViewModel.FFsFabricClearanceDetailsList.Any(c=>c.ROLL_ID.Equals(d.ROLL_ID))))
                {
                    fFsFabricClearanceViewModel.FFsFabricClearanceDetailsList.Add(new F_FS_FABRIC_CLEARANCE_DETAILS
                    {
                        ROLL_ = item,
                        ROLL_ID = item.ROLL_ID,
                        PROD_DATE = item.ROLL_INSPDATE,
                        INSPECTION_REMARKS = item.REMARKS,
                        STATUS = fFsFabricClearanceViewModel.FFsFabricClearanceDetails.STATUS
                    });
                }

                foreach (var item in result.Where(d => !fFsFabricClearanceViewModel.FFsFabricClearanceDetailsList.Any(c => c.ROLL_ID.Equals(d.ROLL_ID))))
                {
                    fFsFabricClearanceViewModel.FFsFabricClearanceDetailsList.Add(new F_FS_FABRIC_CLEARANCE_DETAILS
                    {
                        ROLL_ = item,
                        ROLL_ID = item.ROLL_ID,
                        PROD_DATE = item.ROLL_INSPDATE,
                        INSPECTION_REMARKS = item.REMARKS,
                        STATUS = fFsFabricClearanceViewModel.FFsFabricClearanceDetails.STATUS
                    });
                }

                foreach (var item in result.Where(d => fFsFabricClearanceViewModel.FFsFabricClearanceDetailsList.Any(c => c.ROLL_ID.Equals(d.ROLL_ID))))
                {
                    foreach (var i in fFsFabricClearanceViewModel.FFsFabricClearanceDetailsList.Where(i => i.ROLL_ID == item.ROLL_ID))
                    {
                        i.ROLL_ = item;
                    }
                }
                

                return fFsFabricClearanceViewModel;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<FFsFabricClearanceViewModel> GetDetailsAsync(
            FFsFabricClearanceViewModel fFsFabricClearanceViewModel)
        {
            try
            {

                foreach (var item in fFsFabricClearanceViewModel.FFsFabricClearanceDetailsList)
                {
                    item.ROLL_ = await DenimDbContext.F_PR_INSPECTION_PROCESS_DETAILS
                        .Where(c => c.ROLL_ID.Equals(item.ROLL_ID)).FirstOrDefaultAsync();
                }

                return fFsFabricClearanceViewModel;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
