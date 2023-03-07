using System.Collections.Generic;
using DenimERP.Models;

namespace DenimERP.ViewModels
{
    public class FWasteProductInfoViewModel
    {

        public FWasteProductInfoViewModel()
        {
            FBasUnitList = new List<F_BAS_UNITS>();
        }

        public F_WASTE_PRODUCTINFO F_WASTE_PRODUCTINFO { get; set; }

        public List<F_BAS_UNITS> FBasUnitList { get; set; }
        
    }

}

