using System.Collections.Generic;
using DenimERP.Models;

namespace DenimERP.ViewModels.HR
{
    public class FBasHrdNationalityViewModel
    {
        public FBasHrdNationalityViewModel()
        {
            CurrencyList = new List<CURRENCY>();
        }

        public F_BAS_HRD_NATIONALITY FBasHrdNationality { get; set; }

        public List<CURRENCY> CurrencyList { get; set; }
    }
}
