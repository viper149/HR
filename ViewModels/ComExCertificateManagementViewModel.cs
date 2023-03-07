using System.Collections.Generic;
using DenimERP.Models;

namespace DenimERP.ViewModels
{
    public class ComExCertificateManagementViewModel
    {
        public ComExCertificateManagementViewModel()
        {
            ComExInvoicemasterList = new List<COM_EX_INVOICEMASTER>();

            ComExLcList = new List<COM_EX_LCINFO>();
        }
        public List<COM_EX_INVOICEMASTER> ComExInvoicemasterList { get; set; }

        public List<COM_EX_LCINFO> ComExLcList { get; set; }

        public COM_EX_CERTIFICATE_MANAGEMENT ComExCertificateManagement { get; set; }


    }

    
}
