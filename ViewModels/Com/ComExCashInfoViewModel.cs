using System.Collections.Generic;
using DenimERP.Models;

namespace DenimERP.ViewModels.Com
{
    public class ComExCashInfoViewModel
    {
        public ComExCashInfoViewModel()
        {
            ComExLcInfos = new List<COM_EX_LCINFO>();
            CurrenciesList = new List<CURRENCY>();
        }

        public COM_EX_CASHINFO ComExCashInfo { get; set; }

        public List<COM_EX_LCINFO> ComExLcInfos { get; set; }
        public List<CURRENCY> CurrenciesList { get; set; }
    }
}
