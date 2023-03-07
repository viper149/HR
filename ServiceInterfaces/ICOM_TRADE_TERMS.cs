using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;

namespace DenimERP.ServiceInterfaces
{
    public interface ICOM_TRADE_TERMS:IBaseService<COM_TRADE_TERMS>
    {
        Task<bool> FindByTypeName(string name);
    }
}
