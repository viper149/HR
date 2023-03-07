using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels.PostCostingMaster;

namespace DenimERP.ServiceInterfaces.PostCosting
{
    public interface ICOS_POSTCOSTING_CHEMDETAILS:IBaseService<COS_POSTCOSTING_CHEMDETAILS>
    {
        Task<PostCostingViewModel> GetChemicalDetailsBySo(PostCostingViewModel postCostingViewModel);
    }
}
