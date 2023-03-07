using System;
using System.Collections.Generic;
using DenimERP.Models;

namespace DenimERP.ViewModels
{
    public class ProcWorkOrderViewModel
    {
        public ProcWorkOrderViewModel()
        {
            SupplierList = new List<BAS_SUPPLIERINFO>();
            ProductList = new List<F_GS_PRODUCT_INFORMATION>();
            IndentList = new List<F_YS_INDENT_MASTER>();
            F_GS_PRODUCT_INFORMATION = new List<F_GS_PRODUCT_INFORMATION>();

            ProcWorkOrderMaster = new PROC_WORKORDER_MASTER
            {
                WODATE = DateTime.Now
            };
        }

        public PROC_WORKORDER_MASTER ProcWorkOrderMaster { get; set; }
        public PROC_WORKORDER_DETAILS ProcWorkOrderDetails { get; set; }
        public List<BAS_SUPPLIERINFO> SupplierList { get; set; }
        public List<F_GS_PRODUCT_INFORMATION> ProductList { get; set; }
        public List<F_YS_INDENT_MASTER> IndentList { get; set; }
        public List<F_GS_PRODUCT_INFORMATION> F_GS_PRODUCT_INFORMATION { get; set; }
    }

}
