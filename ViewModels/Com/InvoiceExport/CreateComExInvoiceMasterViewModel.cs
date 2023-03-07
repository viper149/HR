using System.Collections.Generic;
using DenimERP.Models;

namespace DenimERP.ViewModels.Com.InvoiceExport
{
    public class CreateComExInvoiceMasterViewModel
    {
        public CreateComExInvoiceMasterViewModel()
        {
            ComExInvdetailses = new List<COM_EX_INVDETAILS>();
        }

        public COM_EX_INVOICEMASTER ComExInvoicemaster { get; set; }
        public COM_EX_INVDETAILS ComExInvdetails { get; set; }

        public List<COM_EX_INVDETAILS> ComExInvdetailses { get; set; }
    }
}
