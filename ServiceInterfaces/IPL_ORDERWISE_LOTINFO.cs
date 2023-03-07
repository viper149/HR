using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;

namespace DenimERP.ServiceInterfaces
{
    public interface IPL_ORDERWISE_LOTINFO: IBaseService<PL_ORDERWISE_LOTINFO>
    {
        Task<IEnumerable<PL_ORDERWISE_LOTINFO>> GetInitObjects(List<PL_ORDERWISE_LOTINFO> plOrderwiseLotInfos);
        Task<IEnumerable<PL_ORDERWISE_LOTINFO>> FindByPoIdAsync(int poid);
    }
}
