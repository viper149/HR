using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels.Marketing;

namespace DenimERP.ServiceInterfaces
{
    public interface IMKT_TEAM: IBaseService<MKT_TEAM>
    {
        Task<IEnumerable<MKT_TEAM>> FindTeamMembersAsync(int teamid);
        Task<ContainEmailsInformation> GetEmailsByTeamIdMktPersonIdAsync(int teamId, int mktPersonId);
    }
}
