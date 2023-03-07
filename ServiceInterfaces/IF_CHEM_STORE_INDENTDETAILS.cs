using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;

namespace DenimERP.ServiceInterfaces
{
    public interface IF_CHEM_STORE_INDENTDETAILS : IBaseService<F_CHEM_STORE_INDENTDETAILS>
    {
        Task<F_CHEM_STORE_INDENTDETAILS> FindChemIndentListByIdAsync(int id, int prdId);
    }
}
