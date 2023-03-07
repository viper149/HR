using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels;

namespace DenimERP.ServiceInterfaces
{
    public interface IF_QA_YARN_TEST_INFORMATION_COTTON : IBaseService<F_QA_YARN_TEST_INFORMATION_COTTON>
    {
        Task<FQaYarnTestInformationCottonViewModel> GetOtherDetailsOfYMaster(int yRcvId);
        Task<IEnumerable<F_QA_YARN_TEST_INFORMATION_COTTON>> GetAllAsync();
        Task<FQaYarnTestInformationCottonViewModel> GetInitObjByAsync(FQaYarnTestInformationCottonViewModel fQaYarnTestInformationCottonViewModel);
    }
}