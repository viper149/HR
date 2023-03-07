using System;
using System.Collections.Generic;
using DenimERP.Models;

namespace DenimERP.ViewModels
{
    public class ComImpWorkOrderViewModel
    {
        public ComImpWorkOrderViewModel()
        {
            ComImpWorkOrderMaster = new COM_IMP_WORK_ORDER_MASTER
            {
                WODATE = DateTime.Now
            };

            ComImpWorkOrderDetailsList = new List<COM_IMP_WORK_ORDER_DETAILS>();
            BasSupplierinfoList = new List<BAS_SUPPLIERINFO>();
            BasYarnCountinfoList = new List<BAS_YARN_COUNTINFO>();
            FYsIndentMasterList = new List<F_YS_INDENT_MASTER>();
        }
        public COM_IMP_WORK_ORDER_MASTER ComImpWorkOrderMaster { get; set; }
        public COM_IMP_WORK_ORDER_DETAILS ComImpWorkOrderDetails { get; set; }

        public List<COM_IMP_WORK_ORDER_DETAILS> ComImpWorkOrderDetailsList { get; set; }
        public List<BAS_SUPPLIERINFO> BasSupplierinfoList { get; set; }
        public List<BAS_YARN_COUNTINFO> BasYarnCountinfoList { get; set; }
        public List<F_YS_INDENT_MASTER> FYsIndentMasterList { get; set; }

        public bool IsDelete { get; set; }
        public int RemoveIndex { get; set; }
    }
}
