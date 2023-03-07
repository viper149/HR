using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;

namespace DenimERP.ServiceInterfaces
{
    public interface IUPOZILAS: IBaseService<UPOZILAS>
    {
        Task<IEnumerable<UPOZILAS>> GetThanaByDistrictIdAsync(int id);
    }
}
