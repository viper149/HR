using System;
using System.Collections.Generic;
using DenimERP.Models;

namespace DenimERP.ViewModels
{
    public class FGenSRequisitionViewModel
    {
        public FGenSRequisitionViewModel()
        {
            FGenSPurchaseRequisitionMastersList = new List<F_GEN_S_PURCHASE_REQUISITION_MASTER>();
            FGenSIndentdetailsesList = new List<F_GEN_S_INDENTDETAILS>();
            FGsProductInformationsList = new List<F_GS_PRODUCT_INFORMATION>();
            FGenSIndentmastersList = new List<F_GEN_S_INDENTMASTER>();
            FHrEmployeesList = new List<F_HRD_EMPLOYEE>();
            FGenSIndentTypes = new List<F_GEN_S_INDENT_TYPE>();

            FGenSPurchaseRequisitionMaster = new F_GEN_S_PURCHASE_REQUISITION_MASTER
            {
                INDSLDATE = DateTime.Now
            };

            FGenSIndentdetails = new F_GEN_S_INDENTDETAILS
            {
                TRNSDATE = DateTime.Now
            };

            FGenSIndentmaster = new F_GEN_S_INDENTMASTER
            {
                GINDDATE = DateTime.Now
            };
        }

        public F_GEN_S_PURCHASE_REQUISITION_MASTER FGenSPurchaseRequisitionMaster { get; set; }
        public F_GS_PRODUCT_INFORMATION FGsProductInformation { get; set; }
        public F_GEN_S_INDENTDETAILS FGenSIndentdetails { get; set; }
        public F_GEN_S_INDENTMASTER FGenSIndentmaster { get; set; }

        public List<F_GEN_S_PURCHASE_REQUISITION_MASTER> FGenSPurchaseRequisitionMastersList { get; set; }
        public List<F_GEN_S_INDENTMASTER> FGenSIndentmastersList { get; set; }
        public List<F_GEN_S_INDENTDETAILS> FGenSIndentdetailsesList { get; set; }
        public List<F_GS_PRODUCT_INFORMATION> FGsProductInformationsList { get; set; }
        public List<F_HRD_EMPLOYEE> FHrEmployeesList { get; set; }
        public List<F_GEN_S_INDENT_TYPE> FGenSIndentTypes { get; set; }

        public int RemoveIndex { get; set; }
        public bool IsDelete { get; set; }
    }
}
