using System;
using System.Collections.Generic;
using DenimERP.Models;

namespace DenimERP.ViewModels
{
    public class ProcBillMasterViewModel
    {
        public ProcBillMasterViewModel()
        {
            ChallanList = new List<F_GEN_S_RECEIVE_MASTER>();
            GsProductList = new List<F_GS_PRODUCT_INFORMATION>();

            PROC_BILL_MASTER = new PROC_BILL_MASTER
            {
                BILLDATE = DateTime.Now
            };
        }

        public PROC_BILL_MASTER PROC_BILL_MASTER { get; set; }

        public List<F_GEN_S_RECEIVE_MASTER> ChallanList { get; set; }
        public PROC_BILL_DETAILS PROC_BILL_DETAILS { get; set; }
        //public F_GS_INDENT_MASTER F_GS_INDENT_MASTER { get; set; }
        public List<F_GS_PRODUCT_INFORMATION> GsProductList { get; set; }

        //public F_GS_PURCHASE_REQUISITION_DETAILS F_GS_PURCHASE_REQUISITION_DETAILS { get; set; }

    }
}
