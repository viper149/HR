using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;

namespace DenimERP.ServiceInterfaces.SampleGarments.HDispatch
{
    public interface IH_SAMPLE_DESPATCH_D : IBaseService<H_SAMPLE_DESPATCH_D>
    {
        Task<string> GetBarcodeByAsync(int rcvId);
    }
}
