using System;
using System.Collections.Generic;
using DenimERP.Models;

namespace DenimERP.ViewModels
{
    public class RndFabTestSampleBulkViewModel
    {
        public RndFabTestSampleBulkViewModel()
        {
            RndFabtestSampleBulk = new RND_FABTEST_SAMPLE_BULK()
            {
                LTSGDATE = DateTime.Now
            };
        }
        public RND_FABTEST_SAMPLE_BULK RndFabtestSampleBulk { get; set; }
        public List<F_HR_SHIFT_INFO> FHrShiftInfoList { get; set; }
        public List<RND_SAMPLEINFO_FINISHING> RndSampleinfoFinishingList { get; set; }
        public List<F_HRD_EMPLOYEE> FHrEmployeeList { get; set; }
        public List<F_BAS_TESTMETHOD> FBasTestmethodList { get; set; }
        public List<F_LOOM_MACHINE_NO> FLoomMachineNoList { get; set; }
        public List<F_PR_FIN_TROLLY> FPrFinTrollyList { get; set; }
    }
}
