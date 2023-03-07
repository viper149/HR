using System;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;
using DenimERP.ViewModels.Basic.YarnCountInfo;
using Microsoft.EntityFrameworkCore;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLBAS_YARN_COUNT_LOT_INFO_Repository:BaseRepository<BAS_YARN_COUNT_LOT_INFO>, IBAS_YARN_COUNT_LOT_INFO
    {
        public SQLBAS_YARN_COUNT_LOT_INFO_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }

        public async Task<CreateBasYarnCountInfoViewModel> GetLotDetailsAsync(
            CreateBasYarnCountInfoViewModel createBasYarnCountInfoViewModel)
        {
            try
            {
                foreach (var item in createBasYarnCountInfoViewModel.BasYarnCountLotInfoList)
                {
                    item.LOT = await DenimDbContext.BAS_YARN_LOTINFO
                        .FirstOrDefaultAsync(c => c.LOTID.Equals(item.LOTID));
                }
                return createBasYarnCountInfoViewModel;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<bool> DeleteCountLotAsync(int countId)
        {
            try
            {
                var result = await DenimDbContext.BAS_YARN_COUNT_LOT_INFO.Where(c => c.COUNTID.Equals(countId))
                    .ToListAsync();

                DenimDbContext.BAS_YARN_COUNT_LOT_INFO.RemoveRange(result);
                 await SaveChanges();
                return true;

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }
    }
}
