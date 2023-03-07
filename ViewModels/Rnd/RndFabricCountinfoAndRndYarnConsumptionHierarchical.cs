using System.Collections.Generic;
using DenimERP.Models;

namespace DenimERP.ViewModels.Rnd
{
    public class RndFabricCountinfoAndRndYarnConsumptionHierarchical
    {
        public string EncryptedId { get; set; }
        public List<BAS_YARN_COUNTINFO> bAS_YARN_COUNTINFOs { get; set; }
        public List<BAS_YARN_LOTINFO> bAS_YARN_LOTINFOs { get; set; }
        public List<BAS_SUPPLIERINFO> bAS_SUPPLIERINFOs { get; set; }        
        public List<RndFabricCountinfoAndRndYarnConsumptionViewModel> Items { get; set; }

        public RndFabricCountinfoAndRndYarnConsumptionHierarchical()
        {
            Items = new List<RndFabricCountinfoAndRndYarnConsumptionViewModel>();
            bAS_YARN_COUNTINFOs = new List<BAS_YARN_COUNTINFO>();
            bAS_YARN_LOTINFOs = new List<BAS_YARN_LOTINFO>();
            bAS_SUPPLIERINFOs = new List<BAS_SUPPLIERINFO>();
        }
    }
}
