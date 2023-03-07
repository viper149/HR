using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;

namespace DenimERP.ServiceInterfaces
{
    public interface IBAS_TEAMINFO : IBaseService<BAS_TEAMINFO>
    {
        Task<IEnumerable<BAS_TEAMINFO>> GetBasTeamsAllAsync();
        bool FindByTeamName(string teamName);
    }
}
