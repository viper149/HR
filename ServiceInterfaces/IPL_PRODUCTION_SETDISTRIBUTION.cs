using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;

namespace DenimERP.ServiceInterfaces
{
    public interface IPL_PRODUCTION_SETDISTRIBUTION: IBaseService<PL_PRODUCTION_SETDISTRIBUTION>
    {
        Task<List<PL_PRODUCTION_SETDISTRIBUTION>> GetSetListBySubGroup(int subGroup);
    }
}
