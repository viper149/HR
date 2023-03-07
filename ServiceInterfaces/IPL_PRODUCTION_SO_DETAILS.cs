using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;

namespace DenimERP.ServiceInterfaces
{
    public interface IPL_PRODUCTION_SO_DETAILS: IBaseService<PL_PRODUCTION_SO_DETAILS>
    {
        Task<PL_PRODUCTION_SO_DETAILS> FindBySetIdAsync(int progId);
    }
}
