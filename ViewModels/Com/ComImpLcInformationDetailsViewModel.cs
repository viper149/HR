using System.Collections.Generic;
using DenimERP.Models;
using DenimERP.ViewModels.Com.Import;

namespace DenimERP.ViewModels.Com
{
    public class ComImpLcInformationDetailsViewModel
    {
        public COM_IMP_LCINFORMATION cOM_IMP_LCINFORMATION { get; set; }
        public List<ComImpLcWithBasProductInfoViewModel> ComImpLcWithBasProductInfoViewModels { get; set; }
    }
}
