using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels;

namespace DenimERP.ServiceInterfaces
{
    public interface IF_GEN_S_INDENTDETAILS :IBaseService<F_GEN_S_INDENTDETAILS>
    {
        Task<F_GEN_S_INDENTDETAILS> FindFGenSIndentListByIdAsync(int id, int prdId, bool edit=false);
        Task<FGenSRequisitionViewModel> GetInitObjForDetails(FGenSRequisitionViewModel fGenSRequisitionViewModel);
    }
}
