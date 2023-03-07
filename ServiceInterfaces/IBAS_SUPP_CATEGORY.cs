using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;

namespace DenimERP.ServiceInterfaces
{
    public interface IBAS_SUPP_CATEGORY : IBaseService<BAS_SUPP_CATEGORY>
    {
        Task<IEnumerable<BAS_SUPP_CATEGORY>> GetBasSupplierCategoriesWithPaged(int pageNumber = 1, int pageSize = 5);
        Task<bool> DeleteCategory(int id);
        bool FindBySupplierCategoryName(string categoryName);
    }
}
