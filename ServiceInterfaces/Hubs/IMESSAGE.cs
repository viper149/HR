using System.Collections.Generic;
using System.Threading.Tasks;
using HRMS.Models;
using HRMS.ServiceInterfaces.BaseInterfaces;

namespace HRMS.ServiceInterfaces.Hubs
{
    public interface IMESSAGE : IBaseService<MESSAGE>
    {
        Task<IEnumerable<MESSAGE>> GetAllIncludeOtherObjects();
    }
}
