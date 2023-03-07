using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;

namespace DenimERP.ServiceInterfaces
{
    public interface IH_GS_ITEM_CATEGORY : IBaseService<H_GS_ITEM_CATEGORY>
    {
         Task<List<H_GS_ITEM_CATEGORY>> GetAllHGsItemCategory();
         Task<bool> FindByCatName(string catName);
    }
}
