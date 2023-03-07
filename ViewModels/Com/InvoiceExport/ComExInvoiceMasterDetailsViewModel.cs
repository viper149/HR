using System.Collections.Generic;
using DenimERP.Models;

namespace DenimERP.ViewModels.Com.InvoiceExport
{
    public class ComExInvoiceMasterDetailsViewModel
    {
        public ComExInvoiceMasterDetailsViewModel()
        {
            comExInvoicemaster = new COM_EX_INVOICEMASTER();
            ComExInvdetailses = new List<COM_EX_INVDETAILS>();
        }

        public COM_EX_INVOICEMASTER comExInvoicemaster { get; set; }
        public List<COM_EX_INVDETAILS> ComExInvdetailses { get; set; }
    }
}
