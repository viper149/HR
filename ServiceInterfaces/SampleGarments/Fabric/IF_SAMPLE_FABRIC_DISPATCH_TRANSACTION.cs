using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;

namespace DenimERP.ServiceInterfaces.SampleGarments.Fabric
{
    public interface IF_SAMPLE_FABRIC_DISPATCH_TRANSACTION : IBaseService<F_SAMPLE_FABRIC_DISPATCH_TRANSACTION>
    {
        Task<double?> FindByDpIdAsync(int dpId, int trnsId);
    }
}
