using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels;

namespace DenimERP.ServiceInterfaces
{
   public interface IRND_BOM : IBaseService<RND_BOM>
    {
        Task<RndBomViewModel> GetInitObjects(RndBomViewModel rndBomViewModel);
        Task<IEnumerable<RND_BOM>> GetAllRndBomInfoAsync();
        Task<RND_FABRICINFO> GetAllByStyleIdAsync(int styleId);

        Task<RndBomViewModel> FindByIdIncludeAllAsync(int rbId);
    }
}
