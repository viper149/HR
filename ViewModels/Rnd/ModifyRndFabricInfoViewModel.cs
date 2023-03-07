using System.Collections.Generic;
using DenimERP.Models;

namespace DenimERP.ViewModels.Rnd
{
    public class ModifyRndFabricInfoViewModel : RndFabricInfoViewModel
    {
        public ModifyRndFabricInfoViewModel()
        {
            rndFabricCountinfoAndRndYarnConsumptionViewModels = new List<RndFabricCountinfoAndRndYarnConsumptionViewModel>();
        }

        public List<RND_FABRIC_COUNTINFO> rND_FABRIC_COUNTINFOs { get; set; }
        public List<RND_YARNCONSUMPTION> rND_YARNCONSUMPTIONs { get; set; }
        public List<RndFabricCountinfoAndRndYarnConsumptionViewModel> rndFabricCountinfoAndRndYarnConsumptionViewModels { get; set; }
        public int RemoveIndex { get; set; }
    }
}
