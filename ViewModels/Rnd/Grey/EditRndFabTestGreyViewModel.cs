using System.Collections.Generic;
using DenimERP.Models;

namespace DenimERP.ViewModels.Rnd.Grey
{
    public class EditRndFabTestGreyViewModel
    {
        public EditRndFabTestGreyViewModel()
        {
            WashedByFHrEmployees = new List<F_HRD_EMPLOYEE>();
        }

        public RND_FABTEST_GREY RndFabtestGrey { get; set; }
        public List<RND_SAMPLE_INFO_WEAVING_DETAILS> RndSampleInfoWeavingDetailses { get; set; }
        public List<RND_SAMPLE_INFO_DETAILS> RndSampleInfoDetailses { get; set; }
        public List<F_HRD_EMPLOYEE> WashedByFHrEmployees { get; set; }
        public List<F_HR_SHIFT_INFO> FHrShiftInfoList { get; set; }
    }
}
