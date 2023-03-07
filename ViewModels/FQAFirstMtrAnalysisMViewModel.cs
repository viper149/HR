using System;
using System.Collections.Generic;
using DenimERP.Models;

namespace DenimERP.ViewModels
{
    public class FQAFirstMtrAnalysisMViewModel
    {
        public FQAFirstMtrAnalysisMViewModel()
        {
            FirstMtrAnalysisM = new F_QA_FIRST_MTR_ANALYSIS_M
            {
                TRANS_DATE = DateTime.Now,
                LENGTH_TRIAL = 1
            };

            EmployeeList = new List<F_HRD_EMPLOYEE>();
            BeamList = new List<F_PR_WEAVING_PROCESS_BEAM_DETAILS_B>();
            SetList = new List<PL_PRODUCTION_SETDISTRIBUTION>();
            SupplierList = new List<BAS_SUPPLIERINFO>();
            LotList = new List<BAS_YARN_LOTINFO>();
            FQaFirstMtrAnalysisDsList = new List<F_QA_FIRST_MTR_ANALYSIS_D>();
        }

        public F_QA_FIRST_MTR_ANALYSIS_M FirstMtrAnalysisM { get; set; }
        public F_QA_FIRST_MTR_ANALYSIS_D FQaFirstMtrAnalysisD { get; set; }

        public List<F_HRD_EMPLOYEE> EmployeeList { get; set; }
        public List<F_PR_WEAVING_PROCESS_BEAM_DETAILS_B> BeamList{ get; set; }
        public List<PL_PRODUCTION_SETDISTRIBUTION> SetList { get; set; }
        public List<BAS_SUPPLIERINFO> SupplierList { get; set; }
        public List<BAS_YARN_LOTINFO> LotList { get; set; }
        public List<F_QA_FIRST_MTR_ANALYSIS_D> FQaFirstMtrAnalysisDsList{ get; set; }

        public int RemoveIndex { get; set; }
        public bool IsDelete { get; set; }
    }
}
