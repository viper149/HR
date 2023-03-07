using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;

namespace DenimERP.ServiceInterfaces
{
    public interface IBAS_YARN_CATEGORY:IBaseService<BAS_YARN_CATEGORY>
    {
        Task<bool> FindByYarnCategoryName(string catName);
        Task<bool> FindByYarnCode(int? code);
        Task<bool> DeleteCategory(int id);
    }
}
