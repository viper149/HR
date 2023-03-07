using System;
using System.Collections.Generic;
using DenimERP.Models;

namespace DenimERP.ViewModels
{
    public class FYsYarnIssueViewModel
    {
        public FYsYarnIssueViewModel()
        {
            YarnIssueMasterList = new List<F_YS_YARN_ISSUE_MASTER>();
            YarnIssueDetailsList = new List<F_YS_YARN_ISSUE_DETAILS>();
            IssueTypeList = new List<F_BAS_ISSUE_TYPE>();
            ReceiveTypeList = new List<F_BAS_RECEIVE_TYPE>();
            CountNameList = new List<BAS_YARN_COUNTINFO>();
            SoListFromYsridAsync = new List<TypeTableViewModel>();
            FYarnReqDetailsList = new List<F_YARN_REQ_DETAILS>();
            BasUnits = new List<F_BAS_UNITS>();
            FYsLocationList = new List<F_YS_LOCATION>();
            FYsYarnReceiveDetailsList = new List<F_YS_YARN_RECEIVE_DETAILS>();

            YarnIssueMaster = new F_YS_YARN_ISSUE_MASTER
            {
                YISSUEDATE = DateTime.Now,
                ISREMARKABLE = false
            };

            YarnIssueDetails = new F_YS_YARN_ISSUE_DETAILS
            {
                TRNSDATE = DateTime.Now
            };
        }

        public F_BAS_SECTION FBaseSection{ get; set; }
        public F_YARN_REQ_MASTER FYarnReqMaster{ get; set; }
        public F_YARN_TRANSACTION FYarnTransaction { get; set; }
        public F_YS_YARN_ISSUE_MASTER YarnIssueMaster { get; set; }
        public F_YS_YARN_ISSUE_DETAILS YarnIssueDetails { get; set; }

        public List<F_YS_YARN_ISSUE_MASTER> YarnIssueMasterList { get; set; }
        public List<F_YS_YARN_ISSUE_DETAILS> YarnIssueDetailsList { get; set; }
        public List<F_YARN_REQ_DETAILS> FYarnReqDetailsList{ get; set; }
        public List<F_YARN_REQ_MASTER> FYarnReqMasterList{ get; set; }
        public List<F_BAS_ISSUE_TYPE> IssueTypeList { get; set; }
        public List<F_BAS_RECEIVE_TYPE> ReceiveTypeList { get; set; }
        public List<BAS_YARN_COUNTINFO> CountNameList { get; set; }
        public List<F_BAS_UNITS> BasUnits { get; set; }
        public List<RND_PRODUCTION_ORDER> RndProductionOrders { get; set; }
        public List<TypeTableViewModel> SoListFromYsridAsync { get; set; }
        public List<F_YS_LOCATION> FYsLocationList { get; set; }
        public List<F_YS_YARN_RECEIVE_DETAILS> FYsYarnReceiveDetailsList { get; set; }
        public int RemoveIndex { get; set; }
        public bool IsDelete { get; set; }

        public double ? TotalIssuedCount { get; set; }
    }
}
