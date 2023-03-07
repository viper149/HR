using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels.Planning;

namespace DenimERP.ServiceInterfaces
{
    public interface IPL_BULK_PROG_SETUP_M: IBaseService<PL_BULK_PROG_SETUP_M>
    {
        Task<PlBulkProgSetupViewModel> GetInitObjects(PlBulkProgSetupViewModel plBulkProgSetupViewModel);
        Task<PlBulkProgSetupViewModel> GetInitData(PlBulkProgSetupViewModel plBulkProgSetupViewModel);
        Task<PL_ORDERWISE_LOTINFO> GetLotDetailsFromLotwiseTable(string lotId);
        Task<int> InsertAndGetIdAsync(PL_BULK_PROG_SETUP_M plBulkProgSetupM);
        Task<PlBulkProgSetupViewModel> FindAllByIdAsync(int id);
        Task<IEnumerable<PL_BULK_PROG_SETUP_M>> GetAllAsync();
        Task<IEnumerable<PL_BULK_PROG_SETUP_D>> GetAllSetAsync();
    }
}
