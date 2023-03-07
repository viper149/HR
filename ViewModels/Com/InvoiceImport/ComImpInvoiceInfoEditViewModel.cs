using System.Collections.Generic;
using DenimERP.Models;

namespace DenimERP.ViewModels.Com.InvoiceImport
{
    public class ComImpInvoiceInfoEditViewModel : ComImpInvoiceInfoCreateViewModel
    {
        public ComImpInvoiceInfoEditViewModel()
        {
            ComImpInvdetailsesForExistingList = new List<COM_IMP_INVDETAILS>();
            CnFList = new List<COM_IMP_CNFINFO>();
        }

        public List<COM_IMP_INVDETAILS> ComImpInvdetailsesForExistingList { get; set; }
        public List<COM_IMP_CNFINFO> CnFList { get; set; }
    }
}
