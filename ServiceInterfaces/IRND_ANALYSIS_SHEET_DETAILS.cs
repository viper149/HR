using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;

namespace DenimERP.ServiceInterfaces
{
    public interface IRND_ANALYSIS_SHEET_DETAILS: IBaseService<RND_ANALYSIS_SHEET_DETAILS>
    {
        Task<IEnumerable<RND_ANALYSIS_SHEET_DETAILS>> GetAnalysisYarnDetailsList(int aid);
    }
}
