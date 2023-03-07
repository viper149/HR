using System;
using System.Collections.Generic;
using DenimERP.Models;

namespace DenimERP.ViewModels
{
    public class FQaYarnTestInformationPolyesterViewModel
    {
        public FQaYarnTestInformationPolyesterViewModel()
        {
            FQaYarnTestInformationPolyester = new F_QA_YARN_TEST_INFORMATION_POLYESTER()
            {
                TESTDATE = DateTime.Now
            };
        }

        public F_QA_YARN_TEST_INFORMATION_POLYESTER FQaYarnTestInformationPolyester { get; set; }
        public BAS_YARN_COUNTINFO BasYarnCountInfo { get; set; }
        public F_YS_YARN_RECEIVE_MASTER FYsYarnReceiveMaster { get; set; }
        public F_YS_YARN_RECEIVE_DETAILS FYsYarnReceiveDetails { get; set; }
        public F_YS_INDENT_MASTER FYsIndentMaster { get; set; }
        public List<F_YS_YARN_RECEIVE_MASTER> ReceiveMasterList { get; set; }
        public List<F_YS_YARN_RECEIVE_DETAILS> ReceiveDetailsList { get; set; }
        public List<BAS_COLOR> Colors { get; set; }
    }
}
