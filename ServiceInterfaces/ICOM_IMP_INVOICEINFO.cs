using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels;
using DenimERP.ViewModels.Com.InvoiceImport;

namespace DenimERP.ServiceInterfaces
{
    public interface ICOM_IMP_INVOICEINFO : IBaseService<COM_IMP_INVOICEINFO>
    {
        Task<bool> FindByInvNoAsync(string invNo);
        Task<COM_IMP_INVOICEINFO> FindByIdIncludeAllAsync(int id);
        Task<ComImpInvoiceInfoCreateViewModel> GetInitObjByAsync(ComImpInvoiceInfoCreateViewModel comImpInvoiceInfoCreateViewModel);
        Task<DataTableObject<COM_IMP_INVOICEINFO>> GetForDataTableByAsync(string sortColumn, string sortColumnDirection, string searchValue, string draw, int skip, int pageSize);
        Task<ComImpInvoiceInfoDetailsViewModel> FindBInvIdIncludeAllAsync(int invId);
        Task<ComImpInvoiceInfoEditViewModel> FindByInvIdAsync(int invId);
        Task<ComImpInvoiceInfoCreateViewModel> GetInitObjForDetailsTableByAsync(ComImpInvoiceInfoCreateViewModel comImpInvoiceInfoCreateViewModel);
        Task<dynamic> GetProductInfoByAsync(int lc_Id);
    }
}
