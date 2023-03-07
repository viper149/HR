using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;

namespace DenimERP.ServiceInterfaces.SampleGarments.Receive
{
    public interface IF_SAMPLE_GARMENT_RCV_D : IBaseService<F_SAMPLE_GARMENT_RCV_D>
    {
        Task<IEnumerable<F_SAMPLE_GARMENT_RCV_D>> FindBySrIdAsync(int srId);
    }
}
