using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;

namespace DenimERP.ServiceInterfaces
{
    public interface IPDL_EMAIL_SENDER : IBaseService<PDL_EMAIL_SENDER>
    {
        Task<PDL_EMAIL_SENDER> GetTop1Async();
    }
}
