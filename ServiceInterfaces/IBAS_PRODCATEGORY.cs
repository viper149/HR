using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;

namespace DenimERP.ServiceInterfaces
{
    public interface IBAS_PRODCATEGORY : IBaseService<BAS_PRODCATEGORY>
    {
        bool FindByProductCategoryName(string categoryName);
        Task<IEnumerable<BAS_PRODCATEGORY>> GetProductCategoryInfoWithPaged(int pageNumber = 1, int pageSize = 1);
        Task<int> TotalNumberOfProductCategory();
        Task<bool> DeleteCategory(int id);
    }
}
