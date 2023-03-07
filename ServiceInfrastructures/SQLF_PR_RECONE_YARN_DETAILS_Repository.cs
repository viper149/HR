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
    public class SQLF_PR_RECONE_YARN_DETAILS_Repository : BaseRepository<F_PR_RECONE_YARN_DETAILS>, IF_PR_RECONE_YARN_DETAILS
    {
        public SQLF_PR_RECONE_YARN_DETAILS_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }


        public async Task<FPrReconeMasterViewModel> GetInitObjForDetailsByAsync(FPrReconeMasterViewModel fPrReconeMasterViewModel)
        {

            foreach (var item in fPrReconeMasterViewModel.ReconeYarnDetailsList)
            {
                item.RECONE_ = await DenimDbContext.F_PR_RECONE_MASTER
                    .Where(d => d.TRANSID.Equals(item.TRANSID))
                    .Select(d => new F_PR_RECONE_MASTER
                    {
                        TRANSID = d.TRANSID,
                        NO_OF_BALL = d.NO_OF_BALL
                    }).FirstOrDefaultAsync();

                item.RECONE_ = await DenimDbContext.F_PR_RECONE_MASTER
                    .Where(d => d.TRANSID.Equals(item.TRANSID))
                    .Select(d => new F_PR_RECONE_MASTER
                    {
                        TRANSID = d.TRANSID,
                        NO_OF_BALL = d.NO_OF_BALL
                    }).FirstOrDefaultAsync();
            }
            return fPrReconeMasterViewModel;
        }
    }
}
