using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;

namespace DenimERP.ServiceInterfaces
{
    public interface IH_GS_ITEM_SUBCATEGORY : IBaseService<H_GS_ITEM_SUBCATEGORY>
    {
        Task<List<H_GS_ITEM_SUBCATEGORY>> GetAllHGsItemSubCategoryAsync();
    }
}
