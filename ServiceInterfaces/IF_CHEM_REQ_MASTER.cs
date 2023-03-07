using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels;

namespace DenimERP.ServiceInterfaces
{
    public interface IF_CHEM_REQ_MASTER : IBaseService<F_CHEM_REQ_MASTER>
    {
        Task<IEnumerable<F_CHEM_REQ_MASTER>> GetAllChemicalRequirementAsync();
        Task<IEnumerable<FChemIssueViewModel>> GetChemReqMaster(int id);
        Task<IEnumerable<F_CHEM_REQ_MASTER>> GetRequirementDD();
        Task<FChemRequirementViewModel> GetInitObjByAsync(FChemRequirementViewModel fChemRequirementViewModel);
        Task<IEnumerable<F_BAS_SUBSECTION>> GetSubSectionsBySectionIdAsync(int sectionId);
        Task<FChemRequirementViewModel> GetInitDetailsObjByAsync(FChemRequirementViewModel fChemRequirementViewModel);
        Task<FChemRequirementViewModel> FindByIdIncludeAllAsync(int csrId);
    }
}
