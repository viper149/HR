using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels.SampleGarments.Fabric;

namespace DenimERP.ServiceInterfaces.SampleGarments.Fabric
{
    public interface IF_SAMPLE_FABRIC_ISSUE : IBaseService<F_SAMPLE_FABRIC_ISSUE>
    {
        Task<CreateFSampleFabricIssueViewModel> GetInitObjByAsync(CreateFSampleFabricIssueViewModel createFSampleFabricIssueViewModel);
        Task<IEnumerable<ExtendFSampleFabricIssueViewModel>> GetAllForDataTableByAsync();
        Task<bool> IsSrNoInUseByAsync(CreateFSampleFabricIssueViewModel createFSampleFabricIssueViewModel);
        Task<CreateFSampleFabricIssueViewModel> FindByIdIncludeAllAsync(int sfiId);
        Task<string> GetSrNoPrefixByAsync(CreateFSampleFabricIssueViewModel createFSampleFabricIssueViewModel);
        Task<bool> FindBySrNoAsync(string srNo);
        Task<CreateFSampleFabricIssueViewModel> GetInitObjForDetailsTableByAsync(CreateFSampleFabricIssueViewModel createFSampleFabricIssueViewModel);
        Task<F_SAMPLE_FABRIC_ISSUE> FindByIdIncludeAllForDeleteAsync(int sfiId);
    }
}
