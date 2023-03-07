using System;
using System.Collections.Generic;
using DenimERP.Models;

namespace DenimERP.ViewModels
{
    public class FYsGpViewModel
    {
        public FYsGpViewModel()
        {
            LcInformationList = new List<COM_IMP_LCINFORMATION>();

            FYsPartyInfoList = new List<F_YS_PARTY_INFO>();

            BasYarnCountInfoList = new List<BAS_YARN_COUNTINFO>();

            FYsLocationList = new List<F_YS_LOCATION>();

            FYsLedgerList = new List<F_YS_LEDGER>();

            fysgpdetailsList = new List<F_YS_GP_DETAILS>();

            f_YS_GP_MASTER = new F_YS_GP_MASTER
            {
                GPDATE = DateTime.Now
            };

            f_YS_GP_DETAILS = new F_YS_GP_DETAILS
            {
                TRNSDATE = DateTime.Now
            };

        }

        public F_YS_GP_MASTER f_YS_GP_MASTER  { get; set; }
        public F_YS_GP_DETAILS f_YS_GP_DETAILS { get; set; }

        public F_YS_PARTY_INFO f_YS_PARTY_INFO { get; set; }

        public BAS_YARN_COUNTINFO bAS_YARN_COUNTINFO { get; set; }
        public COM_IMP_LCINFORMATION cOM_IMP_LCINFORMATION { get; set; }

        public BAS_YARN_LOTINFO bAS_YARN_LOTINFO { get; set; }
        public F_YS_LOCATION f_YS_LOCATION { get; set; }
        public F_YS_LEDGER f_YS_LEDGER { get; set; }


        public List<COM_IMP_LCINFORMATION> LcInformationList { get; set; }

        public List<F_YS_PARTY_INFO> FYsPartyInfoList { get; set; }
        //public List<F_YS_GP_MASTER> FYsGpMasterList { get; set; }
        public List<BAS_YARN_COUNTINFO> BasYarnCountInfoList { get; set; }
        public List<BAS_YARN_LOTINFO> BasYarnLotInfoList { get; set; }
        public List<F_YS_LOCATION> FYsLocationList { get; set; }
        public List<F_YS_LEDGER> FYsLedgerList { get; set; }

        public List<F_YARN_TRANSACTION_TYPE> StockList { get; set; }

        public List<F_YS_GP_DETAILS> fysgpdetailsList { get; set; }

        public bool IsDelete { get; set; }
        public int RemoveIndex { get; set; }


    }
}
