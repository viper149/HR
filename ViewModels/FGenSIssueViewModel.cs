using System;
using System.Collections.Generic;
using DenimERP.Models;

namespace DenimERP.ViewModels
{
    public class FGenSIssueViewModel
    {
        public FGenSIssueViewModel()
        {
            FGenSIssueDetailsesList = new List<F_GEN_S_ISSUE_DETAILS>();
            FBasIssueTypesList = new List<F_BAS_ISSUE_TYPE>();
            FGenSReqMastersList = new List<F_GEN_S_REQ_MASTER>();
            FGsProductInformationsList = new List<F_GS_PRODUCT_INFORMATION>();
            FGenSReqDetailsesList = new List<F_GEN_S_REQ_DETAILS>();
            BasUnits = new List<F_BAS_UNITS>();
            IssueFHrEmployees = new List<F_HRD_EMPLOYEE>();
            ReceiveFHrEmployees = new List<F_HRD_EMPLOYEE>();

            FGenSIssueMaster = new F_GEN_S_ISSUE_MASTER
            {
                GISSUEDATE = DateTime.Now
            };

            FGenSIssueDetails = new F_GEN_S_ISSUE_DETAILS
            {
                GISSDDATE = DateTime.Now
            };
        }

        public F_GEN_S_REQ_MASTER FGenSReqMaster { get; set; }
        public F_GEN_S_ISSUE_MASTER FGenSIssueMaster { get; set; }
        public F_GEN_S_ISSUE_DETAILS FGenSIssueDetails { get; set; }

        public List<F_GEN_S_ISSUE_DETAILS> FGenSIssueDetailsesList { get; set; }
        public List<F_BAS_ISSUE_TYPE> FBasIssueTypesList { get; set; }
        public List<F_GS_PRODUCT_INFORMATION> FGsProductInformationsList { get; set; }
        public List<F_GEN_S_REQ_MASTER> FGenSReqMastersList { get; set; }
        public List<F_BAS_UNITS> BasUnits { get; set; }
        public List<F_GEN_S_REQ_DETAILS> FGenSReqDetailsesList { get; set; }
        public List<F_HRD_EMPLOYEE> IssueFHrEmployees { get; set; }
        public List<F_HRD_EMPLOYEE> ReceiveFHrEmployees { get; set; }

        public bool IsDelete { get; set; }
        public int RemoveIndex { get; set; }
    }
}
