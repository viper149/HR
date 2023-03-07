using System;
using System.Collections.Generic;
using DenimERP.Models;

namespace DenimERP.ViewModels
{
    public class FYsYarnIssueSViewModel
    {
        public FYsYarnIssueSViewModel()
        {
            YarnIssueMasterSList = new List<F_YS_YARN_ISSUE_MASTER_S>();
            YarnIssueDetailsSList = new List<F_YS_YARN_ISSUE_DETAILS_S>();
            IssueTypeList = new List<F_BAS_ISSUE_TYPE>();
            ReceiveTypeList = new List<F_BAS_RECEIVE_TYPE>();
            CountNameList = new List<BAS_YARN_COUNTINFO>();
            SoListFromYsridAsync = new List<TypeTableViewModel>();
            FYarnReqMasterSList = new List<F_YARN_REQ_MASTER_S>();
            FYarnReqDetailsSList = new List<F_YARN_REQ_DETAILS_S>();
            BasUnits = new List<F_BAS_UNITS>();
            FYsLocationList = new List<F_YS_LOCATION>();
            FYsYarnReceiveDetailsList = new List<F_YS_YARN_RECEIVE_DETAILS_S>();

            YarnIssueMasterS = new F_YS_YARN_ISSUE_MASTER_S
            {
                YISSUEDATE = DateTime.Now,
                ISREMARKABLE = false
            };

            YarnIssueDetailsS = new F_YS_YARN_ISSUE_DETAILS_S
            {
                TRNSDATE = DateTime.Now
            };
        }

        public F_BAS_SECTION FBaseSection { get; set; }
        public F_YARN_REQ_MASTER_S FYarnReqMasterS { get; set; }
        public F_YARN_TRANSACTION_S FYarnTransactionS { get; set; }
        public F_YS_YARN_ISSUE_MASTER_S YarnIssueMasterS { get; set; }
        public F_YS_YARN_ISSUE_DETAILS_S YarnIssueDetailsS { get; set; }
        public List<F_YS_YARN_ISSUE_MASTER_S> YarnIssueMasterSList { get; set; }
        public List<F_YS_YARN_ISSUE_DETAILS_S> YarnIssueDetailsSList { get; set; }
        public List<F_YARN_REQ_DETAILS_S> FYarnReqDetailsSList { get; set; }
        public List<F_YARN_REQ_MASTER_S> FYarnReqMasterSList { get; set; }
        public List<F_BAS_ISSUE_TYPE> IssueTypeList { get; set; }
        public List<F_BAS_RECEIVE_TYPE> ReceiveTypeList { get; set; }
        public List<BAS_YARN_COUNTINFO> CountNameList { get; set; }
        public List<F_BAS_UNITS> BasUnits { get; set; }
        public List<TypeTableViewModel> SoListFromYsridAsync { get; set; }
        public List<F_YS_LOCATION> FYsLocationList { get; set; }
        public List<F_YS_YARN_RECEIVE_DETAILS_S> FYsYarnReceiveDetailsList { get; set; }

        public int RemoveIndex { get; set; }
        public bool IsDelete { get; set; }
    }
}
