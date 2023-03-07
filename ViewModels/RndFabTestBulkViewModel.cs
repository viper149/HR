using System;
using System.Collections.Generic;
using DenimERP.Models;

namespace DenimERP.ViewModels
{
    public class RndFabTestBulkViewModel
    {
        public RndFabTestBulkViewModel()
        {
            RndFabtestBulk = new RND_FABTEST_BULK()
            {
                LTB_DATE = DateTime.Now
            };

            RndProductionOrderList = new List<RND_PRODUCTION_ORDER>();
            PlProductionSetdistributionList = new List<PL_PRODUCTION_SETDISTRIBUTION>();
            FHrEmployeeList = new List<F_HRD_EMPLOYEE>();
            FHrShiftInfoList = new List<F_HR_SHIFT_INFO>();
            FBasTestmethodList = new List<F_BAS_TESTMETHOD>();
        }

        public RND_FABTEST_BULK RndFabtestBulk { get; set; }

        public List<RND_PRODUCTION_ORDER> RndProductionOrderList { get; set; }
        public List<PL_PRODUCTION_SETDISTRIBUTION> PlProductionSetdistributionList { get; set; }
        public List<F_PR_FINISHING_FNPROCESS> FPrFinishingFnprocessList { get; set; }
        public List<F_HRD_EMPLOYEE> FHrEmployeeList { get; set; }
        public List<F_HR_SHIFT_INFO> FHrShiftInfoList { get; set; }
        public List<F_BAS_TESTMETHOD> FBasTestmethodList { get; set; }
    }
}
