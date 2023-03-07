using System;
using System.Collections.Generic;
using DenimERP.Models;

namespace DenimERP.ViewModels
{
    public class FChemStoreReceiveViewModel
    {
        public FChemStoreReceiveViewModel()
        {
            FChemStoreReceiveMasterList = new List<F_CHEM_STORE_RECEIVE_MASTER>();
            FChemStoreReceiveDetailsList = new List<F_CHEM_STORE_RECEIVE_DETAILS>();
            FBasReceiveTypesList = new List<F_BAS_RECEIVE_TYPE>();
            ComImpInvoiceInfoList = new List<COM_IMP_INVOICEINFO>();
            ComImpCnfinfosList = new List<COM_IMP_CNFINFO>();
            FChemStoreProductinfosList = new List<F_CHEM_STORE_PRODUCTINFO>();
            FChemStoreIndentmastersList = new List<F_CHEM_STORE_INDENTMASTER>();
            Countrieses = new List<COUNTRIES>();
            BasSupplierinfos = new List<BAS_SUPPLIERINFO>();
            ComImpLcinformations = new List<COM_IMP_LCINFORMATION>();
            RcvEmployees = new List<F_HRD_EMPLOYEE>();
            CheckEmployees = new List<F_HRD_EMPLOYEE>();
            BasTransportinfos = new List<BAS_TRANSPORTINFO>();

            FChemStoreReceiveMaster = new F_CHEM_STORE_RECEIVE_MASTER
            {
                RCVDATE = DateTime.Now
            };

            FChemStoreReceiveDetails = new F_CHEM_STORE_RECEIVE_DETAILS
            {
                TRNSDATE = DateTime.Now
            };
        }

        public F_CS_CHEM_RECEIVE_REPORT FCsChemReceiveReport { get; set; }
        public F_CHEM_TRANSECTION FChemTransection { get; set; }
        public F_CHEM_STORE_RECEIVE_MASTER FChemStoreReceiveMaster { get; set; }
        public F_CHEM_STORE_RECEIVE_DETAILS FChemStoreReceiveDetails { get; set; }
        public List<F_CHEM_STORE_RECEIVE_MASTER> FChemStoreReceiveMasterList { get; set; }
        public List<F_CHEM_STORE_RECEIVE_DETAILS> FChemStoreReceiveDetailsList { get; set; }
        public List<F_BAS_RECEIVE_TYPE> FBasReceiveTypesList { get; set; }
        public List<COM_IMP_INVOICEINFO> ComImpInvoiceInfoList { get; set; }
        public List<COM_IMP_CNFINFO> ComImpCnfinfosList { get; set; }
        public List<F_CHEM_STORE_PRODUCTINFO> FChemStoreProductinfosList { get; set; }
        public List<F_CHEM_STORE_INDENTMASTER> FChemStoreIndentmastersList { get; set; }
        public List<COUNTRIES> Countrieses { get; set; }
        public List<BAS_SUPPLIERINFO> BasSupplierinfos { get; set; }
        public List<COM_IMP_LCINFORMATION> ComImpLcinformations { get; set; }
        public List<F_HRD_EMPLOYEE> RcvEmployees { get; set; }
        public List<F_HRD_EMPLOYEE> CheckEmployees { get; set; }
        public List<BAS_TRANSPORTINFO> BasTransportinfos { get; set; }

        public int RemoveIndex { get; set; }
        public bool IsDelete { get; set; }
    }
}
