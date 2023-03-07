using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;
using Microsoft.EntityFrameworkCore;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLF_PR_FINISHING_FAB_PROCESS_Repository:BaseRepository<F_PR_FINISHING_FAB_PROCESS>, IF_PR_FINISHING_FAB_PROCESS
    {
        public SQLF_PR_FINISHING_FAB_PROCESS_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }

        public async Task<IEnumerable<F_PR_FINISHING_FAB_PROCESS>> GetInitFabricData(List<F_PR_FINISHING_FAB_PROCESS> fPrFinishingFabProcesses)
        {
            try
            {
                foreach (var item in fPrFinishingFabProcesses)
                {
                    item.FAB_MACHINE = await DenimDbContext.F_PR_PROCESS_MACHINEINFO.FirstOrDefaultAsync(c =>
                            c.FBMACHINEID.Equals(item.FAB_MACHINEID));
                    item.FAB_PRO_TYPE = await DenimDbContext.F_PR_PROCESS_TYPE_INFO.FirstOrDefaultAsync(c =>
                            c.FBPRTYPEID.Equals(item.FAB_PRO_TYPEID));
                    item.PROCESS_BYNavigation = await DenimDbContext.F_HRD_EMPLOYEE
                        .Select(c=>new F_HRD_EMPLOYEE
                        {
                            FIRST_NAME = c.FIRST_NAME+" "+ c.LAST_NAME,
                            EMPID = c.EMPID
                        })
                        .FirstOrDefaultAsync(c => c.EMPID.Equals(item.PROCESS_BY));
                    item.TROLLYNONavigation = await DenimDbContext.F_PR_FIN_TROLLY.FirstOrDefaultAsync(c => c.FIN_TORLLY_ID.Equals(item.TROLLYNO));
                }
                return fPrFinishingFabProcesses;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public async Task<IEnumerable<F_PR_FINISHING_FAB_PROCESS>> GetFabricList(int finId)
        {
            try
            {
                var fPrFinishingFabProcesses = await DenimDbContext.F_PR_FINISHING_FAB_PROCESS
                    .Where(c => c.FN_PROCESSID.Equals(finId))
                    .ToListAsync();
                return fPrFinishingFabProcesses;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }


    }
}
