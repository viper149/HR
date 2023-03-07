using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;

namespace DenimERP.ServiceInterfaces
{
    public interface IRND_FABRICINFO_APPROVAL_DETAILS : IBaseService<RND_FABRICINFO_APPROVAL_DETAILS>
    {
        Task<bool> FindByFabCodeAsync(int fabCode);
    }
}
