using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels;

namespace DenimERP.ServiceInterfaces
{
    public interface IF_CHEM_STORE_INDENTMASTER : IBaseService<F_CHEM_STORE_INDENTMASTER>
    {
        Task<IEnumerable<F_CHEM_STORE_INDENTMASTER>> GetAllChemicalIndentAsync();
        Task<F_CHEM_STORE_INDENTMASTER> GetIndentByCindid(int id);
        Task<int> GetLastChmIndNo();
        Task<FChemicalRequisitionViewModel> FindByIdIncludeAllAsync(int cindId);
        Task<FChemicalRequisitionViewModel> GetInitObjByAsync(FChemicalRequisitionViewModel fChemicalRequisition);
    }
}
