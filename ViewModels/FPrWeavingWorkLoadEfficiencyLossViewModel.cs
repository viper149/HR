using System.Collections.Generic;
using DenimERP.Models;

namespace DenimERP.ViewModels
{
    public class FPrWeavingWorkLoadEfficiencyLossViewModel
    {
        public FPrWeavingWorkLoadEfficiencyLossViewModel()
        {
            LoomTypeList = new List<LOOM_TYPE>();
            EmployeeList = new List<F_HRD_EMPLOYEE>();
            ShiftList = new List<F_HR_SHIFT_INFO>();

        }

        public F_PR_WEAVING_WORKLOAD_EFFICIENCELOSS FPrWeavingWorkloadEfficienceloss { get; set; }
        public List<LOOM_TYPE> LoomTypeList { get; set; }
        public List<F_HRD_EMPLOYEE> EmployeeList { get; set; }
        public List<F_HR_SHIFT_INFO> ShiftList { get; set; }
    }
}
