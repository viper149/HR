using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels;
using DenimERP.ViewModels.Rnd;

namespace DenimERP.ServiceInterfaces
{
    public interface IRND_FABRIC_COUNTINFO : IBaseService<RND_FABRIC_COUNTINFO>
    {
        Task<IEnumerable<RND_FABRIC_COUNTINFO>> FindByFabCodeIAsync(int fabCode);
        Task<IEnumerable<RndFabricCountInfoViewModel>> FindByFabCodeIAllAsync(int fabCode);
        Task<RND_FABRIC_COUNTINFO> GetLotFromRNDFCI(int count);
        Task<RndFabricInfoViewModel> GetCountDetailsInfo(RndFabricInfoViewModel rndFabricInfoViewModel);
    }
}
