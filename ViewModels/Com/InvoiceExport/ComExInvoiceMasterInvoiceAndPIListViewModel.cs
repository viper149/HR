using System.Collections.Generic;

namespace DenimERP.ViewModels.Com.InvoiceExport
{
    public class ComExInvoiceMasterInvoiceAndPIListViewModel
    {
        public ComExInvoiceMasterInvoiceAndPIListViewModel()
        {
            ComExInvoiceMasterInvoiceListViewModels = new List<ComExInvoiceMasterInvoiceListViewModel>();
            ComExInvoiceMasterPiListViewModels = new List<ComExInvoiceMasterPiListViewModel>();
        }

        public List<ComExInvoiceMasterInvoiceListViewModel> ComExInvoiceMasterInvoiceListViewModels { get; set; }
        public List<ComExInvoiceMasterPiListViewModel> ComExInvoiceMasterPiListViewModels { get; set; }

    }
}
