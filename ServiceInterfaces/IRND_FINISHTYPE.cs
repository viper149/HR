using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;

namespace DenimERP.ServiceInterfaces
{
    public interface IRND_FINISHTYPE: IBaseService<RND_FINISHTYPE>
    {
        Task<IEnumerable<RND_FINISHTYPE>> GetRndFinishTypeWithPaged(int pageNumber, int pageSize);
        Task<bool> FindByTypeName(string typeName);
    }
}
