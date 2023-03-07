using System.Collections.Generic;
using DenimERP.Models;

namespace DenimERP.ViewModels
{
    public class FChemProductEntryViewModel
    {
        public FChemProductEntryViewModel()
        {
            FBasUnitsList = new List<F_BAS_UNITS>();
            FChemTypeList = new List<F_CHEM_TYPE>();
            Countries = new List<COUNTRIES>();
        }

        public F_CHEM_STORE_PRODUCTINFO FChemStoreProductinfo { get; set; }
        public BAS_PRODUCTINFO BasProductinfo { get; set; }

        public List<F_BAS_UNITS> FBasUnitsList { get; set; }
        public List<F_CHEM_TYPE> FChemTypeList { get; set; }
        public List<COUNTRIES> Countries { get; set; }
    }
}
