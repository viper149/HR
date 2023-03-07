using System.Collections.Generic;
using DenimERP.Models;
using DenimERP.ViewModels.Factory;

namespace DenimERP.ViewModels
{
    public class FGsReturnRcvGatePassViewModel
    {

        public FGsReturnRcvGatePassViewModel()
        {
            FGsReturnableGpRcvMList = new List<F_GS_RETURNABLE_GP_RCV_M>();
            FGsReturnableGpRcvDList = new List<F_GS_RETURNABLE_GP_RCV_D>();
            FGsGatepassInformationMs = new List<F_GS_GATEPASS_INFORMATION_M>();
            GetFHrEmployeeViewModels = new List<GetFHrEmployeeViewModel>();
        }

        public F_GS_RETURNABLE_GP_RCV_M FGsReturnableGpRcvM { get; set; }
        public List<F_GS_RETURNABLE_GP_RCV_M> FGsReturnableGpRcvMList { get; set; }
        public F_GS_RETURNABLE_GP_RCV_D FGsReturnableGpRcvD { get; set; }
        public List<F_GS_RETURNABLE_GP_RCV_D> FGsReturnableGpRcvDList { get; set; }
        public List<F_GS_GATEPASS_INFORMATION_M> FGsGatepassInformationMs { get; set; }
        public List<GetFHrEmployeeViewModel> GetFHrEmployeeViewModels { get; set; }
    }
}
