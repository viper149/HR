using System.Threading.Tasks;
using HRMS.Models;
using HRMS.ServiceInterfaces.BaseInterfaces;

namespace HRMS.ServiceInterfaces
{
    public interface IPDL_EMAIL_SENDER : IBaseService<PDL_EMAIL_SENDER>
    {
        Task<PDL_EMAIL_SENDER> GetTop1Async();
    }
}
