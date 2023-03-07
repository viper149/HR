using System.Collections.Generic;
using DenimERP.Models;

namespace DenimERP.ViewModels.Com
{
    public class ComImpLcInformationEditViewModel : ComImpLcInformationForCreateViewModel
    {
        public ComImpLcInformationEditViewModel()
        {
            PrevComImpLcdetailses = new List<COM_IMP_LCDETAILS>();
        }

        public List<COM_IMP_LCDETAILS> PrevComImpLcdetailses { get; set; }
    }
}
