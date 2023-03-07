using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;

namespace DenimERP.ServiceInterfaces.Hubs
{
    public interface IMESSAGE : IBaseService<MESSAGE>
    {
        Task<IEnumerable<MESSAGE>> GetAllIncludeOtherObjects();
    }
}
