using System.Collections.Generic;
using DenimERP.Models;

namespace DenimERP.ViewModels
{
    public class AccLoanManagementMViewModel
    {
        public AccLoanManagementMViewModel()
        {
            LcList = new List<COM_IMP_LCINFORMATION>();
            InvoiceList = new List<COM_IMP_INVOICEINFO>();
            BankList = new List<BAS_BEN_BANK_MASTER>();
        }

        public ACC_LOAN_MANAGEMENT_M ACC_LOAN_MANAGEMENT_M { get; set; }
        public ACC_LOAN_MANAGEMENT_D ACC_LOAN_MANAGEMENT_D { get; set; }
        public List<BAS_BEN_BANK_MASTER> BankList { get; set; }

        public List<COM_IMP_INVOICEINFO> InvoiceList { get; set; }
        public List<COM_IMP_LCINFORMATION> LcList { get; set; }

    }
}
    

