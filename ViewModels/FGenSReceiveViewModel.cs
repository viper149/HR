using System;
using System.Collections.Generic;
using DenimERP.Models;

namespace DenimERP.ViewModels
{
    public class FGenSReceiveViewModel
    {
        public FGenSReceiveViewModel()
        {
            FGenSReceiveMastersList = new List<F_GEN_S_RECEIVE_MASTER>();
            FGenSReceiveDetailsesList = new List<F_GEN_S_RECEIVE_DETAILS>();
            FBasReceiveTypesList = new List<F_BAS_RECEIVE_TYPE>();
            ComImpInvoiceInfoList = new List<COM_IMP_INVOICEINFO>();
            ComImpCnfinfosList = new List<COM_IMP_CNFINFO>();
            FGsProductInformationsList = new List<F_GS_PRODUCT_INFORMATION>();
            FGenSIndentmastersList = new List<F_GEN_S_INDENTMASTER>();
            FGenSIndentdetailsesList = new List<F_GEN_S_INDENTDETAILS>();
            CountriesList = new List<COUNTRIES>();
            BasSupplierinfos = new List<BAS_SUPPLIERINFO>();
            ComImpLcdetailsesList = new List<COM_IMP_LCDETAILS>();
            RcvEmployees = new List<F_HRD_EMPLOYEE>();
            CheckEmployees = new List<F_HRD_EMPLOYEE>();
            BasTransportinfos = new List<BAS_TRANSPORTINFO>();

            FGenSReceiveMaster = new F_GEN_S_RECEIVE_MASTER
            {
                RCVDATE = DateTime.Now
            };

            FGenSReceiveDetails = new F_GEN_S_RECEIVE_DETAILS
            {
                TRNSDATE = DateTime.Now
            };
        }

        public F_GEN_S_RECEIVE_MASTER FGenSReceiveMaster { get; set; }
        public F_GEN_S_RECEIVE_DETAILS FGenSReceiveDetails { get; set; }
        public F_GEN_S_QC_APPROVE FGenSQcApprove { get; set; }
        public F_GEN_S_MRR FGenSMrr { get; set; }
        public List<F_GEN_S_RECEIVE_MASTER> FGenSReceiveMastersList { get; set; }
        public List<F_GEN_S_RECEIVE_DETAILS> FGenSReceiveDetailsesList { get; set; }
        public List<F_BAS_RECEIVE_TYPE> FBasReceiveTypesList { get; set; }
        public List<COM_IMP_INVOICEINFO> ComImpInvoiceInfoList { get; set; }
        public List<COM_IMP_CNFINFO> ComImpCnfinfosList { get; set; }
        public List<F_GS_PRODUCT_INFORMATION> FGsProductInformationsList { get; set; }
        public List<F_GEN_S_INDENTMASTER> FGenSIndentmastersList { get; set; }
        public List<F_GEN_S_INDENTDETAILS> FGenSIndentdetailsesList { get; set; }
        public List<COUNTRIES> CountriesList { get; set; }
        public List<BAS_SUPPLIERINFO> BasSupplierinfos { get; set; }
        public List<COM_IMP_LCDETAILS> ComImpLcdetailsesList { get; set; }
        public List<F_HRD_EMPLOYEE> RcvEmployees { get; set; }
        public List<F_HRD_EMPLOYEE> CheckEmployees { get; set; }
        public List<BAS_TRANSPORTINFO> BasTransportinfos { get; set; }

        public int RemoveIndex { get; set; }
        public bool IsDelete { get; set; }
    }
}
