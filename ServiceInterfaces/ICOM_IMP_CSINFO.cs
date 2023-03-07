using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;

namespace DenimERP.ServiceInterfaces
{
    public interface ICOM_IMP_CSINFO:IBaseService<COM_IMP_CSINFO>
    {
        Task<int> InsertAndGetIdAsync(COM_IMP_CSINFO comImpCsInfo);
    }
}
