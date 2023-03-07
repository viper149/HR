using System.Collections.Generic;
using DenimERP.Models;

namespace DenimERP.ViewModels.Rnd
{
    public class RndFabricCountinfoAndRndYarnConsumptionViewModel
    {
        public RND_FABRIC_COUNTINFO rND_FABRIC_COUNTINFO { get; set; }
        public RND_YARNCONSUMPTION rND_YARNCONSUMPTION { get; set; }
        public List<RND_YARNCONSUMPTION> RndYarnconsumptions { get; set; }
    }
}
