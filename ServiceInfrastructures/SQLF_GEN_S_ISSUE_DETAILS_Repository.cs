using System.Threading.Tasks;
using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;
using DenimERP.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLF_GEN_S_ISSUE_DETAILS_Repository : BaseRepository<F_GEN_S_ISSUE_DETAILS>, IF_GEN_S_ISSUE_DETAILS
    {
        public SQLF_GEN_S_ISSUE_DETAILS_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }

        public async Task<FGenSIssueViewModel> GetInitObjForDetails(FGenSIssueViewModel fGenSIssueViewModel)
        {
            foreach (var item in fGenSIssueViewModel.FGenSIssueDetailsesList)
            {
                item.PRODUCT = await DenimDbContext.F_GS_PRODUCT_INFORMATION
                    .Include(d => d.UNITNavigation)
                    .FirstOrDefaultAsync(d => d.PRODID.Equals(item.PRODUCTID));
            }

            return fGenSIssueViewModel;
        }
    }
}
