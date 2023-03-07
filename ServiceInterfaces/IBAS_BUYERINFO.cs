using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;

namespace DenimERP.ServiceInterfaces
{
    public interface IBAS_BUYERINFO : IBaseService<BAS_BUYERINFO>
    {
        Task<bool> DeleteInfo(int id);
        Task<string> GetBuyerNameById(int id);
        bool FindByBuyerName(string buyerName);
        Task<IQueryable<BAS_BUYERINFO>> GetDataForDataTableByAsync();
        Task<IEnumerable<BAS_BUYERINFO>> GetBasBuyerInfoWithPaged(int pageNumber = 1, int pageSize = 5);
        Task<string> GetBuyerAddressByIdAsync(int buyerId);
    }
}
