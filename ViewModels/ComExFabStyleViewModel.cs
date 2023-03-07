using System.Collections.Generic;
using DenimERP.Models;

namespace DenimERP.ViewModels
{
    public class ComExFabStyleViewModel
    {
        public ComExFabStyleViewModel()
        {
            rND_FABRICINFOs = new List<RND_FABRICINFO>();
            bAS_BRANDINFOs = new List<BAS_BRANDINFO>();
            rND_FABRIC_COUNTINFOs = new List<RND_FABRIC_COUNTINFO>();
        }

        public COM_EX_FABSTYLE cOM_EX_FABSTYLE { get; set; }
        public RND_FABRICINFO rND_FABRICINFO { get; set; }
        public List<RND_FABRICINFO> rND_FABRICINFOs{ get; set; }
        public List<BAS_BRANDINFO> bAS_BRANDINFOs { get; set; }
        public List<RND_FABRIC_COUNTINFO> rND_FABRIC_COUNTINFOs{ get; set; }
        public string WarpCount { get; set; }
        public string WeftCount { get; set; }
        public string Construction { get; set; }
    }
}
