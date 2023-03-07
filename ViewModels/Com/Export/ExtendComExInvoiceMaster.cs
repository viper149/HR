using DenimERP.Models;

namespace DenimERP.ViewModels.Com.Export
{
    public class ExtendComExInvoiceMaster : COM_EX_INVOICEMASTER
    {
        public bool ReadOnly { get; set; }
        public bool DeleteOnly { get; set; }
        public bool EditOnly { get; set; }
        public bool CreateOnly { get; set; }
    }
}
