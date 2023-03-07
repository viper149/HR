using System;
using System.Collections.Generic;
using DenimERP.Models;

namespace DenimERP.ViewModels.Rnd.Grey
{
    public class CreateRndFabTestGreyViewModel
    {
        public CreateRndFabTestGreyViewModel()
        {
            PlProductionSetdistributionsList = new List<PL_PRODUCTION_SETDISTRIBUTION>();
            FHrEmployees = new List<F_HRD_EMPLOYEE>();
            RndFabtestGrey = new RND_FABTEST_GREY()
            {
                LTGDATE = DateTime.Now
            };
        }

        public RND_FABTEST_GREY RndFabtestGrey { get; set; }
        public List<PL_PRODUCTION_SETDISTRIBUTION> PlProductionSetdistributionsList { get; set; }
        public List<F_HRD_EMPLOYEE> FHrEmployees { get; set; }
        public List<F_PR_WEAVING_PROCESS_DETAILS_B> Doffs { get; set; }
        public List<RND_ORDER_REPEAT> OrderRepeats { get; set; }
        public List<RND_SAMPLE_INFO_WEAVING_DETAILS> RndSampleInfoWeavingDetailses { get; set; }
        public List<RND_SAMPLE_INFO_DETAILS> RndSampleInfoDetailses { get; set; }
        public List<F_HR_SHIFT_INFO> FHrShiftInfoList { get; set; }
    }
}
