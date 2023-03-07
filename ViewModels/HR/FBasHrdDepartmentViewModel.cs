using System.Collections.Generic;
using DenimERP.Models;

namespace DenimERP.ViewModels.HR
{
    public class FBasHrdDepartmentViewModel
    {
        public FBasHrdDepartmentViewModel()
        {
            FBasHrdLocationList = new List<F_BAS_HRD_LOCATION>();
        }

        public F_BAS_HRD_DEPARTMENT FBasHrdDepartment { get; set; }

        public List<F_BAS_HRD_LOCATION> FBasHrdLocationList { get; set; }
    }
}