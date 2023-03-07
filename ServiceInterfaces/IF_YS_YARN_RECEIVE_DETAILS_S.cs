using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels;

namespace DenimERP.ServiceInterfaces
{
    public interface IF_YS_YARN_RECEIVE_DETAILS_S : IBaseService<F_YS_YARN_RECEIVE_DETAILS_S>
    {
        Task<FYsYarnReceiveSViewModel> GetDetailsData(FYsYarnReceiveSViewModel fYsYarnReceiveSViewModel);
    }
}
