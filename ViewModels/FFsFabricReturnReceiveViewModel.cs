using System.Collections.Generic;
using DenimERP.Models;

namespace DenimERP.ViewModels
{
    public class FFsFabricReturnReceiveViewModel
    {
        public FFsFabricReturnReceiveViewModel()
        {
            BuyerList = new List<BAS_BUYERINFO>();

            DoList = new List<ACC_EXPORT_DOMASTER>();

            FabList = new List<RND_FABRICINFO>();

            PiList = new List<COM_EX_PIMASTER>();

        }

        public F_FS_FABRIC_RETURN_RECEIVE f_FS_FABRIC_RETURN_RECEIVE { get; set; }
        public BAS_BUYERINFO bAS_BUYERINFO { get; set; }

        public F_YS_PARTY_INFO f_YS_PARTY_INFO { get; set; }

        public ACC_EXPORT_DOMASTER aCC_EXPORT_DOMASTER { get; set; }
        public RND_FABRICINFO rnD_FABRICINFO { get; set; }
        public COM_EX_PIMASTER coM_EX_PIMASTER { get; set; }

        public List<BAS_BUYERINFO> BuyerList { get; set; }

        public List<ACC_EXPORT_DOMASTER> DoList { get; set; }
        //public List<F_YS_GP_MASTER> FYsGpMasterList { get; set; }
        public List<RND_FABRICINFO> FabList { get; set; }
        public List<COM_EX_PIMASTER> PiList { get; set; }
       
        public bool IsDelete { get; set; }
        public int RemoveIndex { get; set; }
    }
}
