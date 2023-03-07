using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels.YarnStore.Receive;

namespace DenimERP.ServiceInterfaces
{
    public interface IF_YARN_QC_APPROVE : IBaseService<F_YARN_QC_APPROVE>
    {
        Task<GetQcAndReceiveReportViewModel> FindByTrnsIdAsync(int trnsId);
    }
}
