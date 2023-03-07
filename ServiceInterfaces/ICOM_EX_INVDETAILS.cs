using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels.Com.InvoiceExport;

namespace DenimERP.ServiceInterfaces
{
    public interface ICOM_EX_INVDETAILS : IBaseService<COM_EX_INVDETAILS>
    {
        Task<IEnumerable<COM_EX_INVDETAILS>> FindByInvNoAsync(string invNo);
        Task<bool> FindByInvNoStyleIdAsync(string invNo, int styleId);
        Task<ComExInvoiceMasterCreateViewModel> GetInitObjByAsync(ComExInvoiceMasterCreateViewModel comExInvoiceMasterCreateViewModel);
        Task<int> FindByTrnsIdAsync(int trnsId);
        Task<COM_EX_INVDETAILS> GetSingleInvDetails(int trnsId);
        Task<COM_EX_LCINFO> GetInvoicAmountByLcno(string id);
    }
}
