using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;

namespace DenimERP.ServiceInterfaces
{
    public interface IBAS_BRANDINFO : IBaseService<BAS_BRANDINFO>
    {
        bool FindByBrandName(string brandName);
        Task<IEnumerable<BAS_BRANDINFO>> GetBasBrandInfoWithPaged(int pageNumber = 1, int pageSize = 5);
        Task<bool> DeleteInfo(int id);
    }
}
