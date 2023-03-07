using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels;

namespace DenimERP.ServiceInterfaces
{
    public interface IF_QA_FIRST_MTR_ANALYSIS_D : IBaseService<F_QA_FIRST_MTR_ANALYSIS_D>
    {
        Task<FQAFirstMtrAnalysisMViewModel> GetInitObjForDetails(FQAFirstMtrAnalysisMViewModel fqaFirstMtrAnalysisMViewModel);
    }
}
