using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels;
using DenimERP.ViewModels.Com.Export;
using DenimERP.ViewModels.Com.InvoiceExport;

namespace DenimERP.ServiceInterfaces
{
    public interface ICOM_EX_INVOICEMASTER : IBaseService<COM_EX_INVOICEMASTER>
    {
        Task<ComExInvoiceMasterCreateViewModel> GetInitObjByAsync(ComExInvoiceMasterCreateViewModel comExInvoiceMasterCreateViewModel);
        Task<ComExInvoiceMasterGetBuyerAndPDocNoViewModel> GetBuyerAndPDocNumber(int lcId);
        Task<bool> FindByInvNoAsync(string invNo);
        Task<COM_EX_INVOICEMASTER> FindByIdIncludeAllAsync(int invId);
        Task<COM_EX_INVOICEMASTER> FindByIdIncludeAsync(int invId);
        Task<COM_EX_INVOICEMASTER> FindByIdIncludeAllNotRealizedAsync(int invId);
        Task<IEnumerable<COM_EX_INVOICEMASTER>> GetComExInvoiceMasterBy(string searchBy);
        Task<DataTableObject<ExtendComExInvoiceMaster>> GetForDataTableByAsync(string sortColumn, string sortColumnDirection, string searchValue, string draw, int skip, int pageSize);
        Task<IEnumerable<ExtendComExInvoiceMaster>> GetAllAsync();
        Task<IEnumerable<ComExInvoiceMasterInvoiceListViewModel>> GetInvoices(COM_EX_LCINFO comExLcinfo);
        Task<IEnumerable<ComExInvoiceMasterPiListViewModel>> GetPI(COM_EX_LCINFO comExLcinfo);
        Task<ComExInvoiceMasterDetailsViewModel> FindByInvIdWithOutRealizedAsync(int invId);
        Task<dynamic> GetFabricStylesByAsync(int lcId);
        Task<ComExInvoiceMasterCreateViewModel> FindByinvIdIncludeAllAsync(int invId);
        Task<bool> HasBalanceByAsync(int lcId, double? amount);
        Task<string> GetLastInvoiceNoAsync();
        Task<COM_EX_INVOICEMASTER> FindByIdForDeleteAsync(int invId);
        Task<CreateComExInvoiceMasterViewModel> FindByInvIdIncludeAllAsync(int invId);
        Task<IEnumerable<COM_EX_INVOICEMASTER>> GetInvoiceList();
    }
}
