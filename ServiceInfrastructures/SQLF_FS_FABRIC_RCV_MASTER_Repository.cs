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
    public class SQLF_FS_FABRIC_RCV_MASTER_Repository: BaseRepository<F_FS_FABRIC_RCV_MASTER>, IF_FS_FABRIC_RCV_MASTER
    {
        public SQLF_FS_FABRIC_RCV_MASTER_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }

        public async Task<IEnumerable<F_FS_FABRIC_RCV_MASTER>> GetAllAsync()
        {
            try
            {
                var result = await DenimDbContext.F_FS_FABRIC_RCV_MASTER
                    .Include(c => c.SEC)
                    .Include(c=>c.F_FS_FABRIC_RCV_DETAILS)
                    .ToListAsync();
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<FFsRollReceiveViewModel> GetInitObjects(FFsRollReceiveViewModel fFsRollReceiveViewModel)
        {
            try
            {
                fFsRollReceiveViewModel.FBasSections = await DenimDbContext.F_BAS_SECTION.ToListAsync();
                fFsRollReceiveViewModel.FFsLocations = await DenimDbContext.F_FS_LOCATION.OrderBy(c=>c.LOC_NO).ToListAsync();
                return fFsRollReceiveViewModel;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<int> InsertAndGetIdAsync(F_FS_FABRIC_RCV_MASTER fFsFabricRcvMaster)
        {
            try
            {
                await DenimDbContext.F_FS_FABRIC_RCV_MASTER.AddAsync(fFsFabricRcvMaster);
                await SaveChanges();
                return fFsFabricRcvMaster.RCVID;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public async Task<F_FS_FABRIC_RCV_MASTER> GetRollDetailsByDate(DateTime? rcvDate)
        {
            try
            {
                var fFsFabricRcvMaster = await DenimDbContext.F_FS_FABRIC_RCV_MASTER
                    .FirstOrDefaultAsync(c => c.RCVDATE.Equals(rcvDate));
                return fFsFabricRcvMaster;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<FFsRollReceiveViewModel> FindByRollRcvIdAsync(int rollRcvId)
        {
            try
            {
                var result = await DenimDbContext.F_FS_FABRIC_RCV_MASTER
                    .Include(c=>c.F_FS_FABRIC_RCV_DETAILS)
                    .ThenInclude(c=>c.FABCODENavigation)
                    .Include(c=>c.F_FS_FABRIC_RCV_DETAILS)
                    .ThenInclude(c=>c.SO_NONavigation)
                    .Include(c=>c.F_FS_FABRIC_RCV_DETAILS)
                    .ThenInclude(c=>c.LOCATIONNavigation)
                    .Include(c=>c.F_FS_FABRIC_RCV_DETAILS)
                    .ThenInclude(c=>c.PO_NONavigation)
                    .Include(c=>c.SEC)
                    .Include(c=>c.F_FS_FABRIC_RCV_DETAILS)
                    .ThenInclude(c=>c.ROLL_)
                    .Where(c => c.RCVID.Equals(rollRcvId)).FirstOrDefaultAsync();

                var fFsRollReceiveViewModel = new FFsRollReceiveViewModel
                {
                    FFsFabricRcvMaster = result,
                    FFsFabricRcvDetailsList = result.F_FS_FABRIC_RCV_DETAILS.ToList()
                };

                return fFsRollReceiveViewModel;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
