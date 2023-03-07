using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels;

namespace DenimERP.ServiceInterfaces
{
    public interface IF_GEN_S_INDENTMASTER : IBaseService<F_GEN_S_INDENTMASTER>
    {
        Task<FGenSRequisitionViewModel> GetInitObjByAsync(FGenSRequisitionViewModel fGenSRequisitionViewModel);
        Task<FGenSRequisitionViewModel> FindByIdIncludeAllAsync(int gindId);
        Task<IEnumerable<F_GEN_S_INDENTMASTER>> GetAllGenSIndentAsync();
        Task<int> GetLastGenSIndentNo();
        Task<F_GEN_S_INDENTMASTER> GetIndentByIndId(int id);
        Task<FGenSRequisitionViewModel> GetInitEditObjByAsync(FGenSRequisitionViewModel fGenSRequisitionViewModel);
    }
}
