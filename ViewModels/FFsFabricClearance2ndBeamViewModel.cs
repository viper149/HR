using System.Collections.Generic;
using DenimERP.Models;

namespace DenimERP.ViewModels
{
    public class FFsFabricClearance2ndBeamViewModel
    {
        public FFsFabricClearance2ndBeamViewModel()
        {

            BEAMList = new List<F_PR_WEAVING_PROCESS_DETAILS_B>();
            EMPList = new List<F_HRD_EMPLOYEE>();
            LBTESTList = new List<RND_FABTEST_BULK>();
            LGTESTList = new List<RND_FABTEST_GREY>();
            ORDERNONavigationList = new List<RND_PRODUCTION_ORDER>();
            SETList = new List<PL_PRODUCTION_SETDISTRIBUTION>();
            TROLLYNONavigationList = new List<F_PR_FIN_TROLLY>();
            TTList = new List<F_FS_FABRIC_TYPE>();

        }

        public F_FS_FABRIC_CLEARENCE_2ND_BEAM FFsFabricClearence2NdBeam { get; set; }

        public List<F_PR_WEAVING_PROCESS_DETAILS_B> BEAMList { get; set; }
        public List<F_HRD_EMPLOYEE> EMPList { get; set; }
        public List<RND_FABTEST_BULK> LBTESTList{ get; set; }
        public List<RND_FABTEST_GREY> LGTESTList { get; set; }
        public List<RND_PRODUCTION_ORDER> ORDERNONavigationList { get; set; }
        public List<PL_PRODUCTION_SETDISTRIBUTION> SETList { get; set; }
        public List<F_PR_FIN_TROLLY> TROLLYNONavigationList { get; set; }
        public List<TypeTableViewModel> FinishMachineList { get; set; }
        public List<F_FS_FABRIC_TYPE> TTList { get; set; }
    }
}
