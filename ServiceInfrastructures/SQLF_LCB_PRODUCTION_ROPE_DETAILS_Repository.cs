using System;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;
using DenimERP.ViewModels.Factory.Production;
using Microsoft.EntityFrameworkCore;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLF_LCB_PRODUCTION_ROPE_DETAILS_Repository:BaseRepository<F_LCB_PRODUCTION_ROPE_DETAILS>, IF_LCB_PRODUCTION_ROPE_DETAILS
    {
        public SQLF_LCB_PRODUCTION_ROPE_DETAILS_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }

        public async Task<FLcbProductionRopeViewModel> GetInitData(FLcbProductionRopeViewModel fLcbProductionRopeViewModel)
        {
            try
            {
                foreach (var item in fLcbProductionRopeViewModel.FLcbProductionRopeDetailsList)
                {
                    item.CAN = await DenimDbContext.F_DYEING_PROCESS_ROPE_DETAILS
                        .Include(c => c.CAN_NONavigation)
                        .FirstOrDefaultAsync(c=>c.ROPEID.Equals(item.CANID));
                    item.EMPLOYEE = await DenimDbContext.F_HRD_EMPLOYEE.FirstOrDefaultAsync(c => c.EMPID.Equals(item.EMPLOYEEID));
                    foreach (var i in item.FLcbProductionRopeProcessInfoList)
                    {
                        i.BEAM = await DenimDbContext.F_LCB_BEAM.FirstOrDefaultAsync(c => c.ID.Equals(i.BEAMID));
                        i.MACHINE = await DenimDbContext.F_LCB_MACHINE.FirstOrDefaultAsync(c => c.ID.Equals(i.MACHINEID));
                    }
                }
                return fLcbProductionRopeViewModel;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }


        public async Task<int> InsertAndGetIdAsync(F_LCB_PRODUCTION_ROPE_DETAILS fLcbProductionRopeDetails)
        {
            try
            {
                await DenimDbContext.F_LCB_PRODUCTION_ROPE_DETAILS.AddAsync(fLcbProductionRopeDetails);
                await SaveChanges();
                return fLcbProductionRopeDetails.LCB_D_ID;
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                throw;
            }
        }


        public async Task<F_DYEING_PROCESS_ROPE_DETAILS> GetCanDetails(int canId)
        {
            try
            {

                var result = await DenimDbContext.F_DYEING_PROCESS_ROPE_DETAILS
                    .Include(c=>c.BALL)
                    .ThenInclude(c=>c.COUNT_)
                    //.Include(c=> c.SETNO.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.RND_FABRIC_COUNTINFO)
                    //.ThenInclude(c=>c.LOT)
                    //.Include(c=> c.SETNO.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.RND_FABRIC_COUNTINFO)
                    //.ThenInclude(c=>c.SUPP)
                    //.Include(c=> c.SETNO.PROG_.BLK_PROG_.RndProductionOrder.SO.STYLE.FABCODENavigation.RND_FABRIC_COUNTINFO)
                    //.ThenInclude(c=>c.COUNT)
                    .Include(c=>c.SUBGROUP.PL_PRODUCTION_SETDISTRIBUTION)
                    .ThenInclude(c=>c.PROG_.PL_BULK_PROG_YARN_D)
                    .ThenInclude(c=>c.LOT)
                    .Where(c => c.ROPEID.Equals(canId))
                    .FirstOrDefaultAsync();

                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

    }
}
