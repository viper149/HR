using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;

namespace DenimERP.ServiceInterfaces
{
    public interface IBAS_YARN_LOTINFO : IBaseService<BAS_YARN_LOTINFO>
    {
        Task<IEnumerable<BAS_YARN_LOTINFO>> GetBasYarnLotInfoWithPaged(int pageNumber = 1, int pageSize = 5);
        Task<int> TotalNumberOfBasYarnLot();
        Task<string> FindLotNoByIdAsync(int id);
        Task<IEnumerable<BAS_YARN_LOTINFO>> GetForSelectItemsByAsync();
    }
}
