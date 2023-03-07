using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels.PostCostingMaster;

namespace DenimERP.ServiceInterfaces.PostCosting
{
    public interface ICOS_POSTCOSTING_MASTER: IBaseService<COS_POSTCOSTING_MASTER>
    {
        Task<PostCostingViewModel> GetInitObj(PostCostingViewModel postCostingViewModel);
        Task<PostCostingViewModel> GetInfoAsync(PostCostingViewModel postCostingViewModel);
        Task<List<COS_POSTCOSTING_MASTER>> GetAllAsync();
        Task<PostCostingViewModel> FindAllByIdAsync(int id);
        Task<double> GetSoEliteQty(int id);
    }
}
