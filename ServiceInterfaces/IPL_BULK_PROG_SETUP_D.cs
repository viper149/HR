using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels.Planning;

namespace DenimERP.ServiceInterfaces
{
    public interface IPL_BULK_PROG_SETUP_D: IBaseService<PL_BULK_PROG_SETUP_D>
    {
        Task<int> InsertAndGetIdAsync(PL_BULK_PROG_SETUP_D plBulkProgSetupD);
        Task<bool> FindByProgNoInUseAsync(string ProgNo);
        Task<PlBulkProgSetupViewModel> GetBulkProgList(PlBulkProgSetupViewModel plBulkProgSetupViewModel);
        Task<string> GetLastSetNo(PlBulkProgSetupViewModel plBulkProgSetupViewModel);
    }
}
