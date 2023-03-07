using System;
using System.Collections.Generic;
using DenimERP.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DenimERP.ViewModels
{
    public class FYsYarnReceiveSViewModel
    {
        public FYsYarnReceiveSViewModel()
        {
            FYsYarnReceiveDetailList = new List<F_YS_YARN_RECEIVE_DETAILS_S>();
            FYsYarnReceiveMasterList = new List<F_YS_YARN_RECEIVE_MASTER_S>();
            BasYarnCountInfoList = new List<BAS_YARN_COUNTINFO>();
            FYsLocationList = new List<F_YS_LOCATION>();
            FYsLadgerList = new List<F_YS_LEDGER>();
            ComImpInvoiceInfoList = new List<COM_IMP_INVOICEINFO>();
            FBasReceiveTypeList = new List<F_BAS_RECEIVE_TYPE>();
            FYarnLotinfoList = new List<BAS_YARN_LOTINFO>();
            FYsIndentMastersList = new List<F_YS_INDENT_MASTER>();
            FYsYarnReceiveReports = new List<F_YS_YARN_RECEIVE_REPORT_S>();
            RndProductionOrders = new List<RND_PRODUCTION_ORDER>();
            FBasSections = new List<F_BAS_SECTION>();
            BasSupplierinfos = new List<BAS_SUPPLIERINFO>();

            FYsYarnReceiveMaster = new F_YS_YARN_RECEIVE_MASTER_S
            {
                YRCVDATE = DateTime.Now,
                CHALLANDATE = DateTime.Now,
                G_ENTRY_DATE = DateTime.Now
            };

            FYsYarnReceiveDetail = new F_YS_YARN_RECEIVE_DETAILS_S
            {
                TRNSDATE = DateTime.Now
            };
        }

        public F_YS_YARN_RECEIVE_MASTER_S FYsYarnReceiveMaster { get; set; }
        public F_YS_YARN_RECEIVE_DETAILS_S FYsYarnReceiveDetail { get; set; }
        public F_YARN_TRANSACTION_S FYarnTransaction { get; set; }
        public List<RND_PRODUCTION_ORDER> RndProductionOrders { get; set; }
        public List<F_YS_YARN_RECEIVE_REPORT_S> FYsYarnReceiveReports { get; set; }
        public List<F_YS_YARN_RECEIVE_DETAILS_S> FYsYarnReceiveDetailList { get; set; }
        public List<F_YS_YARN_RECEIVE_MASTER_S> FYsYarnReceiveMasterList { get; set; }
        public List<BAS_YARN_COUNTINFO> BasYarnCountInfoList { get; set; }
        public List<F_YS_LOCATION> FYsLocationList { get; set; }
        public List<F_YS_LEDGER> FYsLadgerList { get; set; }
        public List<COM_IMP_INVOICEINFO> ComImpInvoiceInfoList { get; set; }
        public List<F_BAS_RECEIVE_TYPE> FBasReceiveTypeList { get; set; }
        public List<BAS_YARN_LOTINFO> FYarnLotinfoList { get; set; }
        public List<F_YS_RAW_PER> FYsRawPers { get; set; }
        public List<F_YS_INDENT_MASTER> FYsIndentMastersList { get; set; }
        public List<F_BAS_SECTION> FBasSections { get; set; }
        public List<BAS_SUPPLIERINFO> BasSupplierinfos { get; set; }

        public SelectList IsReturnable { get; set; }
        public int RemoveIndex { get; set; }
        public bool IsDelete { get; set; }
    }
}
