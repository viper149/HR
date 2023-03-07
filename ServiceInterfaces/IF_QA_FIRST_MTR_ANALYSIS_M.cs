using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels;

namespace DenimERP.ServiceInterfaces
{
    public interface IF_QA_FIRST_MTR_ANALYSIS_M : IBaseService<F_QA_FIRST_MTR_ANALYSIS_M>
    {
        Task<IEnumerable<F_QA_FIRST_MTR_ANALYSIS_M>> GetAllFirstMeterAnalysisInformation();
        Task<FQAFirstMtrAnalysisMViewModel> GetInitObjByAsync(FQAFirstMtrAnalysisMViewModel fqaFirstMtrAnalysisMViewModel);

        Task<string> GetLastReptNoAsync();
        Task<PL_PRODUCTION_SETDISTRIBUTION> GetBySetIdAsync(int setId);
        Task<F_PR_WEAVING_PROCESS_BEAM_DETAILS_B> GetByBeamIdAsync(int beamId);
        Task<FQAFirstMtrAnalysisMViewModel> FindByIdIncludeAllAsync(int fmaId);
    }
}
