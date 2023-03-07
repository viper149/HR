using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;

namespace DenimERP.ServiceInterfaces
{
    public interface IBAS_SEASON : IBaseService<BAS_SEASON>
    {

        Task<IEnumerable<BAS_SEASON>> GetAllBasSeason();
    }
}


