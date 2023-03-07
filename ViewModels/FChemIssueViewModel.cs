using System;
using System.Collections.Generic;
using DenimERP.Models;

namespace DenimERP.ViewModels
{
    public class FChemIssueViewModel
    {
        public FChemIssueViewModel()
        {
            FChemIssueDetailsList = new List<F_CHEM_ISSUE_DETAILS>();
            FBasIssueTypesList = new List<F_BAS_ISSUE_TYPE>();
            FChemReqMastersList = new List<F_CHEM_REQ_MASTER>();
            FChemStoreProductinfos = new List<F_CHEM_STORE_PRODUCTINFO>();
            FChemReqDetailsesList = new List<F_CHEM_REQ_DETAILS>();
            BasUnits = new List<F_BAS_UNITS>();
            IssueFHrEmployees = new List<F_HRD_EMPLOYEE>();
            ReceiveFHrEmployees = new List<F_HRD_EMPLOYEE>();

            FChemIssueMaster = new F_CHEM_ISSUE_MASTER
            {
                CISSUEDATE = DateTime.Now
            };

            FChemIssueDetails = new F_CHEM_ISSUE_DETAILS
            {
                CISSDDATE = DateTime.Now
            };
        }

        public F_CHEM_TRANSECTION FChemTransection { get; set; }
        public F_CHEM_REQ_MASTER FChemReqMaster { get; set; }
        public F_CHEM_ISSUE_MASTER FChemIssueMaster { get; set; }
        public F_CHEM_ISSUE_DETAILS FChemIssueDetails { get; set; }
        public List<F_CHEM_ISSUE_DETAILS> FChemIssueDetailsList { get; set; }
        public List<F_BAS_ISSUE_TYPE> FBasIssueTypesList { get; set; }
        public List<F_CHEM_STORE_PRODUCTINFO> FChemStoreProductinfos { get; set; }
        public List<F_CHEM_REQ_MASTER> FChemReqMastersList { get; set; }
        public List<F_BAS_UNITS> BasUnits { get; set; }
        public List<F_CHEM_REQ_DETAILS> FChemReqDetailsesList { get; set; }
        public List<F_HRD_EMPLOYEE> IssueFHrEmployees { get; set; }
        public List<F_HRD_EMPLOYEE> ReceiveFHrEmployees { get; set; }

        public bool IsDelete { get; set; }
        public int RemoveIndex { get; set; }
        public string BB { get; set; }

        public double? TotalIssuedChemical { get; set; }
    }
}
