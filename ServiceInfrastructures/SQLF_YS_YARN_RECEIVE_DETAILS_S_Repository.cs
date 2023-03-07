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
    public class SQLF_YS_YARN_RECEIVE_DETAILS_S_Repository : BaseRepository<F_YS_YARN_RECEIVE_DETAILS_S>, IF_YS_YARN_RECEIVE_DETAILS_S
    {
        public SQLF_YS_YARN_RECEIVE_DETAILS_S_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }

        public async Task<FYsYarnReceiveSViewModel> GetDetailsData(FYsYarnReceiveSViewModel fYsYarnReceiveSViewModel)
        {
            foreach (var item in fYsYarnReceiveSViewModel.FYsYarnReceiveDetailList)
            {
                item.LOTNavigation = await DenimDbContext.BAS_YARN_LOTINFO.Where(c => c.LOTID.Equals(item.LOT))
                    .FirstOrDefaultAsync();
                item.RAWNavigation = await DenimDbContext.F_YS_RAW_PER.Where(c => c.RAWID.Equals(item.RAW))
                    .FirstOrDefaultAsync();
            }

            return fYsYarnReceiveSViewModel;
        }
    }
}
