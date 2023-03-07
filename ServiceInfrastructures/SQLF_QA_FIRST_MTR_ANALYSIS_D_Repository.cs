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
    public class SQLF_QA_FIRST_MTR_ANALYSIS_D_Repository : BaseRepository<F_QA_FIRST_MTR_ANALYSIS_D>, IF_QA_FIRST_MTR_ANALYSIS_D
    {
        public SQLF_QA_FIRST_MTR_ANALYSIS_D_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }

        public async Task<FQAFirstMtrAnalysisMViewModel> GetInitObjForDetails(FQAFirstMtrAnalysisMViewModel fqaFirstMtrAnalysisMViewModel)
        {
            foreach (var item in fqaFirstMtrAnalysisMViewModel.FQaFirstMtrAnalysisDsList)
            {
                item.LOT = await DenimDbContext.BAS_YARN_LOTINFO
                    .Where(d => d.LOTID.Equals(item.LOTID))
                    .Select(d => new BAS_YARN_LOTINFO
                    {
                        LOTNO = d.LOTNO
                    }).FirstOrDefaultAsync();

                item.SUPPLIER = await DenimDbContext.BAS_SUPPLIERINFO
                    .Where(d => d.SUPPID.Equals(item.SUPPLIERID))
                    .Select(d => new BAS_SUPPLIERINFO
                    {
                        SUPPNAME = d.SUPPNAME
                    }).FirstOrDefaultAsync();
            }

            return fqaFirstMtrAnalysisMViewModel;
        }
    }
}
