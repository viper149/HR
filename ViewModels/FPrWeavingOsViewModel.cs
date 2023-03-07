using System;
using System.Collections.Generic;
using DenimERP.Models;

namespace DenimERP.ViewModels
{
    public class FPrWeavingOsViewModel
    {
        public FPrWeavingOsViewModel()
        {
            CountList = new List<BAS_YARN_COUNTINFO>();
            LotList = new List<BAS_YARN_LOTINFO>();
            f_PR_WEAVING_OS = new F_PR_WEAVING_OS
            {
                OSDATE = DateTime.Now
            };
        }

        public List<POSOViewModel> PosoViewModels { get; set; }
        public List<TypeTableViewModel> PlProductionSetDistributions { get; set; }
        public List<BAS_YARN_COUNTINFO> CountList { get; set; }
        public List<BAS_YARN_LOTINFO> LotList { get; set; }
        public F_PR_WEAVING_OS f_PR_WEAVING_OS { get; set; }



    }
}
