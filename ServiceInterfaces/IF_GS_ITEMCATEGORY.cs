using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;

namespace DenimERP.ServiceInterfaces
{
    public interface IF_GS_ITEMCATEGORY : IBaseService<F_GS_ITEMCATEGORY>
    {
        Task<IEnumerable<F_GS_ITEMCATEGORY>> GetAllFGsItemCategoryAsync();
        Task<bool> FindByCatName(string catName);
    }
}
