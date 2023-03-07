using System.Collections.Generic;
using System.Linq;

namespace DenimERP.ViewModels
{
    public class RndFabricInfoAndYarnConsumptionCtor
    {
        public List<RndFabricInfoAndYarnConsumption> Items { get; set; }

        public int NumberOfItems
        {
            get => Items.Count();
        }

        public RndFabricInfoAndYarnConsumptionCtor()
        {
            Items = new List<RndFabricInfoAndYarnConsumption>();
        }
    }
}
