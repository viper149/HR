using System;
using System.Collections.Generic;
using DenimERP.Models;

namespace DenimERP.ViewModels
{
    public class FChemicalRequisitionViewModel
    {
        public FChemicalRequisitionViewModel()
        {
            FChemPurchaseRequisitionMasterList = new List<F_CHEM_PURCHASE_REQUISITION_MASTER>();
            FChemStoreIndentdetailsList = new List<F_CHEM_STORE_INDENTDETAILS>();
            FChemStoreProductinfoList = new List<F_CHEM_STORE_PRODUCTINFO>();
            FChemStoreIndentmasterList = new List<F_CHEM_STORE_INDENTMASTER>();
            FHrEmployeesList = new List<F_HRD_EMPLOYEE>();
            BasUnits = new List<F_BAS_UNITS>();
            DepartmentList = new List<F_BAS_DEPARTMENT>();
            FBasSectionList = new List<F_BAS_SECTION>();
            FBasSubsections = new List<F_BAS_SUBSECTION>();
            FChemStoreIndentTypes = new List<F_CHEM_STORE_INDENT_TYPE>();

            FChemPurchaseRequisitionMaster = new F_CHEM_PURCHASE_REQUISITION_MASTER
            {
                INDSLDATE = DateTime.Now
            };

            FChemStoreIndentdetails = new F_CHEM_STORE_INDENTDETAILS
            {
                TRNSDATE = DateTime.Now
            };

            FChemStoreIndentmaster = new F_CHEM_STORE_INDENTMASTER
            {
                CINDDATE = DateTime.Now
            };
        }

        public F_CHEM_PURCHASE_REQUISITION_MASTER FChemPurchaseRequisitionMaster { get; set; }
        public F_CHEM_STORE_PRODUCTINFO FChemStoreProductinfo { get; set; }
        public F_CHEM_STORE_INDENTDETAILS FChemStoreIndentdetails { get; set; }
        public F_CHEM_STORE_INDENTMASTER FChemStoreIndentmaster { get; set; }
        public List<F_CHEM_PURCHASE_REQUISITION_MASTER> FChemPurchaseRequisitionMasterList { get; set; }
        public List<F_CHEM_STORE_INDENTMASTER> FChemStoreIndentmasterList { get; set; }
        public List<F_CHEM_STORE_INDENTDETAILS> FChemStoreIndentdetailsList { get; set; }
        public List<F_CHEM_STORE_PRODUCTINFO> FChemStoreProductinfoList { get; set; }
        public List<F_BAS_DEPARTMENT> DepartmentList { get; set; }
        public List<F_BAS_SECTION> FBasSectionList { get; set; }
        public List<F_BAS_SUBSECTION> FBasSubsections { get; set; }
        public List<F_HRD_EMPLOYEE> FHrEmployeesList { get; set; }
        public List<F_BAS_UNITS> BasUnits { get; set; }
        public List<F_CHEM_STORE_INDENT_TYPE> FChemStoreIndentTypes { get; set; }

        public int RemoveIndex { get; set; }
        public bool IsDelete { get; set; }
    }
}
