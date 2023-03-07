using System;
using System.Collections.Generic;
using DenimERP.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DenimERP.ViewModels.Rnd
{
    public class RndYarnRequisitionViewModel
    {
        public RndYarnRequisitionViewModel()
        {
            YarnforList = new List<YARNFOR>();
            BasUnits = new List<F_BAS_UNITS>();
            FHrEmployeeList = new List<F_HRD_EMPLOYEE>();
            FHrEmployeeListRef = new List<F_HRD_EMPLOYEE>();
            FBasSectionList = new List<F_BAS_SECTION>();
            ComExPimasters = new List<COM_EX_PIMASTER>();
            FBasDepartments = new List<F_BAS_DEPARTMENT>();
            BasYarnCountInfos = new List<BAS_YARN_COUNTINFO>();
            FysIndentDetailList = new List<F_YS_INDENT_DETAILS>();
            RndSampleInfoDyeingList = new List<RND_SAMPLE_INFO_DYEING>();
            RndPurchaseRequisitionMasterList = new List<RND_PURCHASE_REQUISITION_MASTER>();
            FysIndentMasterList = new List<F_YS_INDENT_MASTER>();
            CosPrecostingMasters = new List<COS_PRECOSTING_MASTER>();
            Yarnfroms = new List<YARNFROM>();
            BasBuyerinfos = new List<BAS_BUYERINFO>();
            RndFabricinfos = new List<RND_FABRICINFO>();

            FysIndentDetails = new F_YS_INDENT_DETAILS
            {
                TRNSDATE = DateTime.Now,
                ETR = DateTime.Now
            };

            RndPurchaseRequisitionMaster = new RND_PURCHASE_REQUISITION_MASTER
            {
                INDSLDATE = DateTime.Now
            };
        }

        public RND_PURCHASE_REQUISITION_MASTER RndPurchaseRequisitionMaster { get; set; }
        public COM_EX_PI_DETAILS ComExPiDetails { get; set; }
        public COM_EX_PIMASTER ComExPimaster { get; set; }
        public F_YS_INDENT_MASTER FysIndentMaster { get; set; }
        public F_YS_INDENT_DETAILS FysIndentDetails { get; set; }
        public List<RND_PURCHASE_REQUISITION_MASTER> RndPurchaseRequisitionMasterList { get; set; }
        public List<F_YS_INDENT_MASTER> FysIndentMasterList { get; set; }
        public List<F_YS_INDENT_DETAILS> FysIndentDetailList { get; set; }
        public List<F_HRD_EMPLOYEE> FHrEmployeeList { get; set; }
        public List<F_HRD_EMPLOYEE> FHrEmployeeListRef { get; set; }
        public List<F_BAS_DEPARTMENT> FBasDepartments { get; set; }
        public List<F_BAS_SECTION> FBasSectionList { get; set; }
        public List<RND_SAMPLE_INFO_DYEING> RndSampleInfoDyeingList { get; set; }
        public List<BAS_YARN_COUNTINFO> BasYarnCountInfos { get; set; }
        public List<F_BAS_UNITS> BasUnits { get; set; }
        public List<YARNFOR> YarnforList { get; set; }
        public List<BAS_YARN_LOTINFO> LotList { get; set; }
        public List<F_YS_SLUB_CODE> SlubList { get; set; }
        public List<F_YS_RAW_PER> RawList { get; set; }
        public List<YARNFROM> Yarnfroms { get; set; }
        public List<COM_EX_PIMASTER> ComExPimasters { get; set; }
        public List<RND_PRODUCTION_ORDER> PoList { get; set; }
        public List<BAS_BUYERINFO> BasBuyerinfos { get; set; }
        public List<RND_FABRICINFO> RndFabricinfos { get; set; }

        public dynamic CosPrecostingMasters { get; set; }

        public SelectList YarnFor { get; set; }
        public int RemoveIndex { get; set; }
        public bool IsDeletable { get; set; }
    }
}
