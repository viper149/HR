using System.Collections.Generic;
using DenimERP.Models;

namespace DenimERP.ViewModels.Com
{
    public class ComExGspInfoViewModel
    {
        public ComExGspInfoViewModel()
        {
            ComExInvoiceMasters = new List<COM_EX_INVOICEMASTER>();
        }

        public COM_EX_GSPINFO ComExGspInfo { get; set; }

        public List<COM_EX_INVOICEMASTER> ComExInvoiceMasters { get; set; }
    }
}
