using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels.Com.InvoiceImport;

namespace DenimERP.ServiceInterfaces
{
    public interface ICOM_IMP_INVDETAILS : IBaseService<COM_IMP_INVDETAILS>
    {
        Task<IEnumerable<COM_IMP_INVDETAILS>> FindByInvNoAsync(string invNo);
        Task<COM_IMP_INVOICEINFO> GetSingleInvoiceDetails(int id);
        Task<bool> FindByInvNoProdIdAsync(string invNo, int? prodId);
        Task<ComImpInvoiceInfoCreateViewModel> GetProductsByAsync(string search, int lcId, int page);
    }
}
