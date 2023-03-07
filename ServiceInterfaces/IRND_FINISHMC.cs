using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;

namespace DenimERP.ServiceInterfaces
{
    public interface IRND_FINISHMC : IBaseService<RND_FINISHMC>
    {
        Task<IEnumerable<RND_FINISHMC>> GetRndFinishMcWithPaged(int pageNumber, int pageSize);
        Task<bool> FindByTypeName(string name);
        Task<IEnumerable<RND_SAMPLE_INFO_WEAVING>> GetRndSampleInfoWeavingsByAsync();
    }
}
