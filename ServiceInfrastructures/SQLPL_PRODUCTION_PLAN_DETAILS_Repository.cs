using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;
using DenimERP.ViewModels.Planning;
using Microsoft.EntityFrameworkCore;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLPL_PRODUCTION_PLAN_DETAILS_Repository: BaseRepository<PL_PRODUCTION_PLAN_DETAILS>, IPL_PRODUCTION_PLAN_DETAILS
    {
        public SQLPL_PRODUCTION_PLAN_DETAILS_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }

        public async Task<PlProductionGroupViewModel> GetInitData(PlProductionGroupViewModel plProductionGroupViewModel)
        {
            try
            {
                foreach (var i in plProductionGroupViewModel.PlProductionPlanDetailsList.SelectMany(item => item.PlProductionSetDistributionList))
                {
                    i.PROG_ = await DenimDbContext.PL_BULK_PROG_SETUP_D.FindAsync(i.PROG_ID);
                }
                return plProductionGroupViewModel;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public async Task<IEnumerable<PL_PRODUCTION_SO_DETAILS>> GetInitSoData(List<PL_PRODUCTION_SO_DETAILS> plProductionSoDetails)
        {
            try
            {
                foreach (var i in plProductionSoDetails)
                {
                    i.PO = await DenimDbContext.RND_PRODUCTION_ORDER
                        .GroupJoin(DenimDbContext.COM_EX_PI_DETAILS,
                            f1=>f1.ORDERNO,
                            f2=>f2.TRNSID,
                            (f1,f2)=> new RND_PRODUCTION_ORDER
                            {
                                POID = f1.POID,
                                OPT1 = f2.FirstOrDefault().SO_NO
                            })
                        .Where(c=>c.POID.Equals(i.POID))
                        .FirstOrDefaultAsync();
                }
                return plProductionSoDetails;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }


        public async Task<int> InsertAndGetIdAsync(PL_PRODUCTION_PLAN_DETAILS plProductionPlanDetails)
        {
            try
            {
                await DenimDbContext.PL_PRODUCTION_PLAN_DETAILS.AddAsync(plProductionPlanDetails);
                await SaveChanges();
                return plProductionPlanDetails.SUBGROUPID;
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                throw;
            }
        }
    }
}
