using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;

namespace DenimERP.ServiceInterfaces
{
    public interface IBAS_SUPPLIERINFO : IBaseService<BAS_SUPPLIERINFO>
    {
        Task<IEnumerable<BAS_SUPPLIERINFO>> GetBasSupplierInfoWithPaged(int pageNumber = 1, int pageSize = 5);
        Task<IEnumerable<BAS_SUPPLIERINFO>> GetBasSupplierInfoAllAsync();
        Task<bool> DeleteInfo(int id);
        bool FindBySupplierInfoName(string categoryName);
        Task<bool> FindBySupplierInfoById(int id);
        Task<BAS_SUPPLIERINFO> FindSupplierInfoByAsync(int id);
        Task<string> FindSupplierNameByIdAsync(int id);
        Task<IEnumerable<BAS_SUPPLIERINFO>> GetForSelectItemsByAsync();
    }
}
