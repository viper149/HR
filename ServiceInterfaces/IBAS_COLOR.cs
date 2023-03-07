using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;

namespace DenimERP.ServiceInterfaces
{
    public interface IBAS_COLOR : IBaseService<BAS_COLOR>
    {
        Task<IEnumerable<BAS_COLOR>> GetColorsWithPaged(int pageNumber = 1, int pageSize = 5);
        Task<IEnumerable<BAS_COLOR>> GetForSelectItemsByAsync();
    }
}
