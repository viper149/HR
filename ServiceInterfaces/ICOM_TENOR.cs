using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;

namespace DenimERP.ServiceInterfaces
{
    public interface ICOM_TENOR: IBaseService<COM_TENOR>
    {
        Task<bool> FindByTypeName(string name);
        Task<IEnumerable<COM_TENOR>> GetAllForDataTableByAsync();
    }
}
