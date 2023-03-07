using System.Threading.Tasks;
using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;
using DenimERP.ViewModels;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLF_PR_RECONE_YARN_CONSUMPTION_Repository : BaseRepository<F_PR_RECONE_YARN_CONSUMPTION>, IF_PR_RECONE_YARN_CONSUMPTION
    {
        public SQLF_PR_RECONE_YARN_CONSUMPTION_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }

        public async Task<FPrReconeMasterViewModel> GetInitObjForDetailsByAsync(FPrReconeMasterViewModel fPrReconeMasterViewModel)
        {
            return fPrReconeMasterViewModel;
        }
    }
}
