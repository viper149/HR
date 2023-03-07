using System.Collections.Generic;
using DenimERP.Models;

namespace DenimERP.ViewModels.Com.InvoiceImport
{
    public class ComImpInvoiceInfoDetailsViewModel
    {
        public ComImpInvoiceInfoDetailsViewModel()
        {
            ComImpInvdetailses = new List<COM_IMP_INVDETAILS>();
        }

        public COM_IMP_INVOICEINFO ComImpInvoiceinfo { get; set; }
        public COM_IMP_INVDETAILS ComImpInvdetails { get; set; }
        public BAS_PRODUCTINFO BasProductinfo { get; set; }

        public List<COM_IMP_INVDETAILS> ComImpInvdetailses { get; set; }
    }
}
