using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels;

namespace DenimERP.ServiceInterfaces
{
    public interface IF_GEN_S_REQ_MASTER : IBaseService<F_GEN_S_REQ_MASTER>
    {
        Task<IEnumerable<FGenSIssueViewModel>> GetGenSReqMaster(int id);
        Task<IEnumerable<F_GEN_S_REQ_MASTER>> GetRequirementDD();
        Task<FGenSRequirementViewModel> GetInitObjByAsync(FGenSRequirementViewModel fGenSRequirementViewModel);
        Task<FGenSRequirementViewModel> FindByIdIncludeAllAsync(int gsrId, bool edit = false);
        Task<IEnumerable<F_GEN_S_REQ_MASTER>> GetAllFGenSRequirementAsync();
        Task<FGenSRequirementViewModel> GetInitDetailsObjByAsync(FGenSRequirementViewModel fFGenSRequirementViewModel);
    }
}
