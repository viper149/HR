using System;
using System.Collections.Generic;
using DenimERP.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DenimERP.ViewModels
{
    public class YarnReceiveViewModel
    {
        public YarnReceiveViewModel()
        {
            FYsYarnReceiveDetailList = new List<F_YS_YARN_RECEIVE_DETAILS>();
            FYsYarnReceiveMasterList = new List<F_YS_YARN_RECEIVE_MASTER>();
            BasYarnCountInfoList = new List<BAS_YARN_COUNTINFO>();
            FYsLocationList = new List<F_YS_LOCATION>();
            FYsLadgerList = new List<F_YS_LEDGER>();
            ComImpInvoiceInfoList = new List<COM_IMP_INVOICEINFO>();
            FBasReceiveTypeList = new List<F_BAS_RECEIVE_TYPE>();
            FYarnLotinfoList = new List<BAS_YARN_LOTINFO>();
            FYsIndentMastersList = new List<F_YS_INDENT_MASTER>();
            FYsYarnReceiveReports = new List<F_YS_YARN_RECEIVE_REPORT>();
            FYsYarnReceiveMasters = new List<F_YS_YARN_RECEIVE_MASTER>();
            RndProductionOrders = new List<RND_PRODUCTION_ORDER>();
            FBasSections = new List<F_BAS_SECTION>();
            BasSupplierinfos = new List<BAS_SUPPLIERINFO>();
            SecList= new List<F_BAS_SECTION>();
            SubSecList = new List<F_BAS_SUBSECTION>();
            FYsYarnReceiveMaster = new F_YS_YARN_RECEIVE_MASTER
            {
                YRCVDATE = DateTime.Now,
                CHALLANDATE = DateTime.Now,
                G_ENTRY_DATE = DateTime.Now
            };

            FYsYarnReceiveDetail = new F_YS_YARN_RECEIVE_DETAILS
            {
                TRNSDATE = DateTime.Now
            };
        }

        public F_YS_YARN_RECEIVE_MASTER FYsYarnReceiveMaster { get; set; }
        public F_YS_YARN_RECEIVE_DETAILS FYsYarnReceiveDetail { get; set; }
        public F_YARN_TRANSACTION FYarnTransaction { get; set; }
        public List<RND_PRODUCTION_ORDER> RndProductionOrders { get; set; }
        public List<F_YS_YARN_RECEIVE_MASTER> FYsYarnReceiveMasters { get; set; }
        public List<F_YS_YARN_RECEIVE_REPORT> FYsYarnReceiveReports { get; set; }
        public List<F_YS_YARN_RECEIVE_DETAILS> FYsYarnReceiveDetailList { get; set; }
        public List<F_YS_YARN_RECEIVE_MASTER> FYsYarnReceiveMasterList { get; set; }
        public List<BAS_YARN_COUNTINFO> BasYarnCountInfoList { get; set; }
        public List<F_YS_LOCATION> FYsLocationList { get; set; }
        public List<F_YS_LEDGER> FYsLadgerList { get; set; }
        public List<YARNFOR> YarnFor { get; set; }
        public List<COM_IMP_INVOICEINFO> ComImpInvoiceInfoList { get; set; }
        public List<F_BAS_RECEIVE_TYPE> FBasReceiveTypeList { get; set; }
        public List<BAS_YARN_LOTINFO> FYarnLotinfoList { get; set; }
        public List<F_YS_RAW_PER> FYsRawPers { get; set; }
        public List<F_YS_INDENT_MASTER> FYsIndentMastersList { get; set; }
        public List<F_BAS_SECTION> FBasSections { get; set; }
        public List<BAS_SUPPLIERINFO> BasSupplierinfos { get; set; }
        public List<F_BAS_SECTION> SecList { get; set; }
        public List<F_BAS_SUBSECTION> SubSecList { get; set; }

        public SelectList IsReturnable { get; set; }
        public int RemoveIndex { get; set; }
        public bool IsDelete { get; set; }
    }
}
