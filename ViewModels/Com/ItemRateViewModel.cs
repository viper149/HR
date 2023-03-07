using System.Collections.Generic;
using DenimERP.Models;

namespace DenimERP.ViewModels.Com
{
    public class ItemRateViewModel
    {
        public COM_IMP_CSITEM_DETAILS CsItemDetails { get; set; }
        public List<COM_IMP_CSRAT_DETAILS> CsRatDetailsList { get; set; }
    }
}
