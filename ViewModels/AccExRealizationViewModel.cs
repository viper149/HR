using System.Collections.Generic;
using DenimERP.Models;

namespace DenimERP.ViewModels
{
    public class AccExRealizationViewModel
    {
        public AccExRealizationViewModel()
        {
            ComExInvoiceMasters = new List<COM_EX_INVOICEMASTER>();
            LcInfoViewModel = new LcInfoViewModel();
            ComExInvdetailses = new List<COM_EX_INVDETAILS>();
        }

        public double ? TotalRealization { get; set; }
        public double ? TotalRealizationY { get; set; }
        public ACC_EXPORT_REALIZATION AccExportRealization{ get; set; }
        public COM_EX_INVDETAILS ComExInvdetails { get; set; }
        public LcInfoViewModel LcInfoViewModel { get; set; }
        public List<COM_EX_INVOICEMASTER> ComExInvoiceMasters { get; set; }
        public List<COM_EX_INVDETAILS> ComExInvdetailses { get; set; }

        
    }
}
