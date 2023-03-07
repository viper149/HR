using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;

namespace DenimERP.ServiceInterfaces
{
    public interface IF_YS_YARN_RECEIVE_REPORT : IBaseService<F_YS_YARN_RECEIVE_REPORT>
    {
        Task<int> GetLastMrrNo();
    }
}
