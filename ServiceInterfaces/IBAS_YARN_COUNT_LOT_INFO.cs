using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels.Basic.YarnCountInfo;

namespace DenimERP.ServiceInterfaces
{
    public interface IBAS_YARN_COUNT_LOT_INFO:IBaseService<BAS_YARN_COUNT_LOT_INFO>
    {
        Task<CreateBasYarnCountInfoViewModel> GetLotDetailsAsync(CreateBasYarnCountInfoViewModel createBasYarnCountInfoViewModel);
        Task<bool> DeleteCountLotAsync(int countId);
    }
}
