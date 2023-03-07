using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;

namespace DenimERP.ServiceInterfaces
{
    public interface IPL_BULK_PROG_YARN_D: IBaseService<PL_BULK_PROG_YARN_D>
    {
        Task<IEnumerable<PL_BULK_PROG_YARN_D>> GetYarnListByProgId(int progId);
        Task<RND_FABRIC_COUNTINFO> GetCountDetails(int countId);
    }
}
