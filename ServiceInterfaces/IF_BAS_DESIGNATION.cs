using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;

namespace DenimERP.ServiceInterfaces
{
    public interface IF_BAS_DESIGNATION: IBaseService<F_BAS_DESIGNATION>
    {
        Task<bool> FindByDesignationNameAsync(string designationName);
    }
}
