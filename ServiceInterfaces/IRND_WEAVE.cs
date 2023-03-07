using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;

namespace DenimERP.ServiceInterfaces
{
    public interface IRND_WEAVE: IBaseService<RND_WEAVE>
    {
        Task<IEnumerable<RND_WEAVE>> GetRndWeaveWithPaged(int pageNumber, int pageSize);
        Task<bool> FindByTypeName(string name);
    }
}
