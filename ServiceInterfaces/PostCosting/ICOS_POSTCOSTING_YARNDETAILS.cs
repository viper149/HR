using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels.PostCostingMaster;

namespace DenimERP.ServiceInterfaces.PostCosting
{
    public interface ICOS_POSTCOSTING_YARNDETAILS:IBaseService<COS_POSTCOSTING_YARNDETAILS>
    {
        Task<PostCostingViewModel> GetYarnDetailsBySo(PostCostingViewModel postCostingViewModel);
    }
}
