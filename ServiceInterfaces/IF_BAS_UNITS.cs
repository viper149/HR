using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;

namespace DenimERP.ServiceInterfaces
{
    public interface IF_BAS_UNITS : IBaseService<F_BAS_UNITS>
    {
        Task<F_BAS_UNITS> GetSingleDetails(int? unitid);
    }
}
