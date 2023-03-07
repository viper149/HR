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
    public class SQLF_GS_WASTAGE_ISSUE_D_Repository : BaseRepository<F_GS_WASTAGE_ISSUE_D>, IF_GS_WASTAGE_ISSUE_D
    {
        public SQLF_GS_WASTAGE_ISSUE_D_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }

        public async Task<F_BAS_UNITS> GetAllBywpIdAsync(int wpId)
        {
            return await DenimDbContext.F_BAS_UNITS
                .Select(d => new F_BAS_UNITS
                {
                    UID = d.UID,
                    UNAME = d.UNAME,

                }).FirstOrDefaultAsync();
        }

        public async Task<FGsWastageIssueViewModel> GetInitObjForDetailsByAsync(FGsWastageIssueViewModel fGsWastageIssueViewModel)
        {
            foreach (var item in fGsWastageIssueViewModel.FGsWastageIssueDList)
            {
                item.WP= await DenimDbContext.F_WASTE_PRODUCTINFO
                    .Where(d => d.WPID.Equals(item.WPID))
                    .Select(d => new F_WASTE_PRODUCTINFO
                    {
                        WPID = d.WPID,
                        PRODUCT_NAME = d.PRODUCT_NAME
                    }).FirstOrDefaultAsync();

            }

            return fGsWastageIssueViewModel;
        }
    }
}
