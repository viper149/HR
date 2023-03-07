using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;

namespace DenimERP.ServiceInterfaces
{
    public interface IRND_SAMPLE_INFO_DETAILS : IBaseService<RND_SAMPLE_INFO_DETAILS>
    {
        Task<IEnumerable<RND_SAMPLE_INFO_DETAILS>> GetAllBySdIdAsync(int sdId);
        Task<IEnumerable<RND_SAMPLE_INFO_DETAILS>> FindBySdIdAsync(int sdId);
    }
}
