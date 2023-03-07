using System;
using System.Collections.Generic;
using DenimERP.Models;

namespace DenimERP.ViewModels
{
    public class RndFabTestSampleViewModel
    {
        public RndFabTestSampleViewModel()
        {
            RndFabtestSample = new RND_FABTEST_SAMPLE()
            {
                LTSDATE = DateTime.Now
            };
        }

        public RND_FABTEST_SAMPLE RndFabtestSample { get; set; }

        public List<F_HRD_EMPLOYEE> FHrEmployees { get; set; }
        public List<RND_SAMPLEINFO_FINISHING> RndSampleInfoFinishings { get; set; }
        public List<F_PR_FIN_TROLLY> Trollies { get; set; }
        public List<F_HR_SHIFT_INFO> FHrShiftInfoList { get; set; }
        public List<F_BAS_TESTMETHOD> FBasTestmethodList { get; set; }
        public List<F_LOOM_MACHINE_NO> FLoomMachineNoList { get; set; }
        public List<PL_PRODUCTION_SETDISTRIBUTION> PlProductionSetdistributionsList { get; set; }
    }
}
