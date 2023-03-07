using System.Collections.Generic;
using DenimERP.Models;

namespace DenimERP.ViewModels.Com
{
    public class ComExLcInfoWIthDetailsViewModel
    {
        public ComExLcInfoWIthDetailsViewModel()
        {
            cOM_EX_LCDETAILs = new List<COM_EX_LCDETAILS>();
            ComExPimasters = new List<COM_EX_PIMASTER>();
        }

        public COM_EX_LCINFO cOM_EX_LCINFO { get; set; }
        public BAS_BEN_BANK_MASTER NegoBank { get; set; }
        public BAS_BUYER_BANK_MASTER NtfyBank { get; set; }
        public List<COM_EX_LCDETAILS> cOM_EX_LCDETAILs { get; set; }
        public List<COM_EX_PIMASTER> ComExPimasters { get; set; }
    }
}
