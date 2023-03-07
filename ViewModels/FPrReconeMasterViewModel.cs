using System;
using System.Collections.Generic;
using DenimERP.Models;

namespace DenimERP.ViewModels
{
    public class FPrReconeMasterViewModel
    {
        public FPrReconeMasterViewModel()

        {
            SetList = new List<PL_PRODUCTION_SETDISTRIBUTION>();
            BallList = new List<F_LCB_PRODUCTION_ROPE_DETAILS>();
            LinkBallList = new List<F_LCB_PRODUCTION_ROPE_DETAILS>();
            ShiftList = new List<F_HR_SHIFT_INFO>();
            YarnCountList = new List<BAS_YARN_COUNTINFO>();
            MachineList = new List<F_LCB_MACHINE>();
            RndCountList = new List<RND_FABRIC_COUNTINFO>();

            ReconeYarnDetailsList = new List<F_PR_RECONE_YARN_DETAILS>();
            ReconeYarnConsumptionList = new List<F_PR_RECONE_YARN_CONSUMPTION>();

            F_PR_RECONE_MASTER = new F_PR_RECONE_MASTER
            {
               TRANSDATE = DateTime.Now
            };

        }
        public F_PR_RECONE_MASTER F_PR_RECONE_MASTER { get; set; }
        public F_PR_RECONE_YARN_DETAILS F_PR_RECONE_YARN_DETAILS { get; set; }
        public F_PR_RECONE_YARN_CONSUMPTION F_PR_RECONE_YARN_CONSUMPTION { get; set; }


        public List<F_PR_RECONE_YARN_DETAILS> ReconeYarnDetailsList { get; set; }
        public List<F_PR_RECONE_YARN_CONSUMPTION> ReconeYarnConsumptionList { get; set; }
        public List<PL_PRODUCTION_SETDISTRIBUTION> SetList { get; set; }
        public List<F_LCB_PRODUCTION_ROPE_DETAILS> BallList { get; set; }
        public List<F_LCB_PRODUCTION_ROPE_DETAILS> LinkBallList { get; set; }
        public List<F_HR_SHIFT_INFO> ShiftList { get; set; }
        public List<BAS_YARN_COUNTINFO> YarnCountList { get; set; }
        public List<F_LCB_MACHINE> MachineList { get; set; }
        public List<RND_FABRIC_COUNTINFO> RndCountList { get; set; }

        public int RemoveIndex { get; set; }
        public bool IsDelete { get; set; }

    }
}
