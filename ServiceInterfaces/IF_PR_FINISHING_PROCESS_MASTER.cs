using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels;
using DenimERP.ViewModels.Factory.Production;

namespace DenimERP.ServiceInterfaces
{
    public interface IF_PR_FINISHING_PROCESS_MASTER:IBaseService<F_PR_FINISHING_PROCESS_MASTER>
    {
        Task<IEnumerable<dynamic>> GetAllAsync();
        Task<PrFinishingProcessViewModel> GetInitObjects(PrFinishingProcessViewModel prFinishingProcessViewModel);
        Task<F_PR_WEAVING_PROCESS_BEAM_DETAILS_B> GetBeamDetails(int beamId);
        Task<F_PR_WEAVING_PROCESS_DETAILS_B> GetLoomDetails(int loomId);
        Task<int> InsertAndGetIdAsync(F_PR_FINISHING_PROCESS_MASTER fPrFinishingProcessMaster);
        Task<IEnumerable<F_PR_WEAVING_PROCESS_DETAILS_B>> GetStyleDetails(int fabcode,int setId);
        Task<PrFinishingProcessViewModel> GetFinishingDetails(int finId);
        Task<IEnumerable<F_PR_WEAVING_PROCESS_DETAILS_B>> GetStyleDetailsEdit(int fabcode);
        Task<dynamic> GetStyleDetailsBySetId(int setId);
       
    }
}
