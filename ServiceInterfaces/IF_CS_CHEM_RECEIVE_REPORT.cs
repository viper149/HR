using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;

namespace DenimERP.ServiceInterfaces
{
    public interface IF_CS_CHEM_RECEIVE_REPORT : IBaseService<F_CS_CHEM_RECEIVE_REPORT>
    {
        Task<int> GetLastMrrNo();
    }
}
