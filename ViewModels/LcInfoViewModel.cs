using System.Collections.Generic;
using DenimERP.Models;

namespace DenimERP.ViewModels
{
    public class LcInfoViewModel
    {
        public LcInfoViewModel()
        {
            ComExInvDetails = new List<COM_EX_INVDETAILS>();
            //ComExInvoiceMaster = new COM_EX_INVOICEMASTER();
            //ComExLcInfo = new COM_EX_LCINFO();
            //ComExGspInfo = new COM_EX_GSPINFO();
        }
        public COM_EX_GSPINFO ComExGspInfo { get; set; }
        public COM_EX_INVOICEMASTER ComExInvoiceMaster { get; set; }
        public COM_EX_LCINFO ComExLcInfo { get; set; }
        public List<COM_EX_INVDETAILS> ComExInvDetails { get; set; }
    }
}
