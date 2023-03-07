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
    public class SQLF_GS_GATEPASS_INFORMATION_D_Repository : BaseRepository<F_GS_GATEPASS_INFORMATION_D>, IF_GS_GATEPASS_INFORMATION_D
    {
        public SQLF_GS_GATEPASS_INFORMATION_D_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }

        public async Task<IEnumerable<F_GS_GATEPASS_INFORMATION_D>> GetGsGatePassInfoByGpId(int gpId)
        {
            try
            {
                var result = await DenimDbContext.F_GS_GATEPASS_INFORMATION_D
                    .Include(e=>e.GP)
                    .ThenInclude(c=>c.F_GS_RETURNABLE_GP_RCV_M)
                    .Where(e => e.GPID.Equals(gpId) && !DenimDbContext.F_GS_RETURNABLE_GP_RCV_D.Any(f => f.PRODID.Equals(e.PRODID) && f.RCVID.Equals(e.GP.F_GS_RETURNABLE_GP_RCV_M.FirstOrDefault().RCVID)))
                    .ToListAsync();

                //foreach (var item in result)
                //{
                //    if (_denimDbContext.F_GS_RETURNABLE_GP_RCV_D.Any(c => c.PRODID.Equals(item.PRODID) && c.RCVID.Equals(item.GP.F_GS_RETURNABLE_GP_RCV_M.FirstOrDefault().RCVID)))
                //    {
                //        result.Remove(item);
                //    }
                //}

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
