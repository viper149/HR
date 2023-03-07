using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels.Factory.Production;

namespace DenimERP.ServiceInterfaces
{
    public interface IF_LCB_PRODUCTION_ROPE_DETAILS:IBaseService<F_LCB_PRODUCTION_ROPE_DETAILS>
    {
        Task<FLcbProductionRopeViewModel> GetInitData(FLcbProductionRopeViewModel fLcbProductionRopeViewModel);
        Task<int> InsertAndGetIdAsync(F_LCB_PRODUCTION_ROPE_DETAILS fLcbProductionRopeDetails);
        Task<F_DYEING_PROCESS_ROPE_DETAILS> GetCanDetails(int canId);
    }
}
