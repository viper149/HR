using System;
using System.Collections.Generic;
using DenimERP.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DenimERP.ViewModels
{
    public class YarnReceiveForOthersViewModel
    {

        public YarnReceiveForOthersViewModel()
        {
            FYsYarnReceiveDetailList = new List<F_YS_YARN_RECEIVE_DETAILS2>();
            FYsYarnReceiveMasterList = new List<F_YS_YARN_RECEIVE_MASTER2>();
            BasYarnCountInfoList = new List<BAS_YARN_COUNTINFO>();
            FYsLocationList = new List<F_YS_LOCATION>();
            FYsLadgerList = new List<F_YS_LEDGER>();
            ComImpInvoiceInfoList = new List<COM_IMP_INVOICEINFO>();
            FBasReceiveTypeList = new List<F_BAS_RECEIVE_TYPE>();
            FYarnLotinfoList = new List<BAS_YARN_LOTINFO>();
            FYsYarnReceiveMasters = new List<F_YS_YARN_RECEIVE_MASTER2>();
            RndProductionOrders = new List<RND_PRODUCTION_ORDER>();
            BasSupplierinfos = new List<BAS_SUPPLIERINFO>();
            SecList = new List<F_BAS_SECTION>();
            SubSecList = new List<F_BAS_SUBSECTION>();
            FYsYarnReceiveMaster = new F_YS_YARN_RECEIVE_MASTER2
            {
                YRCVDATE = DateTime.Now,
                CHALLANDATE = DateTime.Now,
                G_ENTRY_DATE = DateTime.Now
            };

            FYsYarnReceiveDetail = new F_YS_YARN_RECEIVE_DETAILS2
            {
                TRNSDATE = DateTime.Now
            };
        }

        public F_YS_YARN_RECEIVE_MASTER2 FYsYarnReceiveMaster { get; set; }
        public F_YS_YARN_RECEIVE_DETAILS2 FYsYarnReceiveDetail { get; set; }
        public List<RND_PRODUCTION_ORDER> RndProductionOrders { get; set; }
        public List<F_YS_YARN_RECEIVE_MASTER2> FYsYarnReceiveMasters { get; set; }
        public List<F_YS_YARN_RECEIVE_DETAILS2> FYsYarnReceiveDetailList { get; set; }
        public List<F_YS_YARN_RECEIVE_MASTER2> FYsYarnReceiveMasterList { get; set; }
        public List<BAS_YARN_COUNTINFO> BasYarnCountInfoList { get; set; }
        public List<F_YS_LOCATION> FYsLocationList { get; set; }
        public List<F_YS_LEDGER> FYsLadgerList { get; set; }
        public List<YARNFOR> YarnFor { get; set; }
        public List<COM_IMP_INVOICEINFO> ComImpInvoiceInfoList { get; set; }
        public List<F_BAS_RECEIVE_TYPE> FBasReceiveTypeList { get; set; }
        public List<BAS_YARN_LOTINFO> FYarnLotinfoList { get; set; }
        public List<F_YS_RAW_PER> FYsRawPers { get; set; }
        public List<BAS_SUPPLIERINFO> BasSupplierinfos { get; set; }
        public List<F_BAS_SECTION> SecList { get; set; }
        public List<F_BAS_SUBSECTION> SubSecList { get; set; }

        public SelectList IsReturnable { get; set; }
        public int RemoveIndex { get; set; }
        public bool IsDelete { get; set; }
    }
}

