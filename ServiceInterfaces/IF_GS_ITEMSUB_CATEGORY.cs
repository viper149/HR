using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels.GeneralStore;

namespace DenimERP.ServiceInterfaces
{
    public interface IF_GS_ITEMSUB_CATEGORY : IBaseService<F_GS_ITEMSUB_CATEGORY>
    {
        Task<List<F_GS_ITEMSUB_CATEGORY>> GetAllFGsItemSubCategoryAsync();
        Task<bool> FindBySCatName(string scatName);
        Task<FGsItemSubCategoryViewModel> GetInitObjByAsync(FGsItemSubCategoryViewModel fGsItemSubCategoryViewModel);
        Task<IEnumerable<F_GS_ITEMSUB_CATEGORY>> GetSubCatByCatId(int catId);
    }
}
