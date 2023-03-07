using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels;

namespace DenimERP.ServiceInterfaces
{
    public interface IF_WASTE_PRODUCTINFO : IBaseService<F_WASTE_PRODUCTINFO>
    {

        Task<IEnumerable<F_WASTE_PRODUCTINFO>> GetAllWasteProductInfoAsync();

        Task<FWasteProductInfoViewModel> GetInitObjByAsync(FWasteProductInfoViewModel F_WASTE_PRODUCTINFOViewModel);
        Task<bool> FindByProductName(string pName);
    }

}
