using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels;

namespace DenimERP.ServiceInterfaces
{
    public interface IF_BAS_ASSET_LIST : IBaseService<F_BAS_ASSET_LIST>
    {
        Task<IEnumerable<F_BAS_ASSET_LIST>> GetAllFBasAssetListAsync();
        Task<FBasAssetListViewModel> GetInitObjByAsync(FBasAssetListViewModel fBasAssetListViewModel);
        Task<bool> FindByAssetName(string assetName, int? secId);
    }
}
