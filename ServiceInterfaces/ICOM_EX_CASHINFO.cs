using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels.Com;

namespace DenimERP.ServiceInterfaces
{
    public interface ICOM_EX_CASHINFO:IBaseService<COM_EX_CASHINFO>
    {
        Task<IEnumerable<COM_EX_CASHINFO>> GetAllAsync();
        Task<ComExCashInfoViewModel> GetInitObjects(ComExCashInfoViewModel comExCashInfoViewModel);
        Task<IEnumerable<COM_EX_LCINFO>> FindLCByIdAsync(int lcId);
    }
}
