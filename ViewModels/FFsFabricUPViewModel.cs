using System.Collections.Generic;
using DenimERP.Models;

namespace DenimERP.ViewModels
{
    public class FFsFabricUPViewModel
    {
        public FFsFabricUPViewModel()
        {
            FFsFabricDetailsList = new List<F_FS_UP_DETAILS>();
            ComExLCInfoList = new List<COM_EX_LCINFO>();
        }
        public F_FS_UP_MASTER FFsUPMaster { get; set; }
        public F_FS_UP_DETAILS FFsFabricDetail { get; set; }
        public COM_EX_LCINFO ComExLCInfo { get; set; }
        public List<F_FS_UP_DETAILS> FFsFabricDetailsList { get; set; }
        public List<COM_EX_LCINFO> ComExLCInfoList { get; set; }


        public int RemoveIndex { get; set; }
        public bool IsDeletable { get; set; }
    }
}
