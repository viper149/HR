using System;
using System.Collections.Generic;
using DenimERP.Models;

namespace DenimERP.ViewModels
{
    public class FPrInspectionRejectionBViewModel
    {
        public FPrInspectionRejectionBViewModel()
        {
            FPrInspectionRejectionB = new F_PR_INSPECTION_REJECTION_B
            {
                TRANS_DATE = DateTime.Now
            };

            FPrInspectionDefectinfoList = new List<F_PR_INSPECTION_DEFECTINFO>();
            FHrShiftInfoList = new List<F_HR_SHIFT_INFO>();
            FBasSectionsList = new List<F_BAS_SECTION>();
        }
        public F_PR_INSPECTION_REJECTION_B FPrInspectionRejectionB { get; set; }

        public List<F_PR_INSPECTION_DEFECTINFO> FPrInspectionDefectinfoList { get; set; }
        public List<F_HR_SHIFT_INFO> FHrShiftInfoList { get; set; }
        public List<F_BAS_SECTION> FBasSectionsList { get; set; }
        public List<TypeTableViewModel> StyleSetLoomList { get; set; }
        public List<TypeTableViewModel> StyleSetLoomListEdit { get; set; }

    }
}
