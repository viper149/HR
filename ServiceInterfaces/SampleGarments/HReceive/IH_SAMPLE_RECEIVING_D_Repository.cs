using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;

namespace DenimERP.ServiceInterfaces.SampleGarments.HReceive
{
    public interface IH_SAMPLE_RECEIVING_D : IBaseService<H_SAMPLE_RECEIVING_D>
    {
        Task<double?> GetAvailableQty(int rcvdId);
    }
}
