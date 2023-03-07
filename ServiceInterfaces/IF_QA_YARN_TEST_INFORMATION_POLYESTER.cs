using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels;

namespace DenimERP.ServiceInterfaces
{
    public interface IF_QA_YARN_TEST_INFORMATION_POLYESTER : IBaseService<F_QA_YARN_TEST_INFORMATION_POLYESTER>
    {
        Task<FQaYarnTestInformationPolyesterViewModel> GetOtherDetailsOfYMaster(int yRcvId);
        Task<IEnumerable<F_QA_YARN_TEST_INFORMATION_POLYESTER>> GetAllAsync();
        Task<FQaYarnTestInformationPolyesterViewModel> GetInitObjByAsync(FQaYarnTestInformationPolyesterViewModel fQaYarnTestInformationPolyesterViewModel);
    }
}
