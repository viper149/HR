using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;

namespace DenimERP.ServiceInterfaces
{
    public interface IF_PR_WEAVING_PROCESS_DETAILS_B:IBaseService<F_PR_WEAVING_PROCESS_DETAILS_B>
    {
        Task<F_PR_WEAVING_PROCESS_BEAM_DETAILS_B> FindByIdAllAsync(int beamId);
        Task<string> GetBeamDetailsByDoffIdAsync(int doffId);
    }
}
