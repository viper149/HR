using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels;

namespace DenimERP.ServiceInterfaces
{
    public interface IF_YARN_QC_APPROVE_S : IBaseService<F_YARN_QC_APPROVE_S>
    {
        Task<FYarnQCAppproveSViewModel> FindByTrnsIdAsync(int trnsId);
    }
}
