using DenimERP.Models;

namespace DenimERP.ViewModels.Com
{
    public class ComExLcInfoToExtendViewModel
    {
        public ComExLcInfoToExtendViewModel()
        {
            ComExLcInfo = new COM_EX_LCINFO();
        }

        public COM_EX_LCINFO ComExLcInfo { get; set; }
        public bool Override { get; set; }
        public double? PreviousAmount { get; set; }
    }
}
