using System.Collections.Generic;
using DenimERP.Models;

namespace DenimERP.ViewModels
{
    public class AccLocalDoMasterViewModel
    {
        public AccLocalDoMasterViewModel()
        {
            ACC_LOCAL_DODETAILs = new List<ACC_LOCAL_DODETAILS>();
            COM_EX_SCINFOs = new List<COM_EX_SCINFO>();
            COM_EX_SCDETAILS = new List<COM_EX_SCDETAILS>();
            ComExLcinfos = new List<COM_EX_LCINFO>();
        }

        public ACC_LOCAL_DOMASTER ACC_LOCAL_DOMASTER { get; set; }
        public ACC_LOCAL_DODETAILS ACC_LOCAL_DODETAILS { get; set; }
        public COM_EX_SCINFO COM_EX_SCINFO { get; set; }

        public List<ACC_LOCAL_DODETAILS> ACC_LOCAL_DODETAILs { get; set; }
        public List<COM_EX_SCINFO> COM_EX_SCINFOs { get; set; } 
        public List<COM_EX_SCDETAILS> COM_EX_SCDETAILS { get; set; }
        public List<COM_EX_LCINFO> ComExLcinfos { get; set; }
    }
}
