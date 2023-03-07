using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;

namespace DenimERP.ServiceInterfaces
{
    public interface ICOM_EX_SCINFO : IBaseService<COM_EX_SCINFO>
    {
        Task<IEnumerable<COM_EX_SCINFO>> GetComExScInfoAllAsync();
        Task<COM_EX_SCINFO> GetComExScInfoList(int scNo);
        Task<COM_EX_SCINFO> GetComExScInfoByScNo(int scNo);
        Task<bool> FindByScNoInUseAsync(string scNo);
        Task<string> GetLastSCNoAsync();
    }
}
