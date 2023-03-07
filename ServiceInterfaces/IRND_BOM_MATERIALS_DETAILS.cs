using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels;

namespace DenimERP.ServiceInterfaces
{
    public interface IRND_BOM_MATERIALS_DETAILS : IBaseService<RND_BOM_MATERIALS_DETAILS>
    {
        Task<RndBomViewModel> GetMaterialsNameAsync(RndBomViewModel rndBomViewModel);
    }
}
